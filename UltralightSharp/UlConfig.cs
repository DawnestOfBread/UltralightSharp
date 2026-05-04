using Ultralight;

namespace UltralightSharp;

public sealed class UlConfig : IDisposable
{
	internal readonly C_Config Config;

	/// <summary>
	/// Create config with default values.
	/// </summary>
	public UlConfig()
	{
		Config = CAPI_Config.UlCreateConfig();
	}

	/// <summary>A writable OS file path to store persistent Session data in.</summary>
	/// <remarks>
	/// <para>This data may include cookies, cached network resources, indexed DB, etc.</para>
	/// <para>Files are only written to the path when using a persistent Session.</para>
	/// </remarks>
	public string CachePath
	{
		set
		{
			if (_cachePath != null && _cachePath.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_cachePath);
			_cachePath = CAPI_String.UlCreateString(value);
			CAPI_Config.UlConfigSetCachePath(Config, _cachePath);
		}
	}
	private C_String _cachePath;
	
	/// <summary>The relative path to the resources folder (loaded via the FileSystem API).</summary>
	/// <remarks>
	/// <para>The library loads certain resources (SSL certs, ICU data, etc.) from the FileSystem API during runtime (eg, <c>file:///resources/cacert.pem</c>).</para>
	/// <para>You can customise the relative file path to the resources folder by modifying this setting.</para>
	/// <para>(Default = <c>resources/</c>)</para>
	/// </remarks>
	public string ResourcePathPrefix
	{
		set
		{
			if (_resourcePathPrefix != null && _resourcePathPrefix.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_resourcePathPrefix);
			_resourcePathPrefix = CAPI_String.UlCreateString(value);
			CAPI_Config.UlConfigSetResourcePathPrefix(Config, _resourcePathPrefix);
		}
	}
	private C_String _resourcePathPrefix;

	/// <summary>The winding order for front-facing triangles.</summary>
	/// <remarks>
	/// <para>Only used when GPU rendering is enabled for the View.</para>
	/// <para>(Default = kFaceWinding_CounterClockwise)</para>
	/// </remarks>
	public ULFaceWinding FaceWinding
	{
		set => CAPI_Config.UlConfigSetFaceWinding(Config, value);
	}
	
	/// <summary>The hinting algorithm to use when rendering fonts. (Default = kFontHinting_Normal)</summary>
	/// <remarks>ULFontHinting</remarks>
	public ULFontHinting FontHinting
	{
		set => CAPI_Config.UlConfigSetFontHinting(Config, value);
	}
	
	/// <summary>
	/// <para>The gamma to use when compositing font glyphs, change this value to adjust contrast (Adobe and</para>
	/// <para>Apple prefer 1.8, others may prefer 2.2). (Default = 1.8)</para>
	/// </summary>
	public double FontGamma
	{
		set => CAPI_Config.UlConfigSetFontGamma(Config, value);
	}
	
	/// <summary>Global user-defined CSS string (included before any CSS on the page).</summary>
	/// <remarks>
	/// <para>You can use this to override default styles for various elements on the page.</para>
	/// <para>This is an actual string of CSS, not a file path.</para>
	/// </remarks>
	public string UserStylesheet
	{
		set
		{
			if (_userStylesheet != null && _userStylesheet.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_userStylesheet);
			_userStylesheet = CAPI_String.UlCreateString(value);
			CAPI_Config.UlConfigSetResourcePathPrefix(Config, _userStylesheet);
		}
	}
	private C_String _userStylesheet;

	/// <summary>Whether to continuously repaint any Views, regardless if they are dirty.</summary>
	/// <remarks>
	/// <para>This is mainly used to diagnose painting/shader issues and profile performance.</para>
	/// <para>(Default = False)</para>
	/// </remarks>
	public bool ForceRepaint
	{
		set =>  CAPI_Config.UlConfigSetForceRepaint(Config, value);
	}
	
	/// <summary>The delay (in seconds) between every tick of a CSS animation.</summary>
	/// <remarks>(Default = 1.0 / 60.0)</remarks>
	public double AnimationTimerDelay
	{
		set => CAPI_Config.UlConfigSetAnimationTimerDelay(Config, value);
	}
	
	/// <summary>The delay (in seconds) between every tick of a smooth scroll animation.</summary>
	/// <remarks>(Default = 1.0 / 60.0)</remarks>
	public double ScrollTimerDelay
	{
		set => CAPI_Config.UlConfigSetScrollTimerDelay(Config, value);
	}
	
