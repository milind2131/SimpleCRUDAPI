/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Procedure Name : Security.usp_GetPendingUserByEmail
Author         : Milind Sonawane

Description
-----------------------------------------------------------------------------------------
Retrieves pending user details using Email Address.
**************************************************************************************************/

CREATE   PROCEDURE Security.usp_GetPendingUserByEmail
(
      @Email NVARCHAR(200)
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
          PendingUserId
        , FirstName
        , LastName
        , Email
        , MobileNumber
        , PasswordHash
        , OTPHash
        , OTPExpiry
        , CreatedDate
    FROM Security.PendingUsers
    WHERE Email = @Email;

END
