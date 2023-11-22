using System.ComponentModel.DataAnnotations;

namespace brinux.CommandLineConfigurationProvider.examples
{
	[CommandLineParserOptions]
	public class MyOptions
	{
		[Required]
		[CommandLineParserOption("Enables a specific feature.")]
		public bool EnableSomething { get; set; }

		[Required]
		[CommandLineParserOption("Provides the value for a specific parameter.")]
		public string InputParameter { get; set; }

		[Required]
		[Range(1, 10)]
		[CommandLineParserOption("Provides a value with set boundaries (min: 1, max: 10).")]
		public int ValueInRange { get; set; }

		[CommandLineParserOption("Option for 'test' verb.", Required = true, Verb = typeof(TestVerb))]
		public string VerbOption { get; set; }
	}
}