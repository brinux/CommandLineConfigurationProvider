﻿using System;

namespace brinux.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandLineParserVerbAttribute : Attribute
	{
		public string Name;
		public string[] Aliases;
		public string Description;

		public CommandLineParserVerbAttribute(string verb, string description)
		{
			Name = verb;
			Description = description;
		}
	}
}
