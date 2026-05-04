using Ultralight;

namespace UltralightSharp;

public sealed class UlViewConfig : IDisposable
{
	internal readonly C_ViewConfig Config;

	/// <summary>
	/// Create view configuration with default values
	/// </summary>
	public UlViewConfig()
	{
		Config = CAPI_View.UlCreateViewConfig();
	}
	
	
	/// <summary>Set a user-generated id of the display (monitor, TV, or screen) that the View will be shown on.</summary>
	/// <remarks>
	/// Animations are driven based on the physical refresh rate of the display. Multiple Views can
	/// share the same display.
	/// This is automatically managed for you when ulCreateApp() is used.
	/// </remarks>
	public uint DisplayId
	{
		set => CAPI_View.UlViewConfigSetDisplayId(Config, value);
	}
	
	/// <summary>
	/// Set whether to render using the GPU renderer (accelerated) or the CPU renderer (unaccelerated).
	/// </summary>
	/// <remarks>
	/// <para>This option is only valid if you're managing the Renderer yourself (eg, you've previously
	/// called ulCreateRenderer() instead of ulCreateApp()).</para>
	/// <para>When true, the View will be rendered to an offscreen GPU texture using the GPU driver set in
	/// ulPlatformSetGPUDriver(). You can fetch details for the texture via ulViewGetRenderTarget().</para>
	/// <para>When false (the default), the View will be rendered to an offscreen pixel buffer using the
	/// multithreaded CPU renderer. This pixel buffer can optionally be provided by the user--
	/// for more info see ulViewGetSurface().</para>
	/// </remarks>
	public bool IsAccelerated
	{
		set => CAPI_View.UlViewConfigSetIsAccelerated(Config, value);
	}
	
	
	public bool IsTransparent
	{
		set => CAPI_View.UlViewConfigSetIsTransparent(Config, value);
	}
	
	/// <summary>
	/// Set the initial device scale, i.e. the amount to scale page units to screen pixels. This should be
	/// set to the scaling factor of the device that the View is displayed on. (Default = 1.0)
	/// </summary>
	/// <remarks>1.0 is equal to 100% zoom (no scaling), 2.0 is equal to 200% zoom (2x scaling)</remarks>
	public double InitialDeviceScale
	{
		set => CAPI_View.UlViewConfigSetInitialDeviceScale(Config, value);
	}
	
	/// <summary>Set whether the View should initially have input focus. (Default = True)</summary>
	public bool InitialFocus
	{
		set => CAPI_View.UlViewConfigSetInitialFocus(Config, value);
	}
	
	/// <summary>Set whether images should be enabled (Default = True).</summary>
	public bool EnableImages
	{
		set => CAPI_View.UlViewConfigSetEnableImages(Config, value);
	}
	
	/// <summary>Set whether JavaScript should be enabled (Default = True).</summary>
	public bool EnableJavaScript
	{
		set => CAPI_View.UlViewConfigSetEnableJavaScript(Config, value);
	}
	
	/// <summary>Set default font-family to use (Default = Times New Roman).</summary>
	public string FontFamilyStandard
	{
		set
		{
			if (_fontFamilyStandard != null && _fontFamilyStandard.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_fontFamilyStandard);
			_fontFamilyStandard = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewConfigSetFontFamilyStandard(Config, _fontFamilyStandard);
		}
	}
	private C_String _fontFamilyStandard;
	
	/// <summary>
	/// <para>Set default font-family to use for fixed fonts, eg <pre>and <code></para>
	/// <para>(Default = Courier New).</para>
	/// </summary>
	public string FontFamilyFixed
	{
		set
		{
			if (_fontFamilyFixed != null && _fontFamilyFixed.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_fontFamilyFixed);
			_fontFamilyFixed = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewConfigSetFontFamilyStandard(Config, _fontFamilyFixed);
		}
	}
	private C_String _fontFamilyFixed;
	
	/// <summary>Set default font-family to use for serif fonts (Default = Times New Roman).</summary>
	public string FontFamilySerif
	{
		set
		{
			if (_fontFamilySerif != null && _fontFamilySerif.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_fontFamilySerif);
			_fontFamilySerif = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewConfigSetFontFamilyStandard(Config, _fontFamilySerif);
		}
	}
	private C_String _fontFamilySerif;
	
	/// <summary>Set default font-family to use for sans-serif fonts (Default = Arial).</summary>
	public string FontFamilySansSerif
	{
		set
		{
			if (_fontFamilySansSerif != null && _fontFamilySansSerif.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_fontFamilySansSerif);
			_fontFamilySansSerif = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewConfigSetFontFamilyStandard(Config, _fontFamilySansSerif);
		}
	}
	private C_String _fontFamilySansSerif;
	
	/// <summary>Set the user agent string.</summary>
	public string UserAgent
	{
		set
		{
			if (_userAgent != null && _userAgent.__Instance != IntPtr.Zero)
				CAPI_String.UlDestroyString(_userAgent);
			_userAgent = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewConfigSetFontFamilyStandard(Config, _userAgent);
		}
	}
	private C_String _userAgent;

	public void Dispose()
	{
		if (Config.__Instance != IntPtr.Zero) 
			CAPI_View.UlDestroyViewConfig(Config);
		if (_fontFamilyStandard != null && _fontFamilyStandard.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_fontFamilyStandard);	
		if (_fontFamilyFixed != null && _fontFamilyFixed.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_fontFamilyFixed);
		if (_fontFamilySerif != null && _fontFamilySerif.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_fontFamilySerif);
		if (_fontFamilySansSerif != null && _fontFamilySansSerif.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_fontFamilySansSerif);
		if (_userAgent != null && _userAgent.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_userAgent);
	}
}