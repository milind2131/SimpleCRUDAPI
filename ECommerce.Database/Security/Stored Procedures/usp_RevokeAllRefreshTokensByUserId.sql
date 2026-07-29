/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_RevokeAllRefreshTokensByUserId
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Revokes all active Refresh Tokens of a User.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_RevokeAllRefreshTokensByUserId
(
      @UserId INT
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Security.RefreshTokens
    SET
        IsRevoked = 1,
        RevokedDate = GETDATE()
    WHERE UserId = @UserId
      AND IsRevoked = 0;
END;
