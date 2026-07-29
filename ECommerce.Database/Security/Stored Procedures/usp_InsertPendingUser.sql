/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_InsertPendingUser
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Inserts a pending user registration with OTP before email verification.
**************************************************************************************************/

CREATE   PROCEDURE [Security].[usp_InsertPendingUser]
(
      @FirstName      NVARCHAR(100)
    , @LastName       NVARCHAR(100)
    , @Email          NVARCHAR(200)
    , @MobileNumber   NVARCHAR(15)
    , @PasswordHash   NVARCHAR(500)
    , @OTPHash NVARCHAR(500)
    , @OTPExpiry      DATETIME
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Security.PendingUsers
    (
          FirstName
        , LastName
        , Email
        , MobileNumber
        , PasswordHash
        , OTPHash
        , OTPExpiry
        , CreatedDate
    )
    VALUES
    (
          @FirstName
        , @LastName
        , @Email
        , @MobileNumber
        , @PasswordHash
        , @OTPHash
        , @OTPExpiry
        , GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
