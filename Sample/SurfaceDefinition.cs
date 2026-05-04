using Ultralight;
using UltralightSharp.Interfaces;

namespace Sample;

public class SurfaceDefinition(IntPtr renderer) : ISurfaceDefinition
{
	public SdlSurface GetSurface(IntPtr key) => _surfaces[key];
	
	private readonly Dictionary<IntPtr, SdlSurface> _surfaces = new();
	private readonly IntPtr _renderer = renderer;

	public IntPtr Create(uint width, uint height)
	{
		var surface = new SdlSurface(_renderer, width, height);
		_surfaces.Add(surface.TextureHandle, surface);
		return surface.TextureHandle;
	}

	public void Destroy(IntPtr userData)
	{
		if (_surfaces.TryGetValue(userData, out var surface))
			surface.Dispose();
	}

	public uint GetWidth(IntPtr userData)
	{
		return _surfaces.TryGetValue(userData, out var surface) ? surface.Width : 0;
	}

	public uint GetHeight(IntPtr userData)
	{
		return _surfaces.TryGetValue(userData, out var surface) ? surface.Height : 0;
	}

	public uint GetRowBytes(IntPtr userData)
	{
		if (_surfaces.TryGetValue(userData, out var surface))
			return surface.Bpp * surface.Width;

		return 0;
	}

	public ulong GetSize(IntPtr userData)
	{
		return _surfaces.TryGetValue(userData, out var surface) ? surface.Width * surface.Height * surface.Bpp : 0;
	}

	public IntPtr LockPixels(IntPtr userData)
	{
		return _surfaces.TryGetValue(userData, out var surface) ? surface.LockPixels() : IntPtr.Zero;
	}

	public void UnlockPixels(IntPtr userData)
	{
		if (_surfaces.TryGetValue(userData, out var surface))
			surface.UnlockPixels();
	}

	public void Resize(IntPtr userData, uint width, uint height)
	{
		if (_surfaces.TryGetValue(userData, out var surface))
			surface.Dispose();
		_surfaces[userData] =  new SdlSurface(_renderer, width, height);
	}
}