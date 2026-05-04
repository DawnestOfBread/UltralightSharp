using System.Runtime.InteropServices;
using Ultralight;
using UltralightSharp.Structs;

namespace UltralightSharp;

public sealed class UlSurface
{
	internal readonly C_Surface Surface;
	
	internal UlSurface(C_Surface surface)
	{
		Surface = surface;
	}
	
	/// <summary>Width (in pixels).</summary>
	public uint Width => CAPI_Surface.UlSurfaceGetWidth(Surface);
	
	/// <summary>Height (in pixels).</summary>
	public uint Height => CAPI_Surface.UlSurfaceGetHeight(Surface);
	
	/// <summary>Number of bytes between rows (usually width * 4)</summary>
	public uint RowBytes => CAPI_Surface.UlSurfaceGetRowBytes(Surface);
	
	/// <summary>Size in bytes.</summary>
	public ulong Size => CAPI_Surface.UlSurfaceGetSize(Surface);
	
	/// <summary>Get the underlying Bitmap from the default Surface.</summary>
	/// <remarks>Do not call ulDestroyBitmap() on the returned value, it is owned by the surface.</remarks>
	public UlBitmap Bitmap => UlBitmap.CreateFromExisting(CAPI_Surface.UlBitmapSurfaceGetBitmap(Surface));
	
	/// <summary>Lock the pixel buffer and get a pointer to the beginning of the data for reading/writing.</summary>
	/// <remarks>Native pixel format is premultiplied BGRA 32-bit (8 bits per channel).</remarks>
	public IntPtr LockPixels() => CAPI_Surface.UlSurfaceLockPixels(Surface);
	
	/// <summary>Unlock the pixel buffer.</summary>
	public void UnlockPixels() => CAPI_Surface.UlSurfaceUnlockPixels(Surface);
	
	/// <summary>Resize the pixel buffer to a certain width and height (both in pixels).</summary>
	/// <remarks>This should never be called while pixels are locked.</remarks>
	public void Resize(uint width, uint height) => CAPI_Surface.UlSurfaceResize(Surface, width, height);
	
	/// <summary>Get the dirty bounds.</summary>
	/// <remarks>
	/// <para>This value can be used to determine which portion of the pixel buffer has been updated since the
	/// last call to ulSurfaceClearDirtyBounds().</para>
	/// <para>The general algorithm to determine if a Surface needs display is:
	/// <code>
	/// if (!ulIntRectIsEmpty(ulSurfaceGetDirtyBounds(surface))) {
	///		// Surface pixels are dirty and needs display.
	///		// Cast Surface to native Surface and use it here (pseudocode)
	///		DisplaySurface(surface);
	///		// Once you're done, clear the dirty bounds:
	///		ulSurfaceClearDirtyBounds(surface);
	/// }
	/// </code>
	/// </para>
	/// </remarks>
	public UlIntRect GetDirtyBounds()
	{
		var inter = CAPI_Surface.__Internal.UlSurfaceGetDirtyBounds(Surface.__Instance);
		return new UlIntRect
		{
			Left = inter.left,
			Top = inter.top,
			Right = inter.right,
			Bottom = inter.bottom
		};
	}
	
	/// <summary>Set the dirty bounds to a certain value.</summary>
	/// <remarks>
	/// <para>This is called after the Renderer paints to an area of the pixel buffer. (The new value will be
	/// joined with the existing dirty_bounds())</para>
	/// </remarks>
	public void SetDirtyBounds(UlIntRect bounds)
	{
		var inter = new ULIntRect.__Internal
		{
			left = bounds.Left,
			top = bounds.Top,
			right = bounds.Right,
			bottom = bounds.Bottom
		};
		CAPI_Surface.__Internal.UlSurfaceSetDirtyBounds(Surface.__Instance, inter);
	}

	/// <summary>Clear the dirty bounds.</summary>
	/// <remarks>You should call this after you're done displaying the Surface.</remarks>
	public void ClearDirtyBounds() => CAPI_Surface.UlSurfaceClearDirtyBounds(Surface);

	/// <summary>
	/// Get the underlying user data pointer (this is only valid if you have set a custom surface
	/// implementation via ulPlatformSetSurfaceDefinition).
	/// </summary>
	/// <remarks>This will return nullptr if this surface is the default ULBitmapSurface.</remarks>
	public IntPtr UserData => CAPI_Surface.UlSurfaceGetUserData(Surface);
}