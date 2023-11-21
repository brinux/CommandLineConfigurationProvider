using System;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Class)]
	public class CommandLineParserOptionsAttribute : Attribute
	{
		public CommandLineParserOptionsAttribute()
		{
		}
	}
}
