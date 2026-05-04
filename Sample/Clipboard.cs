using SDL3;
using UltralightSharp.Interfaces;

namespace Sample;

public class Clipboard : IClipboard
{
	public void Clear()
	{
		SDL.ClearClipboardData();
	}

	public string ReadPlainText()
	{
		return SDL.GetClipboardText();
	}

	public void WritePlainText(string plainText)
	{
		SDL.SetClipboardText(plainText);
	}
}