using CommandLineExample;
using Microsoft.Extensions.Configuration.CommandLineConfigurationProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Verbs
{
	[CommandLineParserVerb("run", "Runs the application.", Aliases = new string[] { "-r" })]
	public class RunVerb : IRunVerb
	{
		private IOptions<MyOptions> _options;
		private ILogger<RunVerb> _log;

		public RunVerb(IOptions<MyOptions> options, ILogger<RunVerb> log)
		{
			_options = options;
			_log = log;
		}

		[CommandLineParserVerbHandler]
		public async Task Run()
		{
			_log.LogInformation("Method: run");
			_log.LogInformation($"Option {nameof(MyOptions.InputParameter)}: {_options.Value.InputParameter}");
			_log.LogInformation($"Option {nameof(MyOptions.ValueInRange)}: {_options.Value.ValueInRange}");
			_log.LogInformation($"Option {nameof(MyOptions.EnableSomething)}: {_options.Value.EnableSomething}");
		}
	}
}
