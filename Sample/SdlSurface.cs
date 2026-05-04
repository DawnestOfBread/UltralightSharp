using SDL3;

namespace Sample;

public sealed class SdlSurface : IDisposable
{
	public IntPtr TextureHandle { get; }
	public SdlSurface(IntPtr renderer, uint width, uint height)
	{
		TextureHandle = SDL.CreateTexture(renderer, SDL.PixelFormat.ARGB8888, SDL.TextureAccess.Streaming, (int)width, (int)height);
		if (TextureHandle == IntPtr.Zero)
			throw new SdlException("Failed to create texture");
	}

	public (int Width, int Height) Size
	{
		get
		{
			SDL.GetTextureSize(TextureHandle, out float width, out float height);
			return ((int, int))(width, height);
		}
	}

	public uint Width => (uint)Size.Width;
	public uint Height => (uint)Size.Height;
	public uint Bpp => SDL.BytesPerPixel(SDL.PixelFormat.ARGB8888);

	public IntPtr LockPixels()
	{
		SDL.LockTexture(TextureHandle, IntPtr.Zero, out IntPtr pixels, out int pitch);
		return pixels;
	}

	public void UnlockPixels()
	{
		SDL.UnlockTexture(TextureHandle);
	}

	public void Dispose()
	{
		SDL.DestroyTexture(TextureHandle);
	}
}