/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_VerifyPasswordResetRequest
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Marks password reset request as verified.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_VerifyPasswordResetRequest
(
    @PasswordResetRequestId INT
)
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE Security.PasswordResetRequests
    SET
        IsVerified=1
    WHERE PasswordResetRequestId=@PasswordResetRequestId;

END
