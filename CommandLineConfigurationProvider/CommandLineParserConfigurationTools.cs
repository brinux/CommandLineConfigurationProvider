using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace brinux.CommandLineConfigurationProvider
{
	public static class CommandLineParserConfigurationTools
	{
		private static List<PropertyInfo> _registeredOptions;
		private static List<Type> _registeredVerbs;

		public static List<PropertyInfo> FindCommandLineGlobalOptions()
		{
			return FindRegisteredOptions().Where(o =>
				o.GetCustomAttribute<CommandLineParserOptionAttribute>().Verb == null
			).ToList();
		}

		public static List<PropertyInfo> FindCommandLineVerbOptions(Type verbType)
		{
			return FindRegisteredOptions().Where(o =>
			{
				var verbOptionAttribute = o.GetCustomAttribute<CommandLineParserOptionAttribute>().Verb;

				return verbOptionAttribute != null && verbOptionAttribute == verbType;
			}
			).ToList();
		}

		public static Type GetCommandLineVerbReferencesByName(string verbName)
		{
			return FindRegisteredVerbs().Single(t => t.GetCustomAttribute<CommandLineParserVerbAttribute>().Name.Equals(verbName));
		}

		public static MethodInfo GetCommandLineVerbHandler(Type verb)
		{
			return FindRegisteredVerbs()
				.Single(v => v == verb)
				.GetMethods()
				.Single(m => m.GetCustomAttribute<CommandLineParserVerbHandlerAttribute>() != null);
		}

		public static List<Type> FindRegisteredVerbs()
		{
			if (_registeredVerbs == null)
			{
				_registeredVerbs = AppDomain.CurrentDomain.GetAssemblies()
					.SelectMany(s => s.GetTypes())
					.Where(t => t.IsDefined(typeof(CommandLineParserVerbAttribute)))
					.ToList();
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
