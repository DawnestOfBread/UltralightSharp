using Ultralight;

namespace UltralightSharp;

public sealed class UlSession : IDisposable
{
	internal readonly C_Session Session;
	private readonly C_String _name;

	/// <summary>
	/// Create view configuration with default values
	/// </summary>
	private UlSession(UlRenderer renderer, bool isPersistent, string name)
	{
		_name = CAPI_String.UlCreateString(name);
		Session = CAPI_Session.UlCreateSession(renderer.Renderer, isPersistent, _name);
	}

	public void Dispose()
	{
		if (Session.__Instance != IntPtr.Zero) 
			CAPI_Session.UlDestroySession(Session);
		if (_name != null && _name.__Instance != IntPtr.Zero)
			CAPI_String.UlDestroyString(_name);
	}
}