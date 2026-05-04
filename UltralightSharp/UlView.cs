using System.Runtime.InteropServices;
using Ultralight;

namespace UltralightSharp;

public sealed class UlView : IDisposable
{
	internal readonly C_View View;
	internal UlView(UlRenderer renderer, uint width, uint height, UlViewConfig viewConfig, UlSession? session)
	{
		View = CAPI_View.UlCreateView(renderer.Renderer, width, height, viewConfig.Config, session?.Session ?? null);
	}
	
	/// <summary>Get or set the current URL.</summary>
	public unsafe string Url
	{
		get
		{
			var ulStr = CAPI_View.UlViewGetURL(View);
			sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
			string managedString = Marshal.PtrToStringAnsi((IntPtr)data)!;
			return managedString;
		}
		set
		{
			var ulStr = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewLoadURL(View, ulStr);
		}
	}
	
	/// <summary>Load a raw string of HTML.</summary>
	public string Html
	{
		set
		{
			var ulStr = CAPI_String.UlCreateString(value);
			CAPI_View.UlViewLoadHTML(View, ulStr);
		}
	}
	
	/// <summary>Get current title.</summary>
	public unsafe string Title
	{
		get
		{
			var ulStr = CAPI_View.UlViewGetTitle(View);
			sbyte* data = CAPI_String.UlStringGetData(ulStr);
			
			string managedString = Marshal.PtrToStringAnsi((IntPtr)data)!;
			return managedString;
		}
	}
	
	/// <summary>Get the width, in pixels.</summary>
	public uint Width => CAPI_View.UlViewGetWidth(View);
	
	/// <summary>Get the height, in pixels.</summary>
	public uint Height => CAPI_View.UlViewGetHeight(View);
	
	/// <summary>
	/// Get or set the display id of the View
	/// </summary>
	public uint DisplayId
	{
		get => CAPI_View.UlViewGetDisplayId(View);
		set => CAPI_View.UlViewSetDisplayId(View, value);
	}
	
	/// <summary>Get or set the device scale, i.e. the amount to scale page units to screen pixels.</summary>
	/// <remarks>For example, a value of 1.0 is equivalent to 100% zoom. A value of 2.0 is 200% zoom.</remarks>
	public double DeviceScale
	{
		get => CAPI_View.UlViewGetDeviceScale(View);
		set => CAPI_View.UlViewSetDeviceScale(View, value);
	}

	/// <summary>
	/// Whether the View is GPU-accelerated. If this is false, the page will be rendered via the CPU renderer.
	/// </summary>
	public bool IsAccelerated => CAPI_View.UlViewIsAccelerated(View);
	
	/// <summary>Whether the View supports transparent backgrounds.</summary>
	public bool IsTransparent => CAPI_View.UlViewIsTransparent(View);
	
	/// <summary>Check if the main frame of the page is currently loading.</summary>
	public bool IsLoading => CAPI_View.UlViewIsLoading(View);
	
	/// <summary>Get the RenderTarget for the View.</summary>
	/// <remarks>
	/// <para>Only valid if this View is GPU accelerated.</para>
	/// <para>You can use this with your GPUDriver implementation to bind and display the</para>
	/// <para>corresponding texture in your application.</para>
	/// </remarks>
	public ULRenderTarget RenderTarget => CAPI_View.UlViewGetRenderTarget(View);

	/// <summary>Get the Surface for the View (native pixel buffer that the CPU renderer draws into).</summary>
	/// <remarks>
	/// <para>This operation is only valid if you're managing the Renderer yourself (eg, you've</para>
	/// <para>previously called ulCreateRenderer() instead of ulCreateApp()).</para>
	/// <para>This function will return NULL if this View is GPU accelerated.</para>
	/// <para>The default Surface is BitmapSurface, but you can provide your own Surface implementation</para>
	/// <para>via ulPlatformSetSurfaceDefinition.</para>
	/// <para>When using the default Surface, you can retrieve the underlying bitmap by casting</para>
	/// <para>ULSurface to ULBitmapSurface and calling ulBitmapSurfaceGetBitmap().</para>
	/// </remarks>
	public UlSurface Surface => new(CAPI_View.UlViewGetSurface(View));
	
	/// <summary>Whether the View has focus.</summary>
	public bool HasFocus => CAPI_View.UlViewHasFocus(View);
	
	/// <summary>
	/// Whether the View has an input element with visible keyboard focus (indicated by a
	/// blinking caret).
	/// </summary>
	/// <remarks>
	/// You can use this to decide whether the View should consume keyboard input events (useful
	/// in games with mixed UI and key handling).
	/// </remarks>
	public bool HasInputFocus => CAPI_View.UlViewHasInputFocus(View);
	
	/// <summary>Check if can navigate backwards in history.</summary>
	public bool CanGoBack => CAPI_View.UlViewCanGoBack(View);
	
	/// <summary>Check if can navigate forwards in history.</summary>
	public bool CanGoForward => CAPI_View.UlViewCanGoForward(View);

