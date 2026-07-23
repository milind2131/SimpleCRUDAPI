namespace SimpleCRUDAPI.Ecommerce.Application.Exceptions;

public class InvalidCredentialsException : BusinessException
{
    public InvalidCredentialsException()
        : base("Invalid Email or Password.")
    {
    }
}