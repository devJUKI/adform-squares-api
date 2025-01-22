using System.Net;

namespace API.Domain.Exceptions;

public abstract class ExceptionBase : Exception
{
    public abstract HttpStatusCode StatusCode { get; }
    public string MessageHeader { get; }

    protected ExceptionBase(string message, string messageHeader) : base(message)
    {
        MessageHeader = messageHeader;
    }
}
