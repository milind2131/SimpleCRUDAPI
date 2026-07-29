CREATE   PROCEDURE [Security].[usp_RegisterUser]
(
      @FirstName      NVARCHAR(100)
    , @LastName       NVARCHAR(100)
    , @Email          NVARCHAR(200)
    , @MobileNumber   NVARCHAR(15)
    , @PasswordHash   NVARCHAR(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Security.Users
    (
          FirstName
        , LastName
        , Email
        , MobileNumber
        , PasswordHash
        , IsEmailVerified
        , IsActive
        , CreatedDate
    )
    VALUES
    (
          @FirstName
        , @LastName
        , @Email
        , @MobileNumber
        , @PasswordHash
        , 1
        , 1
        , GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
