using System;

namespace brinux.CommandLineConfigurationProvider
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
	public class CommandLineParserVerbHandlerAttribute : Attribute
	{
		public CommandLineParserVerbHandlerAttribute()
		{
		}
	}
}
