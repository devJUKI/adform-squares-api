using System.Net;

namespace API.Domain.Exceptions;

public class ConflictException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.Conflict;

    public ConflictException(string message) : base(message, "Conflict")
    {

    }
}

