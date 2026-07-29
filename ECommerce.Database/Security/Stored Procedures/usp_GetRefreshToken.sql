/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_GetRefreshToken
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Gets Refresh Token details.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_GetRefreshToken
(
      @RefreshToken NVARCHAR(500)
)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        RefreshTokenId,
        UserId,
        RefreshToken,
        ExpiryDate,
        IsRevoked,
        CreatedDate,
        CreatedByIp,
        RevokedDate,
        ReplacedByToken
    FROM Security.RefreshTokens
    WHERE RefreshToken = @RefreshToken;
END;
