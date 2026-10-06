namespace SimpleCRUDAPI.Ecommerce.Application.Exceptions;

public class UserRoleNotAssignedException : BusinessException
{
    public UserRoleNotAssignedException()
        : base("User role is not assigned. Please contact administrator.")
    {
    }
}