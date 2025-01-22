using System.Net;

namespace API.Domain.Exceptions;

public class ValidationException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

    public ValidationException(string message) : base(message, "Validation Failed")
    {

    }
}
