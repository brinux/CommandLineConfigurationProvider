using System;

namespace brinux.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Property)]
	public class CommandLineParserOptionAttribute : Attribute
	{
		public string Description;
		public bool Required = false;
		public string[] Alias;
		public Type Verb;

		public CommandLineParserOptionAttribute(string description)
		{
			Description = description;
		}
	}
}
