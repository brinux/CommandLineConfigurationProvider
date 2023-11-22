using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;

namespace brinux.CommandLineConfigurationProvider.examples
{
	class Program
	{
		static async Task Main(string[] args)
		{
			try
			{
				await CreateHostBuilder(args)
				.Build()
				.RunAsync();
			}
			catch (CommandLineConfigurationException e)
			{
				// This behavior is desider; it intercepts parsing errors which are already handled in the ConfigurationProvider
			}
		}

		static IHostBuilder CreateHostBuilder(string[] args)
		{
			return new HostBuilder()
				.ConfigureAppConfiguration((hostingContext, configuration) =>
				{
					configuration
						.AddJsonFile("appsettings.json", false)
						.AddCommandLineParser(args, rootCommand =>
						{
							rootCommand.Description = "Test application to demonstrate command line parsing.";
							rootCommand.TreatUnmatchedTokensAsErrors = true;
						});
				})
				.ConfigureServices((context, services) =>
				{
					services.AddOptions<MyOptions>().Bind(context.Configuration).ValidateDataAnnotations();

					services.AddScoped<IInfoVerb, InfoVerb>();
					services.AddScoped<IRunVerb, RunVerb>();
					services.AddScoped<ITestVerb, TestVerb>();

					services.AddCommandLineVerbsHandler();
				})
				.ConfigureLogging((hostingContext, logging) => {
					logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
					logging.AddConsole();
				});
		}
	}
}