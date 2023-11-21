using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Extensions.Configuration.CommandLineConfigurationProvider
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
				var verb = _config.GetSection(CommandLineParserConfigurationProvider.APPLICATION_MAIN_COMMAND_NAME).Value;

				var methodInfo = CommandLineParserConfigurationTools.GetCommandLineVerbReferencesByName(verb);

				var interfaces = methodInfo.DeclaringType.GetInterfaces();

				if (interfaces.Length == 0)
				{
					throw new ApplicationException("CommandLineParser expect the verb handling class to be injected via DI, and thus it must implement an interface.");
				}

				var methodClassInstance = _serviceProvider.GetRequiredService(interfaces[0]);

				if ((AsyncStateMachineAttribute)methodInfo.GetCustomAttribute(typeof(AsyncStateMachineAttribute)) != null)
				{
					// Async method
					var execution = (Task)methodInfo.Invoke(methodClassInstance, new object[] { });

					await execution.ConfigureAwait(false);
				}
				else
				{
					// Sync method
					methodInfo.Invoke(methodClassInstance, new object[] { });
				}
			}

			_applicationLifetime.StopApplication();
		}

		public async Task StopAsync(CancellationToken cancellationToken) {}
	}
}