using CppSharp;

namespace Generator;

internal class Program
{
	private static void Main(string[] args)
	{
		if (args.Length == 0)
			throw new Exception("You must specify an input directory.");
		Console.WriteLine("Generating bindings...");
		var generator = new UltralightLibrary(args[0]);
		ConsoleDriver.Run(generator);
	}
}