namespace Sample;

public class SdlException : Exception
{
    public SdlException()
    {
    }

    public SdlException(string? message) : base(message + ": " + SDL3.SDL.GetError())
    {
    }

    public SdlException(string? message, Exception? innerException) : base(message + ": " + SDL3.SDL.GetError(), innerException)
    {
    }

    public override string ToString() => base.ToString().Replace("Cascade.Implementations.Base.SDL.SdlException", "SdlException", StringComparison.OrdinalIgnoreCase);
}