	/// <summary>Resize view to a certain width and height (in pixels).</summary>
	public void Resize(uint width, uint height) => CAPI_View.UlViewResize(View, width, height);

	/// <summary>Evaluate a string of JavaScript and return result.</summary>
	/// <param name="jsString">The string of JavaScript to evaluate.</param>
	/// <param name="exception">
	/// The address of a ULString to store a description of the last exception. Pass
	/// NULL to ignore this. Don't destroy the exception string returned, it's owned
	/// by the View.
	/// </param>
	/// <remarks>
	/// <para>Don't destroy the returned string, it's owned by the View. This value is reset with every
	/// call-- if you want to retain it you should copy the result to a new string via
	/// ulCreateStringFromCopy().</para>
	/// <para>An example of using this API:
	/// <code>
	/// ULString script = ulCreateString(&quot;1 + 1&quot;);
	/// ULString exception;
	/// ULString result = ulViewEvaluateScript(view, script, &amp;exception);
	/// /* Use the result (&quot;2&quot;) and exception description (if any) here. */
	/// </code>
	/// </para>
	/// </remarks>
	public unsafe string EvaluateScript(string jsString, out string exception)
	{
		var ulStr1 = CAPI_String.UlCreateString(jsString);
		var ulStr2 = CAPI_String.UlCreateString("");
		var ulStr3 = CAPI_View.UlViewEvaluateScript(View, ulStr1, ulStr2);
		sbyte* data1 = CAPI_String.UlStringGetData(ulStr2);
		sbyte* data2 = CAPI_String.UlStringGetData(ulStr3);
		string managedStr1 = Marshal.PtrToStringAnsi((IntPtr)data1)!;
		string managedStr2 = Marshal.PtrToStringAnsi((IntPtr)data2)!;
		
		CAPI_String.UlDestroyString(ulStr1);
		CAPI_String.UlDestroyString(ulStr2);
		CAPI_String.UlDestroyString(ulStr3);
		
		exception = managedStr1;
		return managedStr2;
	}
	
	/// <summary>Give focus to the View.</summary>
	/// <remarks>
	/// You should call this to give visual indication that the View has input focus (changes active
	/// text selection colours, for example).
	/// </remarks>
	public void Focus() => CAPI_View.UlViewFocus(View);
	
	/// <summary>Remove focus from the View and unfocus any focused input elements.</summary>
	/// <remarks>You should call this to give visual indication that the View has lost input focus.</remarks>
	public void Unfocus() => CAPI_View.UlViewUnfocus(View);
	
	/// <summary>Reload current page.</summary>
	public void Reload() => CAPI_View.UlViewReload(View);
	
	/// <summary>Stop all page loads.</summary>
	public void Stop() => CAPI_View.UlViewStop(View);
	
	/// <summary>Navigate to arbitrary offset in history.</summary>
	public void GoToHistoryOffset(int offset) => CAPI_View.UlViewGoToHistoryOffset(View, offset);
	
	/// <summary>Navigate backwards in history.</summary>
	public void GoBack() => CAPI_View.UlViewGoBack(View);
	
	/// <summary>Navigate forwards in history.</summary>
	public void GoForward() => CAPI_View.UlViewGoForward(View);

	/// <summary>Fire a keyboard event.</summary>
	public void FireKeyEvent(
		ULKeyEventType type,
		uint modifiers,
		int virtualKeyCode,
		int nativeKeyCode,
		string text,
		string unmodifiedText,
		bool isKeypad,
		bool isAutoRepeat,
		bool isSystemKey)
	{
		var ulStr1 = CAPI_String.UlCreateString(text);
		var ulStr2 = CAPI_String.UlCreateString(unmodifiedText);
		var e = CAPI_KeyEvent.UlCreateKeyEvent(type, modifiers, virtualKeyCode, nativeKeyCode, ulStr1, ulStr2, isKeypad, isAutoRepeat, isSystemKey);
		CAPI_View.UlViewFireKeyEvent(View, e);
		CAPI_KeyEvent.UlDestroyKeyEvent(e);
		CAPI_String.UlDestroyString(ulStr1);
		CAPI_String.UlDestroyString(ulStr2);
	}

	/// <summary>Fire a mouse event.</summary>
	public void FireMouseEvent(ULMouseEventType type, int x, int y, ULMouseButton button)
	{
		var e = CAPI_MouseEvent.UlCreateMouseEvent(type, x, y, button);
		CAPI_View.UlViewFireMouseEvent(View, e);
		CAPI_MouseEvent.UlDestroyMouseEvent(e);
	}
	
	/// <summary>Fire a scroll event.</summary>
	public void FireScrollEvent(ULScrollEventType type, int deltaX, int deltaY)
	{
		var e = CAPI_ScrollEvent.UlCreateScrollEvent(type, deltaX, deltaY);
		CAPI_View.UlViewFireScrollEvent(View, e);
		CAPI_ScrollEvent.UlDestroyScrollEvent(e);
	}

	public void Dispose()
	{
		if (View.__Instance != IntPtr.Zero) 
			CAPI_View.UlDestroyView(View);
	}
}