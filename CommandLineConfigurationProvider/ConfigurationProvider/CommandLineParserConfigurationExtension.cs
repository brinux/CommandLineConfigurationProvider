using Microsoft.Extensions.Configuration;
using System;
using System.CommandLine;

namespace brinux.CommandLineConfigurationProvider
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
