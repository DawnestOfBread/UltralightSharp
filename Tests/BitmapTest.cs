using System.Runtime.InteropServices.Marshalling;
using Ultralight;

namespace Tests;

[TestClass]
public sealed class BitmapTest
{
	private static readonly string PngOutputBase = Path.Combine(AppContext.BaseDirectory, "test-output");

	static BitmapTest()
	{
		if (!Directory.Exists(PngOutputBase)) 
			Directory.CreateDirectory(PngOutputBase);
	}
	
	[TestMethod]
	public void EmptyBitmap()
	{
		var bitmap = CAPI_Bitmap.UlCreateEmptyBitmap();
		Assert.AreNotEqual(IntPtr.Zero, bitmap.__Instance);
		CAPI_Bitmap.UlBitmapErase(bitmap);
		bool didWrite = CAPI_Bitmap.UlBitmapWritePNG(bitmap, Path.Combine(PngOutputBase, "empty-bitmap.png"));
		Assert.AreNotEqual(true, didWrite);
		CAPI_Bitmap.UlDestroyBitmap(bitmap);
	}
	
	[TestMethod]
	public void Bitmap()
	{
		var bitmap = CAPI_Bitmap.UlCreateBitmap(10, 10, ULBitmapFormat.kBitmapFormatBGRA8UNORM_SRGB);
		Assert.AreNotEqual(IntPtr.Zero, bitmap.__Instance);
		bool didWrite = CAPI_Bitmap.UlBitmapWritePNG(bitmap, Path.Combine(PngOutputBase, "bitmap.png"));
		Assert.AreNotEqual(false, didWrite);
		CAPI_Bitmap.UlDestroyBitmap(bitmap);
	}
}