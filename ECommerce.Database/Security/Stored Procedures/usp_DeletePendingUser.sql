/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_DeletePendingUser
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Deletes pending user after successful email verification.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_DeletePendingUser
(
      @PendingUserId INT
)
AS
BEGIN

    SET NOCOUNT ON;

    DELETE
    FROM Security.PendingUsers
    WHERE PendingUserId = @PendingUserId;

END
