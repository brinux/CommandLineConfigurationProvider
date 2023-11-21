namespace CommandLineExample
{
	public interface IExecutor
	{
		Task Run();
		Task Revert();
		Task Test();
	}
}