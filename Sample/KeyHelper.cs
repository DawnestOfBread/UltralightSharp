using SDL3;
using UltralightSharp;
using static SDL3.SDL.Keycode;
using static Ultralight.UlKeyCodes;

namespace Sample;

public static class KeyHelper
{
    public static int SdlKeyToWindowsVk(SDL.Keycode scancode)
    {
        return scancode switch
        {
            // General
            Backspace => GK_BACK,
            Tab => GK_TAB,
            Return => GK_RETURN,
            LShift => GK_LSHIFT,
            RShift => GK_RSHIFT,
            LCtrl => GK_LCONTROL,
            RCtrl => GK_RCONTROL,
            LAlt => GK_LMENU,
            RAlt => GK_RMENU,
            Menu => GK_MENU,
            Pause => GK_PAUSE,
            Capslock => GK_CAPITAL,
            Escape => GK_ESCAPE,
            Space => GK_SPACE,
            Pageup => GK_PRIOR,
            Prior => GK_PRIOR,
            Pagedown => GK_NEXT,
            End => GK_END,
            Home => GK_HOME,
            Left => GK_LEFT,
            Up => GK_UP,
            Right => GK_RIGHT,
            Down => GK_DOWN,
            Insert => GK_INSERT,
            Delete => GK_DELETE,
            NumLockClear => GK_NUMLOCK,
            ScrollLock => GK_SCROLL,
            PrintScreen => GK_SNAPSHOT,
            
            // F-row
            F1 => GK_F1,
            F2 => GK_F2,
            F3 => GK_F3,
            F4 => GK_F4,
            F5 => GK_F5,
            F6 => GK_F6,
            F7 => GK_F7,
            F8 => GK_F8,
            F9 => GK_F9,
            F10 => GK_F10,
            F11 => GK_F11,
            F12 => GK_F12,
            F13 => GK_F13,
            F14 => GK_F14,
            F15 => GK_F15,
            F16 => GK_F16,
            F17 => GK_F17,
            F18 => GK_F18,
            F19 => GK_F19,
            F20 => GK_F20,
            F21 => GK_F21,
            F22 => GK_F22,
            F23 => GK_F23,
            F24 => GK_F24,
            
            // Keypad/numpad
            Kp0 => GK_NUMPAD0,
            Kp1 => GK_NUMPAD1,
            Kp2 => GK_NUMPAD2,
            Kp3 => GK_NUMPAD3,
            Kp4 => GK_NUMPAD4,
            Kp5 => GK_NUMPAD5,
            Kp6 => GK_NUMPAD6,
            Kp7 => GK_NUMPAD7,
            Kp8 => GK_NUMPAD8,
            Kp9 => GK_NUMPAD9,
            KpMultiply => GK_MULTIPLY,
            KpPlus => GK_ADD,
            KpMinus => GK_SUBTRACT,
            KpDivide => GK_DIVIDE,
            KpPeriod => GK_DECIMAL,
            KpEnter => GK_RETURN,
            
            // Number row
            Alpha0 => GK_0,
            Alpha1 => GK_1,
            Alpha2 => GK_2,
            Alpha3 => GK_3,
            Alpha4 => GK_4,
            Alpha5 => GK_5,
            Alpha6 => GK_6,
            Alpha7 => GK_7,
            Alpha8 => GK_8,
            Alpha9 => GK_9,
            
            // Symbols
            Semicolon => GK_OEM_1,
            SDL.Keycode.Equals => GK_OEM_PLUS,
            Comma => GK_OEM_COMMA,
            Minus => GK_OEM_MINUS,
            Period => GK_OEM_PERIOD,
            Slash => GK_OEM_2,
            Grave => GK_OEM_3,
            LeftBracket => GK_OEM_4,
            Backslash => GK_OEM_5,
            RightBracket => GK_OEM_6,
            Apostrophe => GK_OEM_7,
            
            // Playback
            Mute => GK_VOLUME_MUTE,
            VolumeDown => GK_VOLUME_DOWN,
            VolumeUp => GK_VOLUME_UP,
            MediaNextTrack => GK_MEDIA_NEXT_TRACK,
            MediaPreviousTrack => GK_MEDIA_PREV_TRACK,
            MediaStop => GK_MEDIA_STOP,
            MediaPlayPause => GK_MEDIA_PLAY_PAUSE,
            
            // Browser?
            AcBack => GK_BROWSER_BACK,
            AcForward => GK_BROWSER_FORWARD,
            AcBookmarks => GK_BROWSER_FAVORITES,
            AcHome => GK_BROWSER_HOME,
            AcRefresh => GK_BROWSER_REFRESH,
            AcSearch => GK_BROWSER_SEARCH,
            
            // ???
            Clear => GK_CLEAR,
            
            // Letters
            A => GK_A,
            B => GK_B,
            C => GK_C,
            D => GK_D,
            E => GK_E,
            F => GK_F,
            G => GK_G,
            H => GK_H,
            I => GK_I,
            J => GK_J,
            K => GK_K,
            L => GK_L,
            M => GK_M,
            N => GK_N,
            O => GK_O,
            P => GK_P,
            Q => GK_Q,
            R => GK_R,
            S => GK_S,
            T => GK_T,
            U => GK_U,
            V => GK_V,
            W => GK_W,
            X => GK_X,
            Y => GK_Y,
            Z => GK_Z,
            
            _ => 0
        };
    }
}