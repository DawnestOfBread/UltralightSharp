using Ultralight;
using UltralightSharp;
using UltralightSharp.Interfaces;

namespace Tests;

[TestClass]
public sealed class PlatformTest
{
	private class FileSystem : IFileSystem
	{
		public bool FileExists(string path)
		{
			return path == "existing/file.ext";
		}

		public string GetFileMimeType(string path)
		{
			return "application/octet-stream";
		}

		public string GetFileCharset(string path)
		{
			return "utf-8";
		}

		public UlBuffer OpenFile(string path)
		{
			Span<byte> data = stackalloc byte[1024];
			return UlBuffer.CreateCopy(data);
		}
	}
	
	private class FontLoader : IFontLoader
	{
		public string GetFallbackFont()
		{
			return "Arial";
		}

		public string GetFallbackFontForCharacters(string characters, int weight, bool italic)
		{
			Console.WriteLine($"Characters: {characters}, Weight: {weight}, IsItalic: {italic}");
			return "Arial";
		}

		public UlFontFile? Load(string family, int weight, bool italic)
		{
			Console.WriteLine($"Family: {family}, Weight: {weight}, IsItalic: {italic}");
			return null;
		}
	}
	
	private class Logger : ILogger
	{
		public void LogMessage(ULLogLevel logLevel, string message)
		{
			Console.WriteLine($"[{logLevel}]: {message}");
		}
	}
	
	[TestMethod]
	public void BasicPlatformTest()
	{
		UlPlatform.FileSystem = new FileSystem();
		UlPlatform.FontLoader = new FontLoader();
		UlPlatform.Logger = new Logger();
	}
}