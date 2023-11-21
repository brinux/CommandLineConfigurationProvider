using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	public static class CommandLineParserConfigurationTools
	{
		private static List<PropertyInfo> _registeredOptions;
		private static List<MethodInfo> _registeredVerbs;

		public static List<PropertyInfo> FindCommandLineGlobalOptions()
		{
			return FindRegisteredOptions().Where(o =>
				string.IsNullOrEmpty(o.GetCustomAttribute<CommandLineParserOptionAttribute>().Verb)
			).ToList();
		}

		public static List<PropertyInfo> FindCommandLineVerbOptions(string verbName)
		{
			return FindRegisteredOptions().Where(o =>
			{
				var verbOptionAttribute = o.GetCustomAttribute<CommandLineParserOptionAttribute>().Verb;

				return !string.IsNullOrEmpty(verbOptionAttribute) && (verbOptionAttribute == verbName);
			}
			).ToList();
		}

		public static MethodInfo GetCommandLineVerbReferencesByName(string verbName)
		{
			return FindRegisteredVerbs().Single(m => m.GetCustomAttribute<CommandLineParserVerbAttribute>().Verb.Equals(verbName));
		}

		public static List<MethodInfo> FindRegisteredVerbs()
		{
			if (_registeredVerbs == null)
			{
				var methods = new List<MethodInfo>();

				var optionClasses = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(s => s.GetTypes())
					.Where(t => t.IsDefined(typeof(CommandLineParserVerbsAttribute)));

				foreach (var optionClass in optionClasses)
				{
					methods.AddRange(optionClass.GetMethods().Where(m => Attribute.IsDefined(m, typeof(CommandLineParserVerbAttribute))));
				}

				_registeredVerbs = methods;
			}

			return _registeredVerbs;
		}

		private static List<PropertyInfo> FindRegisteredOptions()
		{
			if (_registeredOptions == null)
			{
				var options = new List<PropertyInfo>();

				var optionClasses = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(s => s.GetTypes())
					.Where(t => t.IsDefined(typeof(CommandLineParserOptionsAttribute)));

				foreach (var optionClass in optionClasses)
				{
					options.AddRange(optionClass.GetProperties().Where(p => Attribute.IsDefined(p, typeof(CommandLineParserOptionAttribute))));
				}

				_registeredOptions = options;
			}

			return _registeredOptions;
		}
	}
}
