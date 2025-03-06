namespace Martiello.Domain.UseCase
{
    public enum ErrorCode
    {
        NotFound = 404,
        BadRequest = 400,
        Business = 422,
        Unauthorized = 401,
        Conflict = 409,
        InternalServerError = 500,
        ServiceUnavailable = 503
    }
}
