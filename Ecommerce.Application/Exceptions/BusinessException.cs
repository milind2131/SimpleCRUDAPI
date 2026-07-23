namespace SimpleCRUDAPI.Ecommerce.Application.Exceptions;

public abstract class BusinessException : Exception
{
    protected BusinessException(string message)
        : base(message)
    {
    }
}