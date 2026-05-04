using Ultralight;

namespace UltralightSharp.Input;

public interface IGamepadEvent : IDisposable
{
	static abstract ULGamepadEventType Type { get; }
}