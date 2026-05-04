using Ultralight;
using UltralightSharp.Interfaces;

namespace Sample;

public class Logger : ILogger
{
	public void LogMessage(ULLogLevel logLevel, string message)
	{
		Console.WriteLine($"[{logLevel}] {message}");
	}
}