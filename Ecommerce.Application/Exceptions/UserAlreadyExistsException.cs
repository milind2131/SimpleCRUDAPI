namespace SimpleCRUDAPI.Ecommerce.Application.Exceptions;

public class UserAlreadyExistsException : BusinessException
{
    public UserAlreadyExistsException()
        : base("User already exists.")
    {
    }
}