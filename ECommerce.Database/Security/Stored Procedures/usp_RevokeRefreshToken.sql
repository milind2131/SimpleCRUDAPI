/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_RevokeRefreshToken
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Revokes a Refresh Token.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_RevokeRefreshToken
(
      @RefreshToken NVARCHAR(500),
      @ReplacedByToken NVARCHAR(500) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Security.RefreshTokens
    SET
        IsRevoked = 1,
        RevokedDate = GETDATE(),
        ReplacedByToken = @ReplacedByToken
    WHERE RefreshToken = @RefreshToken
      AND IsRevoked = 0;
END;
