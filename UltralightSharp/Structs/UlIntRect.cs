namespace UltralightSharp.Structs;

public struct UlIntRect : IEquatable<UlIntRect>
{
	public int Left;
	public int Top;
	public int Right;
	public int Bottom;

	public readonly bool IsEmpty => Left == Right || Top == Bottom;
	
	public readonly bool Equals(UlIntRect other) => Left == other.Left && Top == other.Top && Right == other.Right && Bottom == other.Bottom;
}
