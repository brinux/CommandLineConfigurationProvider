using System;
using System.CommandLine;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	public static class CommandLineParserConfigurationExtension
	{
		public static IConfigurationBuilder AddCommandLineParser(this IConfigurationBuilder configurationBuilder, string[] args)
		{
			return configurationBuilder.Add(new CommandLineParserConfigurationSource(args));
		}

		public static IConfigurationBuilder AddCommandLineParser(this IConfigurationBuilder configurationBuilder, string[] args, Action<RootCommand> rootCommand)
		{
			return configurationBuilder.Add(new CommandLineParserConfigurationSource(args, rootCommand));
		}
	}
}
