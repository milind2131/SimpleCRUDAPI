/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_UpdatePassword
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Updates user password.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_UpdatePassword
(
      @UserId INT
    , @PasswordHash NVARCHAR(500)
)
AS
BEGIN

    SET NOCOUNT ON;

    UPDATE Security.Users
    SET
          PasswordHash=@PasswordHash
        , ModifiedDate=GETDATE()
    WHERE UserId=@UserId;

END
