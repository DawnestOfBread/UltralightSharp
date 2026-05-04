using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp.Interfaces;

public interface ISurfaceDefinition
{
	/// <summary>The callback invoked when a Surface is created.</summary>
	/// <param name="width">The width in pixels.</param>
	/// <param name="height">The height in pixels.</param>
	/// <returns>
	/// This callback should return a pointer to user-defined data for the instance.
	/// This user data pointer will be passed to all other callbacks when operating on the instance.
	/// </returns>
	IntPtr Create(uint width, uint height);
	
	/// <summary>The callback invoked when a Surface is destroyed.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	void Destroy(IntPtr userData);
	
	/// <summary>The callback invoked when a Surface's width (in pixels) is requested.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	uint GetWidth(IntPtr userData);
	
	/// <summary>The callback invoked when a Surface's height (in pixels) is requested.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	uint GetHeight(IntPtr userData);
	
	// <summary>The callback invoked when a Surface's row bytes is requested.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	/// <remarks>This value is also known as <c>stride</c>. Usually width * 4.</remarks>
	uint GetRowBytes(IntPtr userData);
	
	/// <summary>The callback invoked when a Surface's size (in bytes) is requested.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	ulong GetSize(IntPtr userData);
	
	/// <summary>
	/// The callback invoked when a Surface's pixel buffer is requested to be locked for reading/writing (should return a pointer to locked bytes).
	/// </summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	IntPtr LockPixels(IntPtr userData);

	/// <summary>
	/// The callback invoked when a Surface's pixel buffer is requested to be unlocked after previously
	/// being locked.
	/// </summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	void UnlockPixels(IntPtr userData);
	
	/// <summary>The callback invoked when a Surface is requested to be resized to a certain width/height.</summary>
	/// <param name="userData">User data pointer uniquely identifying the surface.</param>
	/// <param name="width">Width in pixels.</param>
	/// <param name="height">Height in pixels.</param>
	void Resize(IntPtr userData, uint width, uint height);
}