/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_DeleteExpiredRefreshTokens
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Deletes expired Refresh Tokens.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_DeleteExpiredRefreshTokens
AS
BEGIN
    SET NOCOUNT ON;

    DELETE
    FROM Security.RefreshTokens
    WHERE ExpiryDate < GETDATE();
END;
