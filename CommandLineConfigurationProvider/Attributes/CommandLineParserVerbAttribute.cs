using System;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Method)]
	public class CommandLineParserVerbAttribute : Attribute
	{
		public string Verb;
		public string Description;
		public string[] Alias;

		public CommandLineParserVerbAttribute(string verb, string description)
		{
			Verb = verb;
			Description = description;
		}
	}
}
