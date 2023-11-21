# Introduction 
The one implemented by this application is a potential approach to parsing command line parameters in a modern .Net applications with built-it Host leveraging C# attributes and a ConfigurationProvider based on System.CommanLine library.

# Getting Started
The project creates a custom ConfigurationProvider to be used with the Host ConfigurationBuilder. It parses the command line looking for parameters (options) and commands (verbs), imports them into application Configuration, and takes charge of the output in case it is required. C# Attributes allow developers to conveniently configure parsing behavior without "really" writing code.

# Build and Test
Input parameters (options) parsing is driven by property attributes. To explicitly mark a property as an attribute to be parsed, declare it like this:

```
// Indicated the class may contain "options"
[CommandLineParserOptions] 
public class MyOptions
{
	[Required]
	// Marks the property as an "option" and sets its description
	[CommandLineParserOption("Enables a specific feature.")] 
	public bool EnableSomething { get; set; }

	// Marks the property as an "option" but only for the "test" verb, for which it is also set as mandatory
	[CommandLineParserOption("Option for 'test' verb.", Required = true, Verb = "test")]
	public string VerbOption { get; set; }
	...
}
```
The applied attribute allows additional params:
* Required:  sets the command line parameter mandatory
* Alias: to set option aliases. At now, each option is expected to be provided as: --[OPTION_NAME] value
* Verb: registers the option as related to a specific verb only

Standard annotatation can be applied to option properties as well, and in particular [Range] will be leveraged to force the accepted input value range.
Please also notice that applying [Required] attribute to a property will mark it as required for the overall configuration, and not for the command line input. To mark as mandatory for the command line set the CommandLineParserOption attribute 'Required' property to true.

To read the parameters from the command line set the ConfigurationProvider in the ConfigurationBuilder.
```
await new HostBuilder()
	.ConfigureAppConfiguration((hostingContext, configuration) =>
	{
		configuration
			.AddJsonFile("appsettings.json", false)
			
			// This adds the ConfigurationProvider to the ConfigurationBuilder
			.AddCommandLineParser(args); 
	})
	.Build()
	.RunAsync();
```
If required, additional control on the root command can be achieved by the following overloading method:
```
.AddCommandLineParser(args, rootCommand =>
{
	rootCommand.Description = "Test application to demonstrate command line parsing.";
	rootCommand.TreatUnmatchedTokensAsErrors = true;
})
```
Command line commands (verbs) are threated in a similar way, but with different attributes.
```
// Indicated the class may contain "verbs"
[CommandLineParserVerbs] 
public class Executor : IExecutor
{
	// Associates the method via a verb named "run" and provides it a description
	[CommandLineParserVerb("run", "Runs the application.")] 
	public async Task Run()
	{ ... }
}
```
The applied attribute allows additional params:
* Alias: to set verb aliases.

To enable the handling of verbs, it is required to configure the HostBuilder adding a specific service:
```
await new HostBuilder()
	.ConfigureAppConfiguration((hostingContext, configuration) =>
	{
		configuration
			.AddJsonFile("appsettings.json", false)
			// This adds the ConfigurationProvider to the ConfigurationBuilder
			.AddCommandLineParser(args); 
	})
	.ConfigureServices((context, services) =>
	{
		// Since the class containing the method will be instantiated via Dependency Injected, it is required to register it
		services.AddScoped<IExecutor, Executor>();

		// This handles the presence of a verb in the provided command line input
		services.AddCommandLineVerbsHandler(); 
	})
	.Build()
	.RunAsync();
```
If a verb is provided in the command line input, the associated method is run.
Please notice that the calls containing the method gets created via Dependency Injection, and this is why in the example Executor is registered as IExecutor.

The overall suggested approach is to also leverage IOptions<> to define configuration entities served by Dependency Injection, that are also convenient entities to mark the input properties/option with the required attribute.

Overall, the configuration should be like this:
```
await new HostBuilder()
	.ConfigureAppConfiguration((hostingContext, configuration) =>
	{
		configuration
			.AddJsonFile("appsettings.json", false)
			// The Configuration Provider is set, parsing options and verbs
			.AddCommandLineParser(args, rootCommand =>
			{
				// Additional configuration is provided via the Action
				rootCommand.Description = "Test application to demonstrate command line parsing.";
				rootCommand.TreatUnmatchedTokensAsErrors = true;
			});
	})
	.ConfigureServices((context, services) =>
	{
		// Multiple options can be declared, and validated using standard attributes
		services.AddOptions<MyOptions>().Bind(context.Configuration).ValidateDataAnnotations();

		services.AddScoped<IExecutor, Executor>();

		// The verb handler intercepts the provided verb and run the associated method
		services.AddCommandLineVerbsHandler();
	})
	.Build()
	.RunAsync();
```
Once appropriately configured, the application will automatically produce a command like output like the following when needed:

	Description:
	  Test application to demonstrate command line parsing.

	Usage:
	  CommandLineExample [command] [options]

	Options:
	  --EnableSomething                  Enables a specific feature.
	  --InputParameter <InputParameter>  Provides the value for a specific parameter.
	  --ValueInRange <ValueInRange>      Provides a value with set boundaries (min: 1, max: 10).
	  -?, -h, --help                     Show help and usage information

	Commands:
	  run     Runs the application.
	  revert  Reverts the outcome.
	  test    Tests the application.

Also, command line output will highlight errors in the input in case they are present.

Many additional behaviors are actually available and possible. Please check the documentation for System.CommandLine library: https://github.com/dotnet/command-line-api

# Contribute
This application is NOT under active development, but don't hesitate to contact me for any support request or potential evolution.