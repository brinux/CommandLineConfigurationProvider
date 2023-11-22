using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Parsing;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace brinux.CommandLineConfigurationProvider
{
	public class CommandLineParserConfigurationProvider : ConfigurationProvider
	{
		public const string APPLICATION_MAIN_COMMAND_NAME = "__SELECTED_COMMANDLINE_VERB__";
		public const string APPLICATION_HELP_REQUEST = "__APPLICATION_HELP_REQUEST__";

		private readonly IEnumerable<string> _args;
		private readonly Action<RootCommand> _rootCommand;

		public CommandLineParserConfigurationProvider(IEnumerable<string> args, Action<RootCommand> rootCommand)
		{
			_args = args;
			_rootCommand = rootCommand;
		}

		public override void Load()
		{
			var rootCommand = new RootCommand();

			_rootCommand(rootCommand);
			
			var registeredVerbs = CommandLineParserConfigurationTools.FindRegisteredVerbs();
			var registeredGlobalOptions = CommandLineParserConfigurationTools.FindCommandLineGlobalOptions();

			var commandLineOptions = ConvertRegisteredOptions(registeredGlobalOptions);

			foreach (var option in commandLineOptions)
			{
				rootCommand.AddGlobalOption(option);
			}

			foreach (var verb in registeredVerbs)
			{
				var attribute = verb.GetCustomAttribute(typeof(CommandLineParserVerbAttribute)) as CommandLineParserVerbAttribute;

				var currentVerb = new Command(attribute.Name, attribute.Description);

				var registeredVerbOptions = CommandLineParserConfigurationTools.FindCommandLineVerbOptions(verb);

				var commandLineVerbOptions = ConvertRegisteredOptions(registeredVerbOptions);

				foreach (var option in commandLineVerbOptions)
				{
					currentVerb.AddOption(option);
				}

				commandLineOptions.AddRange(commandLineVerbOptions);

				if (attribute.Aliases != null && attribute.Aliases.Count() > 0)
				{
					foreach (var alias in attribute.Aliases)
					{
						currentVerb.AddAlias(alias);
					}
				}

				rootCommand.AddCommand(currentVerb);
			}

			var parser = new CommandLineBuilder(rootCommand)
				.UseHelp()
				.UseSuggestDirective()
				.RegisterWithDotnetSuggest()
				.UseTypoCorrections()
				.UseParseErrorReporting()
				.Build();

			var result = parser.Invoke(_args.ToArray());

			if (result != 0)
			{
				throw new CommandLineConfigurationException();
			}
			else
			{
				AddParsedOptionToConfiguration(parser, commandLineOptions);
			}
		}

		private List<Option> ConvertRegisteredOptions(List<PropertyInfo> options)
		{
			var commandOptions = new List<Option>();

			foreach (var option in options)
			{
				var optionAttribute = option.GetCustomAttribute(typeof(CommandLineParserOptionAttribute)) as CommandLineParserOptionAttribute;

				var rangeAttribute = option.GetCustomAttribute(typeof(RangeAttribute)) as RangeAttribute;

				var optionType = typeof(Option<>).MakeGenericType(option.PropertyType);

				var commandLineOption = (Option) Activator.CreateInstance(optionType, new object[] { $"--{ option.Name }", null });
				commandLineOption.Description = optionAttribute.Description;
				commandLineOption.IsRequired = optionAttribute.Required;

				if (optionAttribute.Alias != null && optionAttribute.Alias.Count() > 0)
				{
					foreach (var alias in optionAttribute.Alias)
					{
						commandLineOption.AddAlias(alias);
					}
				}

				if (rangeAttribute != null)
				{
					commandLineOption.AddValidator(optionResult =>
					{
						optionResult.ErrorMessage = optionResult
							.Tokens
							.Select(t => t.Value)
							.Where(v => !int.TryParse(v, out int i) || i < (int)rangeAttribute.Minimum || i > (int)rangeAttribute.Maximum)
							.Select(_ => $"Il valore per l'Opzione { commandLineOption.Name } non rientra nel range { (int)rangeAttribute.Minimum }-{ (int)rangeAttribute.Maximum }.")
							.FirstOrDefault();
					});
				}

				commandOptions.Add(commandLineOption);
			}

			return commandOptions;
		}

		private void AddParsedOptionToConfiguration(Parser parser, List<Option> commandLineOptions)
		{
			var parseResult = parser.Parse(_args.ToList().AsReadOnly());

			Set(APPLICATION_MAIN_COMMAND_NAME, parseResult.CommandResult.Command.Name);

			foreach (var option in commandLineOptions)
			{
				if (ParseResultExtensions.HasOption(parseResult, option))
				{
					var value = parseResult.GetValueForOption(option).ToString();

					Set(option.Name, value);
				}
			}

			if (parser.Configuration.RootCommand.Options.Any(o => o.Name == "help") &&
				parseResult.HasOption(parser.Configuration.RootCommand.Options.First(o => o.Name == "help")))
			{
				Set(APPLICATION_HELP_REQUEST, "help");
			}
		}
	}
}