/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_UpdateRegistrationOtp
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Updates OTP hash and expiry for resend OTP functionality.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_UpdateRegistrationOtp
(
      @Email NVARCHAR(200)
    , @OTPHash NVARCHAR(500)
    , @OTPExpiry DATETIME
)
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE Security.PendingUsers
    SET
          OTPHash = @OTPHash
        , OTPExpiry = @OTPExpiry
    WHERE Email = @Email;

    SELECT @@ROWCOUNT;

END
