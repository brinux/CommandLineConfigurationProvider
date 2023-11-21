using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
{
	public static class CommandLineParserHostedServiceExtension
	{
		public static IServiceCollection AddCommandLineVerbsHandler(this IServiceCollection services)
		{
			services.AddHostedService<CommandLineParserVerbsHandler>();

			return services;
		}
	}
}
