using Ultralight;

namespace UltralightSharp;

public sealed class UlFontFile : IDisposable
{
	internal readonly C_FontFile FontFile;
	
	/// <summary>Create a font file from an in-memory buffer.</summary>
	public UlFontFile(UlBuffer buffer)
	{
		FontFile = CAPI_FontFile.UlFontFileCreateFromBuffer(buffer.Buffer);
	}
	
	/// <summary>Create a font file from an on-disk file path.</summary>
	/// <remarks>The file path should already exist.</remarks>
	public UlFontFile(string path)
	{
		var str = CAPI_String.UlCreateString(path);
		FontFile = CAPI_FontFile.UlFontFileCreateFromFilePath(str);
		CAPI_String.UlDestroyString(str);
	}

	public void Dispose()
	{
		CAPI_FontFile.UlDestroyFontFile(FontFile);
	}
}