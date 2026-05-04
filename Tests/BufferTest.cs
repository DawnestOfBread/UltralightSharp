using System.Runtime.InteropServices;
using Ultralight;
using UlBuffer = UltralightSharp.UlBuffer;

namespace Tests;

[TestClass]
public sealed class BufferTest
{
	[TestMethod]
	public void Buffer()
	{
		const int size = 1024;
		IntPtr dataPtr = Marshal.AllocHGlobal(size);
		
		var buffer = CAPI_Buffer.UlCreateBuffer(dataPtr, size, 0, DestroyedDelegate);
		Assert.AreNotEqual(IntPtr.Zero, buffer.__Instance);
		CAPI_Buffer.UlDestroyBuffer(buffer);
	}
	
	[TestMethod]
	public void CopiedBuffer()
	{
		const int size = 1024;
		IntPtr dataPtr = Marshal.AllocHGlobal(size);
		
		var buffer = CAPI_Buffer.UlCreateBufferFromCopy(dataPtr, size);
		Marshal.FreeHGlobal(dataPtr);
		Assert.AreNotEqual(IntPtr.Zero, buffer.__Instance);
		CAPI_Buffer.UlDestroyBuffer(buffer);
	}
	
	private static void DestroyedDelegate(nint userData, nint data)
	{
		Console.WriteLine("Destroyed");
		Marshal.FreeHGlobal(data);
	}
}