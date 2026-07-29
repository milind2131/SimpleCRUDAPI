/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_SaveRefreshToken
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Saves a new Refresh Token and revokes existing active Refresh Tokens.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_SaveRefreshToken
(
      @UserId INT,
      @RefreshToken NVARCHAR(500),
      @ExpiryDate DATETIME,
      @CreatedByIp NVARCHAR(50) = NULL
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Security.RefreshTokens
    SET
        IsRevoked = 1,
        RevokedDate = GETDATE(),
        ReplacedByToken = @RefreshToken
    WHERE UserId = @UserId
      AND IsRevoked = 0;

    INSERT INTO Security.RefreshTokens
    (
        UserId,
        RefreshToken,
        ExpiryDate,
        CreatedByIp
    )
    VALUES
    (
        @UserId,
        @RefreshToken,
        @ExpiryDate,
        @CreatedByIp
    );
END;
