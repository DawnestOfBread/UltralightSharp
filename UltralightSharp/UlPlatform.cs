using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Ultralight;
using UltralightSharp.Interfaces;

namespace UltralightSharp;

public static class UlPlatform
{
	// Silly little hack to prevent delegates from being garbage collected
	// If you have a better idea to do this, please go ahead
	private static readonly ConditionalWeakTable<object, object> DelegateAnchors = new();
	
	/// <summary>
	/// Set a custom Logger implementation.
	/// <br/><br/>
	/// This is used to log debug messages to the console or to a log file.
	/// <br/><br/>
	/// You should call this before ulCreateRenderer() or ulCreateApp().
	/// </summary>
	/// <note>
	/// ulCreateApp() will use the default logger if you never call this.
	/// <br/><br/>
	/// If you're not using ulCreateApp(), (eg, using ulCreateRenderer()) you can still use the default logger by calling ulEnableDefaultLogger()
	/// </note>
	public static ILogger Logger
	{
		get;
		set
		{
			field = value;
			_logger?.Dispose();
			
			object[] delegates =
			[
				(ULLoggerLogMessageCallback)value.LogMessageInternal,
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_logger = new ULLogger
			{
				LogMessage = (ULLoggerLogMessageCallback)delegates[0]
			};
			CAPI_Platform.UlPlatformSetLogger(_logger);
		}
	}
	private static ULLogger _logger;
	
	/// <summary>
	/// Set a custom FileSystem implementation.
	/// <br/><br/>
	/// The library uses this to load all file URLs (eg, <c>file:///page.html</c>).
	/// <br/><br/>
	/// You can provide the library with your own FileSystem implementation so that file assets are
	/// loaded from your own pipeline.
	/// <br/><br/>
	/// You should call this before ulCreateRenderer() or ulCreateApp().
	/// </summary>
	/// <note>
	/// ulCreateApp() will use the default platform file system if you never call this.
	/// <br/><br/>
	/// If you're not using ulCreateApp(), (eg, using ulCreateRenderer()) you can still use the default platform file system by calling ulEnablePlatformFileSystem()'
	/// </note>
	/// <warning>
	/// This is required to be defined before calling ulCreateRenderer()
	/// </warning>
	public static IFileSystem FileSystem
	{
		get;
		set
		{
			field = value;
			_fileSystem?.Dispose();

			object[] delegates =
			[
				(ULFileSystemFileExistsCallback)value.FileExistsInternal,
				(ULFileSystemGetFileMimeTypeCallback)value.GetFileMimeTypeInternal,
				(ULFileSystemGetFileCharsetCallback)value.GetFileCharsetInternal,
				(ULFileSystemOpenFileCallback)value.OpenFileInternal
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_fileSystem = new ULFileSystem
			{
				FileExists = (ULFileSystemFileExistsCallback)delegates[0],
				GetFileMimeType = (ULFileSystemGetFileMimeTypeCallback)delegates[1],
				GetFileCharset = (ULFileSystemGetFileCharsetCallback)delegates[2],
				OpenFile = (ULFileSystemOpenFileCallback)delegates[3]
			};
			CAPI_Platform.UlPlatformSetFileSystem(_fileSystem);
		}
	}
	private static ULFileSystem _fileSystem;
	
	/// <summary>
	/// Set a custom FontLoader implementation.
	/// <br/><br/>
	/// The library uses this to load all system fonts.
	/// <br/><br/>
	/// Every operating system has its own library of installed system fonts. The FontLoader interface
	/// is used to look up these fonts and fetch the actual font data (raw TTF/OTF file data) for a
	/// given font description.
	/// <br/><br/>
	/// You should call this before ulCreateRenderer() or ulCreateApp().
	/// <br/><br/>
	/// </summary>
	/// <note>
	/// ulCreateApp() will use the default platform font loader if you never call this.
	/// <br/><br/>
	/// If you're not using ulCreateApp(), (eg, using ulCreateRenderer()) you can still use the
	/// default platform font loader by calling ulEnablePlatformFontLoader()'
	/// </note>
	/// <warning>
	/// This is required to be defined before calling ulCreateRenderer()
	/// </warning>
	public static IFontLoader FontLoader
	{
		get;
		set
		{
			field = value;
			_fontLoader?.Dispose();
			
			object[] delegates =
			[
				(ULFontLoaderGetFallbackFont)value.GetFallbackFontInternal,
				(ULFontLoaderGetFallbackFontForCharacters)value.GetFallbackFontForCharactersInternal,
				(ULFontLoaderLoad)value.LoadInternal,
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_fontLoader = new ULFontLoader
			{
				GetFallbackFont = (ULFontLoaderGetFallbackFont)delegates[0],
				GetFallbackFontForCharacters = (ULFontLoaderGetFallbackFontForCharacters)delegates[1],
				Load = (ULFontLoaderLoad)delegates[2]
			};
			CAPI_Platform.UlPlatformSetFontLoader(_fontLoader);
		}
	}
	private static ULFontLoader _fontLoader;
	
	/// <summary>
	/// Set a custom Surface implementation.
	/// <br/><br/>
	/// This can be used to wrap a platform-specific GPU texture, Windows DIB, macOS CGImage, or any
	/// other pixel buffer target for display on screen.
	/// <br/><br/>
	/// By default, the library uses a bitmap surface for all surfaces, but you can override this by
	/// providing your own surface definition here.
	/// <br/><br/>
	/// You should call this before ulCreateRenderer() or ulCreateApp().
	/// </summary>
	public static ISurfaceDefinition SurfaceDefinition
	{
		get;
		set
		{
			field = value;
			_surfaceDefinition?.Dispose();
			
			object[] delegates =
			[
				(ULSurfaceDefinitionCreateCallback)value.Create,
				(ULSurfaceDefinitionDestroyCallback)value.Destroy,
				(ULSurfaceDefinitionGetWidthCallback)value.GetWidth,
				(ULSurfaceDefinitionGetHeightCallback)value.GetHeight,
				(ULSurfaceDefinitionGetRowBytesCallback)value.GetRowBytes,
				(ULSurfaceDefinitionGetSizeCallback)value.GetSize,
				(ULSurfaceDefinitionLockPixelsCallback)value.LockPixels,
				(ULSurfaceDefinitionUnlockPixelsCallback)value.UnlockPixels,
				(ULSurfaceDefinitionResizeCallback)value.Resize,
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_surfaceDefinition = new ULSurfaceDefinition
			{
				Create = (ULSurfaceDefinitionCreateCallback)delegates[0],
				Destroy = (ULSurfaceDefinitionDestroyCallback)delegates[1],
				GetWidth = (ULSurfaceDefinitionGetWidthCallback)delegates[2],
				GetHeight = (ULSurfaceDefinitionGetHeightCallback)delegates[3],
				GetRowBytes = (ULSurfaceDefinitionGetRowBytesCallback)delegates[4],
				GetSize = (ULSurfaceDefinitionGetSizeCallback)delegates[5],
				LockPixels = (ULSurfaceDefinitionLockPixelsCallback)delegates[6],
				UnlockPixels = (ULSurfaceDefinitionUnlockPixelsCallback)delegates[7],
				Resize = (ULSurfaceDefinitionResizeCallback)delegates[8],
			};
			CAPI_Platform.UlPlatformSetSurfaceDefinition(_surfaceDefinition);
		}
	}
	private static ULSurfaceDefinition _surfaceDefinition;
	
	/// <summary>
	/// Set a custom GPUDriver implementation.
	/// <br/><br/>
	/// This should be used if you have enabled the GPU renderer in the Config and are using
	/// ulCreateRenderer() (which does not provide its own GPUDriver implementation).
	/// <br/><br/>
	/// The GPUDriver interface is used by the library to dispatch GPU calls to your native GPU context
	/// (eg, D3D11, Metal, OpenGL, Vulkan, etc.) There are reference implementations for this interface
	/// in the AppCore repo.
	/// <br/><br/>
	/// You should call this before ulCreateRenderer().
	/// </summary>
	public static IGpuDriver GpuDriver
	{
		get;
		set
		{
			field = value;
			_gpuDriver?.Dispose();
			
			object[] delegates =
			[
				(ULGPUDriverBeginSynchronizeCallback)value.BeginSynchronize,
				(ULGPUDriverEndSynchronizeCallback)value.EndSynchronize,
				(ULGPUDriverNextTextureIdCallback)value.NextTextureId,
				(ULGPUDriverCreateTextureCallback)value.CreateTextureInternal,
				(ULGPUDriverUpdateTextureCallback)value.UpdateTextureInternal,
				(ULGPUDriverDestroyTextureCallback)value.DestroyTexture,
				(ULGPUDriverNextRenderBufferIdCallback)value.NextRenderBufferId,
				(ULGPUDriverCreateRenderBufferCallback)value.CreateRenderBufferInternal,
				(ULGPUDriverDestroyRenderBufferCallback)value.DestroyRenderBuffer,
				(ULGPUDriverNextGeometryIdCallback)value.NextGeometryId,
				(ULGPUDriverCreateGeometryCallback)value.CreateGeometryInternal,
				(ULGPUDriverUpdateGeometryCallback)value.UpdateGeometryInternal,
				(ULGPUDriverDestroyGeometryCallback)value.DestroyGeometry,
				(ULGPUDriverUpdateCommandListCallback)value.UpdateCommandListInternal,
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_gpuDriver = new ULGPUDriver
			{
				BeginSynchronize = (ULGPUDriverBeginSynchronizeCallback)delegates[0],
				EndSynchronize = (ULGPUDriverEndSynchronizeCallback)delegates[1],
				NextTextureId = (ULGPUDriverNextTextureIdCallback)delegates[2],
				CreateTexture = (ULGPUDriverCreateTextureCallback)delegates[3],
				UpdateTexture = (ULGPUDriverUpdateTextureCallback)delegates[4],
				DestroyTexture = (ULGPUDriverDestroyTextureCallback)delegates[5],
				NextRenderBufferId = (ULGPUDriverNextRenderBufferIdCallback)delegates[6],
				CreateRenderBuffer = (ULGPUDriverCreateRenderBufferCallback)delegates[7],
				DestroyRenderBuffer = (ULGPUDriverDestroyRenderBufferCallback)delegates[8],
				NextGeometryId = (ULGPUDriverNextGeometryIdCallback)delegates[9],
				CreateGeometry = (ULGPUDriverCreateGeometryCallback)delegates[10],
				UpdateGeometry = (ULGPUDriverUpdateGeometryCallback)delegates[11],
				DestroyGeometry = (ULGPUDriverDestroyGeometryCallback)delegates[12],
				UpdateCommandList = (ULGPUDriverUpdateCommandListCallback)delegates[12],
			};
			CAPI_Platform.UlPlatformSetGPUDriver(_gpuDriver);
		}
	}
	private static ULGPUDriver _gpuDriver;
	
	public static IClipboard Clipboard
	{
		get;
		set
		{
			field = value;
			_clipboard?.Dispose();
			
			object[] delegates =
			[
				(ULClipboardClearCallback)value.Clear,
				(ULClipboardReadPlainTextCallback)value.ReadPlainTextInternal,
				(ULClipboardWritePlainTextCallback)value.WritePlainTextInternal,
			];
			DelegateAnchors.AddOrUpdate(value, delegates);
			
			_clipboard = new ULClipboard
			{
				Clear = (ULClipboardClearCallback)delegates[0],
				ReadPlainText = (ULClipboardReadPlainTextCallback)delegates[1],
				WritePlainText = (ULClipboardWritePlainTextCallback)delegates[2],
			};
			CAPI_Platform.UlPlatformSetClipboard(_clipboard);
		}
	}
	private static ULClipboard _clipboard;
}