using System;

namespace brinux.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandLineParserOptionsAttribute : Attribute
	{
		public CommandLineParserOptionsAttribute()
		{
		}
	}
}
