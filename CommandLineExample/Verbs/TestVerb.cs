using CommandLineExample;
using brinux.CommandLineConfigurationProvider;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Verbs
{
	[CommandLineParserVerb("test", "Tests the application.", Aliases = new string[] { "-t" })]
	public class TestVerb : ITestVerb
	{
		private IOptions<MyOptions> _options;
		private ILogger<TestVerb> _log;

		public TestVerb(IOptions<MyOptions> options, ILogger<TestVerb> log)
		{
			_options = options;
			_log = log;
		}

		[CommandLineParserVerbHandler]
		public async Task Test()
		{
			_log.LogInformation("Method: test");
			_log.LogInformation($"Option {nameof(MyOptions.InputParameter)}: {_options.Value.InputParameter}");
			_log.LogInformation($"Option {nameof(MyOptions.ValueInRange)}: {_options.Value.ValueInRange}");
			_log.LogInformation($"Option {nameof(MyOptions.EnableSomething)}: {_options.Value.EnableSomething}");
		}
	}
}
