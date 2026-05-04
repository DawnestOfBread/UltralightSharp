using SDL3;
using Ultralight;
using UltralightSharp;

namespace Sample;

internal class Program
{
	private static bool IsKeypadKey(SDL.Keycode keycode) => keycode.ToString().StartsWith("Kp");
	private static void Main(string[] args)
	{
		if (!SDL.Init(SDL.InitFlags.Video)) 
			throw new SdlException("Failed to initialise SDL");
		
		IntPtr handle = SDL.CreateWindow("Ultralight SDL Sample", 800, 600, SDL.WindowFlags.Resizable);
		if (handle == IntPtr.Zero)
			throw new SdlException("Failed to create window");

		IntPtr sdlRenderer = SDL.CreateRenderer(handle, null);

		var surfaceDef = new SurfaceDefinition(sdlRenderer);
		UlPlatform.FileSystem = new FileSystem();
		UlPlatform.FontLoader = new FontLoader();
		UlPlatform.Logger = new Logger();
		UlPlatform.Clipboard = new Clipboard();
		UlPlatform.SurfaceDefinition = surfaceDef;
		
		var config = new UlConfig
		{
			CachePath = "./cache",
			FontHinting = ULFontHinting.kFontHintingSmooth,
			FontGamma = 1.8,
		};
		var renderer = UlRenderer.Create(config);

		var viewConfig = new UlViewConfig
		{
			IsAccelerated = false,
			IsTransparent = true,
			InitialDeviceScale = 1,
			EnableImages = true,
			EnableJavaScript = true,
			UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36"
		};
		var view = renderer.CreateView(800, 600, viewConfig);
		view.Url = "https://google.com";

		var isRunning = true;
		while (isRunning)
		{
			while (SDL.PollEvent(out var e))
			{
				var modifiers = ConvertModifiers(e.Key.Mod);
				int virtualKeyCode = KeyHelper.SdlKeyToWindowsVk(e.Key.Key);
				bool isKeypad = IsKeypadKey(e.Key.Key);
				switch (e.Type)
				{
					case (uint)SDL.EventType.KeyDown:
						view.FireKeyEvent(ULKeyEventType.kKeyEventTypeKeyDown, (uint)modifiers, virtualKeyCode, e.Key.Raw, "", "", isKeypad, e.Key.Repeat, false);
						break;
					case (uint)SDL.EventType.KeyUp:
						view.FireKeyEvent(ULKeyEventType.kKeyEventTypeKeyUp, (uint)modifiers, virtualKeyCode, e.Key.Raw, "", "", isKeypad, e.Key.Repeat, false);
						break;
					case (uint)SDL.EventType.MouseButtonDown:
						view.FireMouseEvent(ULMouseEventType.kMouseEventTypeMouseDown, (int)e.Button.X, (int)e.Button.Y, e.Button.Button switch
						{
							1 => ULMouseButton.kMouseButtonLeft,
							2 => ULMouseButton.kMouseButtonMiddle,
							3 => ULMouseButton.kMouseButtonRight,
							_ => ULMouseButton.kMouseButtonNone
						});
						break;
					case (uint)SDL.EventType.MouseButtonUp:
						view.FireMouseEvent(ULMouseEventType.kMouseEventTypeMouseUp, (int)e.Button.X, (int)e.Button.Y, e.Button.Button switch
						{
							1 => ULMouseButton.kMouseButtonLeft,
							2 => ULMouseButton.kMouseButtonMiddle,
							3 => ULMouseButton.kMouseButtonRight,
							_ => ULMouseButton.kMouseButtonNone
						});
						break;
					case (uint)SDL.EventType.MouseMotion:
						view.FireMouseEvent(ULMouseEventType.kMouseEventTypeMouseMoved, (int)e.Button.X, (int)e.Button.Y, ULMouseButton.kMouseButtonNone);
						break;
					case (uint)SDL.EventType.MouseWheel:
						view.FireScrollEvent(ULScrollEventType.kScrollEventTypeScrollByPixel, e.Wheel.IntegerX * 15, e.Wheel.IntegerY * 15);
						break;
					case (uint)SDL.EventType.WindowResized:
						if (e.Window.WindowID == SDL.GetWindowID(handle))
						{
							SDL.GetWindowSizeInPixels(handle, out int width, out int height);
							view.Resize((uint)width, (uint)height);
						}
						break;
					case (uint)SDL.EventType.WindowCloseRequested:
						if (e.Window.WindowID == SDL.GetWindowID(handle))
							isRunning = false;
						break;
				}
			}
			
			renderer.Update();
			renderer.RefreshDisplay(0);
			renderer.Render();
			
			var surface = surfaceDef.GetSurface(view.Surface.UserData);
			SDL.RenderTexture(sdlRenderer, surface.TextureHandle, IntPtr.Zero, IntPtr.Zero);
			SDL.RenderPresent(sdlRenderer);
		}
	}
	
	private static KeyEventModifiers ConvertModifiers(SDL.Keymod mod)
	{
		KeyEventModifiers result = 0;
		if ((mod & SDL.Keymod.Shift) != 0) result |= KeyEventModifiers.ShiftKey;
		if ((mod & SDL.Keymod.Ctrl) != 0) result |= KeyEventModifiers.CtrlKey;
		if ((mod & SDL.Keymod.Alt) != 0) result |= KeyEventModifiers.AltKey;
		if ((mod & SDL.Keymod.GUI) != 0) result |= KeyEventModifiers.MetaKey;
		return result;
	}
}