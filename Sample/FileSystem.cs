using UltralightSharp;
using UltralightSharp.Interfaces;

namespace Sample;

public class FileSystem : IFileSystem
{
	public bool FileExists(string path) => File.Exists(path);

	public string GetFileMimeType(string path)
	{
		return MimeTypes.GetMimeType(Path.GetFileName(path));
	}

	public string GetFileCharset(string path)
	{
		return "utf-8";
	}

	public UlBuffer? OpenFile(string path)
	{
		try
		{
			byte[] bytes = File.ReadAllBytes(path);
			var buffer = UlBuffer.CreateCopy(bytes);
			return buffer;
		}
		catch (Exception e)
		{
			Console.WriteLine(e);
			return null;
		}
	}
}