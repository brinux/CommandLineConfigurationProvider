using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace brinux.CommandLineConfigurationProvider.examples
{
	[CommandLineParserVerb("info", "Gets some information", Aliases = new string[] { "-i" })]
	public class InfoVerb : IInfoVerb
	{
		private IOptions<MyOptions> _options;
		private ILogger<InfoVerb> _log;

		public InfoVerb(IOptions<MyOptions> options, ILogger<InfoVerb> log)
		{
			_options = options;
			_log = log;
		}

		[CommandLineParserVerbHandler]
		public async Task Info()
		{
			_log.LogInformation("Method: info");
			_log.LogInformation($"Option {nameof(MyOptions.InputParameter)}: {_options.Value.InputParameter}");
			_log.LogInformation($"Option {nameof(MyOptions.ValueInRange)}: {_options.Value.ValueInRange}");
			_log.LogInformation($"Option {nameof(MyOptions.EnableSomething)}: {_options.Value.EnableSomething}");
		}
	}
}
