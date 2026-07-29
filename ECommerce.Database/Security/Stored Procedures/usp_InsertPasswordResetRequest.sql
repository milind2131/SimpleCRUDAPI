/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_InsertPasswordResetRequest
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Creates password reset request for forgot password.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_InsertPasswordResetRequest
(
      @UserId INT
    , @OTPHash NVARCHAR(500)
    , @OTPExpiry DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;

    INSERT INTO Security.PasswordResetRequests
    (
          UserId
        , OTPHash
        , OTPExpiry
        , IsVerified
        , CreatedDate
    )
    VALUES
    (
          @UserId
        , @OTPHash
        , @OTPExpiry
        , 0
        , GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);

END
