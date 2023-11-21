using System;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandLineParserVerbsAttribute : Attribute
	{
		public CommandLineParserVerbsAttribute()
		{
		}
	}
}
