using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp;

public sealed class UlBuffer : IDisposable
{
	internal readonly C_Buffer Buffer;
	private UlBuffer(IntPtr data, ulong size, IntPtr userData, UlDestroyBufferCallback destructionCallback)
	{
		Buffer = CAPI_Buffer.UlCreateBuffer(data, size, userData, destructionCallback);
	}
	
	private UlBuffer(IntPtr data, ulong size)
	{
		Buffer = CAPI_Buffer.UlCreateBufferFromCopy(data, size);
	}
	
	/// <summary>
	/// Create a Buffer from existing, user-owned data without any copies. An optional, user-supplied
	/// callback will be called to deallocate data upon destruction
	/// </summary>
	/// <param name="data">A pointer to the data.</param>
	/// <param name="size">Size of the data in bytes.</param>
	/// <param name="userData">
	/// Optional user data that will be passed to destruction_callback when the returned Buffer is destroyed.
	/// </param>
	/// <param name="destructionCallback">
	/// Optional callback that will be called upon destruction. Pass a null pointer if you don't want to be informed of destruction.
	/// </param>
	public static UlBuffer Create(IntPtr data,
		ulong size,
		IntPtr userData,
		UlDestroyBufferCallback destructionCallback) =>
		new(data, size, userData, destructionCallback);
	
	/// <summary>Create a Buffer from existing data, a deep copy of data will be made.</summary>
	public static UlBuffer CreateCopy(IntPtr data, ulong size) => new(data, size);

	/// <inheritdoc cref="CreateCopy"/>
	public static unsafe UlBuffer CreateCopy<TData>(Span<TData> data) where TData : unmanaged
	{
		var size = (ulong)(sizeof(TData) * data.Length);
		fixed (void* ptr = data)
			return new UlBuffer((IntPtr)ptr, size);
	}
	
	/// <inheritdoc cref="CreateCopy"/>
	public static unsafe UlBuffer CreateCopy<TData>(TData[] data) where TData : unmanaged
	{
		var size = (ulong)(sizeof(TData) * data.Length);
		fixed (void* ptr = data)
			return new UlBuffer((IntPtr)ptr, size);
	}
	
	/// <summary>Get a pointer to the raw byte data.</summary>
	public IntPtr Data => CAPI_Buffer.UlBufferGetData(Buffer);
	
	/// <summary>Get the size in bytes.</summary>
	public ulong Size => CAPI_Buffer.UlBufferGetSize(Buffer);

	/// <summary>Get the user data associated with this Buffer, if any.</summary>
	public IntPtr UserData => CAPI_Buffer.UlBufferGetUserData(Buffer);
	
	/// <summary>
	/// <para>Check whether this Buffer owns its own data (Buffer was created via ulCreateBufferFromCopy).</para>
	/// <para>If this is false, Buffer will call the user-supplied destruction callback to deallocate data when this Buffer instance is destroyed.</para>
	/// </summary>
	public bool OwnsData => CAPI_Buffer.UlBufferOwnsData(Buffer);

	public void Dispose()
	{
		if (Buffer.__Instance != IntPtr.Zero) 
			CAPI_Buffer.UlDestroyBuffer(Buffer);
	}
}