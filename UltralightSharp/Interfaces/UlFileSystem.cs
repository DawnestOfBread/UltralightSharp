using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface IFileSystem
{
	/// <summary>
	/// The callback invoked when the FileSystem wants to check if a file path exists, return true if it
	/// exists.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	bool FileExists(string path);
	
	/// <summary>
	/// Get the mime-type of the file (eg "text/html").
	///
	/// This is usually determined by analysing the file extension.
	///
	/// If a mime-type cannot be determined, you should return "application/unknown" for this value.
	/// 
	/// The library will consume the result and call ulDestroyString() after this call returns.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	string GetFileMimeType(string path);
	
	/// <summary>
	/// Get the charset / encoding of the file (eg "utf-8").
	///
	/// This is only important for text-based files and is usually determined by analysing the
	/// contents of the file.
	///
	/// If a charset cannot be determined, it's usually safe to return "utf-8" for this value.
	/// 
	/// The library will consume the result and call ulDestroyString() after this call returns.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	string GetFileCharset(string path);
	
	/// <summary>
	/// Open file for reading and map it to a Buffer.
	///
	/// To minimise copies, you should map the requested file into memory and use ulCreateBuffer()
	/// to wrap the data pointer (unmapping should be performed in the destruction callback).
	///
	/// If the file was unable to be opened, you should return NULL for this value.
	/// </summary>
	/// <param name="path"></param>
	/// <returns></returns>
	UlBuffer? OpenFile(string path);

	internal unsafe bool FileExistsInternal(IntPtr path)
	{
		var ulStr = C_String.__CreateInstance(path);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
		string managedPath = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		return FileExists(managedPath);
	}

	internal unsafe IntPtr GetFileMimeTypeInternal(IntPtr path)
	{
		var ulStr = C_String.__CreateInstance(path);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
		string managedPath = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		string @out = GetFileMimeType(managedPath);
		var str = CAPI_String.UlCreateString(@out);
		return str.__Instance;
	}

	internal unsafe IntPtr GetFileCharsetInternal(IntPtr path)
	{
		var ulStr = C_String.__CreateInstance(path);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
		string managedPath = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		string @out = GetFileCharset(managedPath);
		var str = CAPI_String.UlCreateString(@out);
        return str.__Instance;
	}

	internal unsafe IntPtr OpenFileInternal(IntPtr path)
	{
		var ulStr = C_String.__CreateInstance(path);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
		string managedPath = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		var @out = OpenFile(managedPath);
		return @out?.Buffer.__Instance ?? IntPtr.Zero;
	}
}