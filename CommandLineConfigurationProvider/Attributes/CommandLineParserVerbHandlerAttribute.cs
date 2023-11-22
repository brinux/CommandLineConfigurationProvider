using System;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class CommandLineParserVerbHandlerAttribute : Attribute
	{
		public CommandLineParserVerbHandlerAttribute()
		{
		}
	}
}
