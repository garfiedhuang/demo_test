namespace Hkust.Infra.Core.Exceptions;

[Serializable]
public class BusinessException : Exception, IHkustException
{
    public BusinessException(string message)
        : base(message)
    {
        base.HResult = (int)HttpStatusCode.BadRequest;
    }

    public BusinessException(HttpStatusCode statusCode, string message)
    : base(message)
    {
        base.HResult = (int)statusCode;
    }
}