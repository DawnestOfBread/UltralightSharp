using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface IClipboard
{
	/// <summary>The callback invoked when the library wants to clear the system's clipboard.</summary>
	void Clear();
	
	/// <summary>The callback invoked when the library wants to read from the system's clipboard.</summary>
	string ReadPlainText();
	
	/// <summary>The callback invoked when the library wants to write to the system's clipboard.</summary>
	void WritePlainText(string plainText);
	
	internal void ReadPlainTextInternal(IntPtr result)
	{
		var ulStr = C_String.__CreateInstance(result);
		string @out = ReadPlainText();
		CAPI_String.UlStringAssignCString(ulStr, @out);
	}
	
	internal unsafe void WritePlainTextInternal(IntPtr text)
	{
		var ulStr = C_String.__CreateInstance(text);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
		string managedStr = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		WritePlainText(managedStr);
	}
}