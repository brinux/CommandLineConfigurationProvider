using Microsoft.Extensions.Configuration.CommandLineConfigurationProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace CommandLineExample
{
	[CommandLineParserVerbs]
	public class Executor : IExecutor
	{
		private IOptions<MyOptions> _options;
		private ILogger<Executor> _log;

		public Executor(IOptions<MyOptions> options, ILogger<Executor> log)
		{
			_options = options;
			_log = log;
		}

		[CommandLineParserVerb("run", "Runs the application.")]
		public async Task Run()
		{
			_log.LogInformation("Method: run");
			_log.LogInformation($"Option { nameof(MyOptions.InputParameter) }: { _options.Value.InputParameter }");
			_log.LogInformation($"Option { nameof(MyOptions.ValueInRange) }: { _options.Value.ValueInRange }");
			_log.LogInformation($"Option { nameof(MyOptions.EnableSomething) }: { _options.Value.EnableSomething }");
		}

		[CommandLineParserVerb("revert", "Reverts the outcome.")]
		public async Task Revert()
		{
			_log.LogInformation("Method: revert");
			_log.LogInformation($"Option { nameof(MyOptions.InputParameter) }: { _options.Value.InputParameter }");
			_log.LogInformation($"Option { nameof(MyOptions.ValueInRange) }: { _options.Value.ValueInRange }");
			_log.LogInformation($"Option { nameof(MyOptions.EnableSomething) }: { _options.Value.EnableSomething }");
		}

		[CommandLineParserVerb("test", "Tests the application.")]
		public async Task Test()
		{
			_log.LogInformation("Method: test");
			_log.LogInformation($"Option { nameof(MyOptions.InputParameter) }: { _options.Value.InputParameter }");
			_log.LogInformation($"Option { nameof(MyOptions.ValueInRange) }: { _options.Value.ValueInRange }");
			_log.LogInformation($"Option { nameof(MyOptions.EnableSomething) }: { _options.Value.EnableSomething }");
			_log.LogInformation($"Option { nameof(MyOptions.VerbOption) }: { _options.Value.VerbOption }");
		}
	}
}
