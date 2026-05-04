using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface IFontLoader
{
	/// <summary>
	/// Fallback font family name. Will be used if all other fonts fail to load.
	/// </summary>
	/// <note>
	/// This font should be guaranteed to exist (eg, ULFontLoader::load should not fail when passed this font family name).
	/// <br/><br/>
	/// The returned ULString instance will be consumed (ulDestroyString will be called on it).
	/// </note>
	/// <returns></returns>
	string GetFallbackFont();
	
	/// <summary>
	/// Fallback font family name that can render the specified characters. This is mainly used to
	/// support CJK (Chinese, Japanese, Korean) text display.
	/// </summary>
	/// <param name="characters">One or more UTF-16 characters. This is almost always a single character.</param>
	/// <param name="weight">Font weight.</param>
	/// <param name="italic">Whether italic is requested.</param>
	/// <returns>
	/// Should return a font family name that can render the text.
	/// The returned ULString instance will be consumed (ulDestroyString will be called on it).
	/// </returns>
	string GetFallbackFontForCharacters(string characters, int weight, bool italic);

	/// <summary>
	/// Get the actual font file data (TTF/OTF) for a given font description.
	/// </summary>
	/// <param name="family">Font family name.</param>
	/// <param name="weight">Font weight.</param>
	/// <param name="italic">Whether italic is requested.</param>
	/// <returns>
	/// A font file matching the given description (either an on-disk font filepath or an
	/// in-memory file buffer). You can return NULL here and the loader will fall back to
	/// another font.
	/// </returns>
	UlFontFile? Load(string family, int weight, bool italic);

	internal IntPtr GetFallbackFontInternal()
	{
		string @out = GetFallbackFont();
		var str = CAPI_String.UlCreateString(@out);
		return str.__Instance;
	}
	
	internal unsafe IntPtr GetFallbackFontForCharactersInternal(IntPtr characters, int weight, bool italic)
	{
		var ulStr = C_String.__CreateInstance(characters);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
		string managedStr = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		string @out = GetFallbackFontForCharacters(managedStr, weight, italic);
		var str = CAPI_String.UlCreateString(@out);
		return str.__Instance;
	}
	
	internal unsafe IntPtr LoadInternal(IntPtr family, int weight, bool italic)
	{
		var ulStr = C_String.__CreateInstance(family);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
		string managedStr = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		var @out = Load(managedStr, weight, italic);
		return @out?.FontFile.__Instance ?? IntPtr.Zero;
	}
}