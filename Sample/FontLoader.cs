using UltralightSharp;
using UltralightSharp.Interfaces;

namespace Sample;

public class FontLoader : IFontLoader
{
	public string GetFallbackFont()
	{
		return "Noto Sans";
	}

	public string GetFallbackFontForCharacters(string characters, int weight, bool italic)
	{
		return "Noto Sans";
	}

	public UlFontFile? Load(string family, int weight, bool italic)
	{
		string filename = family.Replace(" ", "");
		if (italic)
			filename += "-Italic";
		filename += ".ttf";
		return File.Exists(filename) ? new UlFontFile(filename) : null;
	}
}