using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface ILogger
{
	/// <summary>
	/// The callback invoked when the library wants to print a message to the log.
	/// </summary>
	/// <returns></returns>
	void LogMessage(ULLogLevel logLevel, string message);
	
	internal unsafe void LogMessageInternal(ULLogLevel logLevel, IntPtr message)
	{
		var ulStr = C_String.__CreateInstance(message);
		sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
		string managedStr = Marshal.PtrToStringAnsi((IntPtr)data)!;
		
		LogMessage(logLevel, managedStr);
	}
}