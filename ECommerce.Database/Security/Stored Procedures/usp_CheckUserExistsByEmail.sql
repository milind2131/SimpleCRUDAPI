/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_CheckUserExistsByEmail
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Checks whether a user already exists using Email Address.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_CheckUserExistsByEmail
(
      @Email NVARCHAR(200)
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        UserId
    FROM Security.Users
    WHERE Email = @Email
      AND IsActive = 1;

END
