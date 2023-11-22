using Microsoft.Extensions.DependencyInjection;

namespace brinux.CommandLineConfigurationProvider
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
