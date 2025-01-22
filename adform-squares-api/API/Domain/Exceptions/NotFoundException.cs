using System.Net;

namespace API.Domain.Exceptions;

public class NotFoundException : ExceptionBase
{
    public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;

    public NotFoundException(string message) : base(message, "Not Found")
    {

    }
}
