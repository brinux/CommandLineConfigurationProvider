using Microsoft.Extensions.Configuration;
using System;
using System.CommandLine;

namespace brinux.CommandLineConfigurationProvider
{
	public class CommandLineParserConfigurationSource : IConfigurationSource
	{
		private string[] _args { get; set; } = Array.Empty<string>();
		private Action<RootCommand> _rootCommand = (rootCommand) => { };

		public CommandLineParserConfigurationSource(string[] args)
		{
			_args = args;
		}

		public CommandLineParserConfigurationSource(string[] args, Action<RootCommand> rootCommand)
		{
			_args = args;
			_rootCommand = rootCommand;
		}

		public IConfigurationProvider Build(IConfigurationBuilder builder)
		{
			return new CommandLineParserConfigurationProvider(_args, _rootCommand);
		}
	}
}
