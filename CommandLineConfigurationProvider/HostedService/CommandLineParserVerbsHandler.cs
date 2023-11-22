using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace brinux.CommandLineConfigurationProvider
{
	public class CommandLineParserVerbsHandler : IHostedService
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly IConfiguration _config;
		private readonly IHostApplicationLifetime _applicationLifetime;

		public CommandLineParserVerbsHandler(
			IServiceProvider sericeProvider,
			IConfiguration config,
			IHostApplicationLifetime applicationLifetime)
		{
			_serviceProvider = sericeProvider;
			_config = config;
			_applicationLifetime = applicationLifetime;
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			if (string.IsNullOrEmpty(_config.GetSection(CommandLineParserConfigurationProvider.APPLICATION_HELP_REQUEST).Value))
			{
				var verbName = _config.GetSection(CommandLineParserConfigurationProvider.APPLICATION_MAIN_COMMAND_NAME).Value;

				var verbHandler = CommandLineParserConfigurationTools.GetCommandLineVerbReferencesByName(verbName);

				var interfaces = verbHandler.GetInterfaces();

				if (interfaces == null || interfaces.Length != 1)
				{
					throw new ApplicationException("CommandLineParser expect the verb handling class to be injected via DI, and thus it must implement an interface.");
				}

				var verbHandlerInstance = _serviceProvider.GetRequiredService(interfaces[0]);

				var verbHandlerMethod = CommandLineParserConfigurationTools.GetCommandLineVerbHandler(verbHandler);

				if ((AsyncStateMachineAttribute)verbHandlerMethod.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
				{
					// Async method
					var execution = (Task)verbHandlerMethod.Invoke(verbHandlerInstance, new object[] { });

					await execution.ConfigureAwait(false);
				}
				else
				{
					// Sync method
					verbHandlerMethod.Invoke(verbHandlerInstance, new object[] { });
				}
			}

			_applicationLifetime.StopApplication();
		}

		public async Task StopAsync(CancellationToken cancellationToken) {}
	}
}