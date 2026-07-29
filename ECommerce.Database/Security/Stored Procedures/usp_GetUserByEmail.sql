CREATE   PROCEDURE Security.usp_GetUserByEmail
(
    @Email NVARCHAR(200)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
          UserId
        , FirstName
        , LastName
        , Email
        , MobileNumber
        , PasswordHash
        , IsEmailVerified
        , IsActive
        , LastLogin
        , CreatedDate
        , ModifiedDate
    FROM Security.Users
    WHERE Email = @Email
      AND IsActive = 1;
END
