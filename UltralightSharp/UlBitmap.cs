using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp;

public sealed class UlBitmap : IDisposable
{
	internal readonly C_Bitmap Bitmap;
	private bool _owns = true;
	
	private UlBitmap() => Bitmap = CAPI_Bitmap.UlCreateEmptyBitmap();
	private UlBitmap(uint width, uint height, ULBitmapFormat format) => Bitmap = CAPI_Bitmap.UlCreateBitmap(width, height, format);
	private UlBitmap(C_Bitmap bitmap, bool copy)
	{
		_owns = copy;
		Bitmap = copy ? CAPI_Bitmap.UlCreateBitmapFromCopy(bitmap) : bitmap;
	}

	private UlBitmap(uint width,
		uint height,
		ULBitmapFormat format,
		uint rowBytes,
		IntPtr pixels,
		ulong size,
		bool shouldCopy) =>
		Bitmap = CAPI_Bitmap.UlCreateBitmapFromPixels(width,
			height,
			format,
			rowBytes,
			pixels,
			size,
			shouldCopy);

	/// <summary>
	/// Create empty bitmap.
	/// </summary>
	public static UlBitmap CreateEmpty() => new();
	
	/// <summary>
	/// Create bitmap from copy.
	/// </summary>
	public static UlBitmap CreateFromCopy(UlBitmap bitmap) => new(bitmap.Bitmap, true);
	
	/// <summary>
	/// Create a managed bitmap representation from an existing bitmap.
	/// </summary>
	internal static UlBitmap CreateFromExisting(C_Bitmap bitmap) => new(bitmap, false);
	
	/// <summary>
	/// Create bitmap with certain dimensions and pixel format.
	/// </summary>
	public static UlBitmap Create(uint width, uint height, ULBitmapFormat format) => new(width, height, format);
	
	/// <summary>
	/// Create bitmap from existing pixel buffer.
	/// </summary>
	public static UlBitmap Create(uint width,
		uint height,
		ULBitmapFormat format,
		uint rowBytes,
		IntPtr pixels,
		ulong size,
		bool shouldCopy) => new(width, height, format, rowBytes, pixels, size, shouldCopy);
	
	/// <summary>
	/// Create bitmap from existing pixel buffer.
	/// </summary>
	/// <remarks>
	/// A copy of the data is made.
	/// </remarks>
	public static unsafe UlBitmap Create<TData>(uint width,
		uint height,
		ULBitmapFormat format,
		Span<TData> data) where TData : unmanaged
	{
		var size = (ulong)(sizeof(TData) * data.Length);
		var rowBytes = (uint)(size / height);
		fixed (void* ptr = data)
		{
			return new UlBitmap(width, height, format, rowBytes, (IntPtr)ptr, size, true);
		}
	}
	
	/// <summary>
	/// Get the width in pixels.
	/// </summary>
	public uint Width => CAPI_Bitmap.UlBitmapGetWidth(Bitmap);
	
	/// <summary>
	/// Get the height in pixels.
	/// </summary>
	public uint Height => CAPI_Bitmap.UlBitmapGetHeight(Bitmap);
	
	/// <summary>
	/// Get the number of bytes per row.
	/// </summary>
	public uint RowBytes => CAPI_Bitmap.UlBitmapGetRowBytes(Bitmap);
	
	/// <summary>
	/// Get the bytes per pixel.
	/// </summary>
	public uint BytesPerPixel => CAPI_Bitmap.UlBitmapGetBpp(Bitmap);
	
	/// <summary>
	/// Get the size in bytes of the underlying pixel buffer.
	/// </summary>
	public ulong Size => CAPI_Bitmap.UlBitmapGetSize(Bitmap);
	
	/// <summary>
	/// Get the pixel format.
	/// </summary>
	public ULBitmapFormat Format => CAPI_Bitmap.UlBitmapGetFormat(Bitmap);
	
	/// <summary>
	/// Whether this bitmap is empty.
	/// </summary>
	public bool IsEmpty => CAPI_Bitmap.UlBitmapIsEmpty(Bitmap);
	
	/// <summary>
	/// Get raw pixel buffer, you should only call this if the Bitmap is already locked.
	/// </summary>
	public IntPtr RawPixels => CAPI_Bitmap.UlBitmapRawPixels(Bitmap);
	
	/// <summary>
	/// Whether this bitmap owns its own pixel buffer.
	/// </summary>
	public bool OwnsPixels => CAPI_Bitmap.UlBitmapOwnsPixels(Bitmap);

    /// <summary>
    /// Write the bitmap to a PNG on disk
    /// </summary>
	public void WritePng(string path) => CAPI_Bitmap.UlBitmapWritePNG(Bitmap, path);
    
    /// <summary>
    /// Reset bitmap pixels to 0.
    /// </summary>
	public void Erase() => CAPI_Bitmap.UlBitmapErase(Bitmap);
    
    /// <summary>
    /// Lock pixels for reading/writing.
    /// </summary>
    /// <returns>Pointer to pixel buffer</returns>
	public IntPtr LockPixels() => CAPI_Bitmap.UlBitmapLockPixels(Bitmap);
    
    /// <summary>
    /// Unlock pixels after locking
    /// </summary>
	public void UnlockPixels() => CAPI_Bitmap.UlBitmapUnlockPixels(Bitmap);
    
    /// <summary>
    /// This converts a BGRA bitmap to RGBA bitmap and vice versa by swapping the red and blue channels.
    /// </summary>
	public void SwapRedBlueChannels() => CAPI_Bitmap.UlBitmapSwapRedBlueChannels(Bitmap);

	public void Dispose()
	{
		if (_owns && Bitmap.__Instance != IntPtr.Zero) 
			CAPI_Bitmap.UlDestroyBitmap(Bitmap);
	}
}