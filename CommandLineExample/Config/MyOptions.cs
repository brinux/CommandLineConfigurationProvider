using Microsoft.Extensions.Configuration.CommandLineConfigurationProvider;
using System.ComponentModel.DataAnnotations;

namespace CommandLineExample
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

		[CommandLineParserOption("Option for 'test' verb.", Required = true, Verb = "test")]
		public string VerbOption { get; set; }
	}
}