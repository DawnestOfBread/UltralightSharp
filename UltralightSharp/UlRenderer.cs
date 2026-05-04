using System.Runtime.InteropServices;
using Ultralight;
using UltralightSharp.Input;

namespace UltralightSharp;

public sealed class UlRenderer : IDisposable
{
	internal readonly C_Renderer Renderer;
	
	private UlRenderer(C_Config config) => Renderer = CAPI_Renderer.UlCreateRenderer(config);

	/// <summary>Create the core renderer singleton for the library.</summary>
	/// <param name="config">The configuration to use for the renderer.</param>
	/// <returns>Returns the new renderer instance.</returns>
	/// <remarks>
	/// <para>You should set up the Platform singleton (see CAPI_Platform.h) before calling this function.</para>
	/// <para>You do not need to the call this if you're using ulCreateApp() from AppCore.</para>
	/// </remarks>
	public static UlRenderer Create(UlConfig config) =>
		new(config.Config);
	
	/// <summary>Create a View with certain size (in pixels).</summary>
	/// <remarks>You can pass null to 'session' to use the default session.</remarks>
	public UlView CreateView(uint width, uint height, UlViewConfig config, UlSession? session = null) =>
		new(this, width, height, config, session);
	
	/// <summary>Update timers and dispatch internal callbacks (JavaScript and network).</summary>
	public void Update() => CAPI_Renderer.UlUpdate(Renderer);
	
	/// <summary>Notify the renderer that a display has refreshed (you should call this after vsync).</summary>
	/// <param name="displayId">The display ID to refresh (0 by default).</param>
	/// <remarks>
	/// <para>This updates animations, smooth scroll, and window.requestAnimationFrame() for all Views</para>
	/// <para>matching the display id.</para>
	/// </remarks>
	public void RefreshDisplay(uint displayId) => CAPI_Renderer.UlRefreshDisplay(Renderer, displayId);
	
	/// <summary>Render all active Views to their respective surfaces and render targets.</summary>
	public void Render() => CAPI_Renderer.UlRender(Renderer);
	
	/// <summary>
	/// <para>Attempt to release as much memory as possible. Don't call this from any callbacks or driver</para>
	/// <para>code.</para>
	/// </summary>
	public void PurgeMemory() => CAPI_Renderer.UlPurgeMemory(Renderer);
	
	/// <summary>Print detailed memory usage statistics to the log. (</summary>
	/// <remarks>ulPlatformSetLogger)</remarks>
	public void LogMemoryUsage() => CAPI_Renderer.UlLogMemoryUsage(Renderer);
	
	/// <summary>Start the remote inspector server.</summary>
	/// <param name="renderer">The active renderer instance.</param>
	/// <param name="address">The address for the server to listen on (eg, &quot;127.0.0.1&quot;)</param>
	/// <param name="port">The port for the server to listen on (eg, 9222)</param>
	/// <returns>Returns whether the server started successfully or not.</returns>
	/// <remarks>
	/// While the remote inspector is active, Views that are loaded into this renderer will be able to be remotely inspected from another Ultralight instance either locally (another app on same machine) or remotely (over the network) by navigating a View to:
	/// </remarks>
	public void StartRemoteInspectorServer(string address, ushort port) => CAPI_Renderer.UlStartRemoteInspectorServer(Renderer, address, port);
	
	/// <summary>
	/// Describe the details of a gamepad, to be used with ulFireGamepadEvent and related events below.
	/// This can be called multiple times with the same index if the details change.
	/// </summary>
	/// <param name="index">
	/// The unique index (or &quot;connection slot&quot;) of the gamepad.
	/// For example, controller #1 would be &quot;1&quot;, controller #2 would be &quot;2&quot; and so on.
	/// </param>
	/// <param name="id">
	/// A string ID representing the device, this will be made available in JavaScript as gamepad.id
	/// </param>
	/// <param name="axisCount">The number of axes on the device.</param>
	/// <param name="buttonCount">The number of buttons on the device.</param>
	public void SetGamepadDetails(uint index, string id, uint axisCount, uint buttonCount)
	{
		if (_gamepadIds.TryGetValue(index, out var value))
			CAPI_String.UlDestroyString(value);
		_gamepadIds[index] = CAPI_String.UlCreateString(id); 
		CAPI_Renderer.UlSetGamepadDetails(Renderer, index, _gamepadIds[index], axisCount, buttonCount);
	}
	private readonly Dictionary<uint, C_String> _gamepadIds = new();

	/// <summary>Fire a gamepad event (connection / disconnection).</summary>
	/// <param name="index">The index of the gamepad.</param>
	/// <param name="type">The event type to fire.</param>
	/// <remarks>
	/// The gamepad should first be described via ulSetGamepadDetails before calling this function.
	/// <para>https://developer.mozilla.org/en-US/docs/Web/API/Gamepad</para>
	/// </remarks>
	public void FireGamepadEvent(uint index, ULGamepadEventType type)
	{
		var e= CAPI_GamepadEvent.UlCreateGamepadEvent(index, type);
		CAPI_Renderer.UlFireGamepadEvent(Renderer, e);
		CAPI_GamepadEvent.UlDestroyGamepadEvent(e);
	}
	
	/// <summary>Fire a gamepad axis event (to be called when an axis value is changed).</summary>
	/// <param name="index">The index of the gamepad.</param>
	/// <param name="axisIndex">The event to fire.</param>
	/// <param name="value">The event to fire.</param>
	/// <remarks>
	/// The gamepad should be connected via a previous call to ulFireGamepadEvent.
	/// <para>https://developer.mozilla.org/en-US/docs/Web/API/Gamepad/axes</para>
	/// </remarks>
	public void FireGamepadAxisEvent(uint index, uint axisIndex, double value)
	{
		var e= CAPI_GamepadEvent.UlCreateGamepadAxisEvent(index, axisIndex, value);
		CAPI_Renderer.UlFireGamepadAxisEvent(Renderer, e);
		CAPI_GamepadEvent.UlDestroyGamepadAxisEvent(e);
	}
	
	/// <summary>Fire a gamepad button event (to be called when a button value is changed).</summary>
	/// <param name="index">The index of the gamepad.</param>
	/// <param name="buttonIndex">The event to fire.</param>
	/// <param name="value">The event to fire.</param>
	/// <remarks>
	/// <para>The gamepad should be connected via a previous call to ulFireGamepadEvent.</para>
	/// <para>https://developer.mozilla.org/en-US/docs/Web/API/Gamepad/buttons</para>
	/// </remarks>
	public void FireGamepadButtonEvent(uint index, uint buttonIndex, double value)
	{
		var e= CAPI_GamepadEvent.UlCreateGamepadButtonEvent(index, buttonIndex, value);
		CAPI_Renderer.UlFireGamepadButtonEvent(Renderer, e);
		CAPI_GamepadEvent.UlDestroyGamepadButtonEvent(e);
	}

	public void Dispose()
	{
		if (Renderer.__Instance != IntPtr.Zero) 
			CAPI_Renderer.UlDestroyRenderer(Renderer);
	}
}