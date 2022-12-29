using System.Runtime.Serialization;

namespace ThFnsc.RemoteControl.Util;

public class TokenMissingException : Exception
{
    public TokenMissingException() { }

    public TokenMissingException(string? message) : base(message) { }

    public TokenMissingException(string? message, Exception? innerException) : base(message, innerException) { }

    protected TokenMissingException(SerializationInfo info, StreamingContext context) : base(info, context) { }
}
