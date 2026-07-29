/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_GetPasswordResetRequest
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Gets latest password reset request.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_GetPasswordResetRequest
(
    @UserId INT
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT TOP(1)
          PasswordResetRequestId
        , UserId
        , OTPHash
        , OTPExpiry
        , IsVerified
        , CreatedDate
    FROM Security.PasswordResetRequests
    WHERE UserId=@UserId
    ORDER BY PasswordResetRequestId DESC;

END
