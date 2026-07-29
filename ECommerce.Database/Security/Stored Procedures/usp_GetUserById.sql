CREATE   PROCEDURE Security.usp_GetUserById
(
    @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        UserId,
        FirstName,
        LastName,
        Email,
        MobileNumber,
        PasswordHash
    FROM Security.Users
    WHERE UserId = @UserId
      AND IsActive = 1;
END
