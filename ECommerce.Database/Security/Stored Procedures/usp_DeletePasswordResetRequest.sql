/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_DeletePasswordResetRequest
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Deletes password reset request.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_DeletePasswordResetRequest
(
    @PasswordResetRequestId INT
)
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM Security.PasswordResetRequests
    WHERE PasswordResetRequestId=@PasswordResetRequestId;

END