	/// <summary>The delay (in seconds) between every call to the recycler.</summary>
	/// <remarks>
	/// <para>The library attempts to reclaim excess memory during calls to the internal recycler. You can change how often this is run by modifying this value.</para>
	/// <para>(Default = 4.0)</para>
	/// </remarks>
	public double RecycleDelay
	{
		set => CAPI_Config.UlConfigSetScrollTimerDelay(Config, value);
	}

	/// <summary>The size of WebCore's memory cache in bytes.</summary>
	/// <remarks>
	/// <para>You should increase this if you anticipate handling pages with large resources, Safari</para>
	/// <para>typically uses 128+ MiB for its cache.</para>
	/// <para>(Default = 64 * 1024 * 1024)</para>
	/// </remarks>
	public uint MemoryCacheSize
	{
		set => CAPI_Config.UlConfigSetMemoryCacheSize(Config, value);
	}
	
	/// <summary>The number of pages to keep in the cache. (Default: 0, none)</summary>
	/// <remarks>(Default = 0)</remarks>
	public uint PageCacheSize
	{
		set => CAPI_Config.UlConfigSetPageCacheSize(Config, value);
	}
	
	/// <summary>The system's physical RAM size in bytes.</summary>
	/// <remarks>
	/// <para>JavaScriptCore tries to detect the system's physical RAM size to set reasonable allocation</para>
	/// <para>limits. Set this to anything other than 0 to override the detected value. Size is in bytes.</para>
	/// <para>This can be used to force JavaScriptCore to be more conservative with its allocation strategy</para>
	/// <para>(at the cost of some performance).</para>
	/// </remarks>
	public uint OverrideRamSize
	{
		set => CAPI_Config.UlConfigSetOverrideRAMSize(Config, value);
	}
	
	/// <summary>The minimum size of large VM heaps in JavaScriptCore.</summary>
	/// <remarks>
	/// <para>Set this to a lower value to make these heaps start with a smaller initial value.</para>
	/// <para>(Default = 32 * 1024 * 1024)</para>
	/// </remarks>
	public uint MinLargeHeapSize
	{
		set => CAPI_Config.UlConfigSetMinLargeHeapSize(Config, value);
	}
	
	/// <summary>The minimum size of small VM heaps in JavaScriptCore.</summary>
	/// <remarks>
	/// <para>Set this to a lower value to make these heaps start with a smaller initial value.</para>
	/// <para>(Default = 1 * 1024 * 1024)</para>
	/// </remarks>
	public uint MinSmallHeapSize
	{
		set => CAPI_Config.UlConfigSetMinSmallHeapSize(Config, value);
	}
	
	/// <summary>The number of threads to use in the Renderer (for parallel painting on the CPU, etc.).</summary>
	/// <remarks>You can set this to a certain number to limit the number of threads to spawn.</remarks>
	public uint NumRendererThreads
	{
		set => CAPI_Config.UlConfigSetNumRendererThreads(Config, value);
	}
	
	/// <summary>
	/// <para>The max amount of time (in seconds) to allow repeating timers to run during each call to</para>
	/// <para>Renderer::Update.</para>
	/// </summary>
	/// <remarks>
	/// <para>The library will attempt to throttle timers if this time budget is exceeded.</para>
	/// <para>(Default = 1.0 / 200.0)</para>
	/// </remarks>
	public double MaxUpdateTime
	{
		set => CAPI_Config.UlConfigSetMaxUpdateTime(Config, value);
	}
	
	/// <summary>The alignment (in bytes) of the BitmapSurface when using the CPU renderer.</summary>
	/// <remarks>
	/// <para>The underlying bitmap associated with each BitmapSurface will have row_bytes padded to reach</para>
	/// <para>this alignment.</para>
	/// <para>Aligning the bitmap helps improve performance when using the CPU renderer. Determining the</para>
	/// <para>proper value to use depends on the CPU architecture and max SIMD instruction set used.</para>
	/// <para>We generally target the 128-bit SSE2 instruction set across most PC platforms so '16' is a safe</para>
	/// <para>value to use.</para>
	/// <para>You can set this to '0' to perform no padding (row_bytes will always be width * 4) at a slight</para>
	/// <para>cost to performance.</para>
	/// <para>(Default = 16)</para>
	/// </remarks>
	public uint BitmapAlignment
	{
		set => CAPI_Config.UlConfigSetBitmapAlignment(Config, value);
	}

	public void Dispose()
	{
		if (Config.__Instance != IntPtr.Zero) 
			CAPI_Config.UlDestroyConfig(Config);
		if (_resourcePathPrefix != null && _resourcePathPrefix.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_resourcePathPrefix);
		if (_cachePath != null && _cachePath.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_cachePath);		
		if (_userStylesheet != null && _userStylesheet.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_userStylesheet);
	}
}