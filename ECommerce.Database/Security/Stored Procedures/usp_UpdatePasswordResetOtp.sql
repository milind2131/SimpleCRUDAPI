/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_UpdatePasswordResetOtp
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Updates OTP for forgot password.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_UpdatePasswordResetOtp
(
      @PasswordResetRequestId INT
    , @OTPHash NVARCHAR(500)
    , @OTPExpiry DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE Security.PasswordResetRequests
    SET
          OTPHash=@OTPHash
        , OTPExpiry=@OTPExpiry
        , IsVerified=0
    WHERE PasswordResetRequestId=@PasswordResetRequestId;

END
