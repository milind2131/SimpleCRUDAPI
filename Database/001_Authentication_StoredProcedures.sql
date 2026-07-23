/**************************************************************************************************
Project        : ECommerce Web API
Module         : Authentication
Script Name    : 001_Authentication_StoredProcedures.sql
Author         : Milind Sonawane
Created Date   : 21-Jul-2026

Description
---------------------------------------------------------------------------------------------------
This script creates all stored procedures required for User Authentication.

Procedures Included
---------------------------------------------------------------------------------------------------
1. Security.usp_CheckUserExistsByEmail
2. Security.usp_RegisterUser
3. Security.usp_GetUserByEmail

Execution
---------------------------------------------------------------------------------------------------
Execute this script after:
001_InitialDatabaseSetup.sql

**************************************************************************************************/

/**************************************************************************************************
Procedure Name : Security.usp_CheckUserExistsByEmail

Description
---------------------------------------------------------------------------------------------------
Checks whether an active user already exists using the supplied email address.

Parameters
---------------------------------------------------------------------------------------------------
@Email - Email address of the user.

Execution
---------------------------------------------------------------------------------------------------
EXEC Security.usp_CheckUserExistsByEmail
     @Email = 'admin@ecommerce.com';

**************************************************************************************************/

CREATE OR ALTER PROCEDURE Security.usp_CheckUserExistsByEmail
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

END;
GO


/**************************************************************************************************
Procedure Name : Security.usp_RegisterUser

Description
---------------------------------------------------------------------------------------------------
Registers a new user into the application.

Parameters
---------------------------------------------------------------------------------------------------
@FirstName      - User First Name
@LastName       - User Last Name
@Email          - User Email Address
@MobileNumber   - User Mobile Number
@PasswordHash   - BCrypt Password Hash

Execution
---------------------------------------------------------------------------------------------------
EXEC Security.usp_RegisterUser
     @FirstName = 'Milind',
     @LastName = 'Sonawane',
     @Email = 'milind@gmail.com',
     @MobileNumber = '9876543210',
     @PasswordHash = 'BCryptPasswordHash';

**************************************************************************************************/

CREATE OR ALTER PROCEDURE Security.usp_RegisterUser
(
      @FirstName      NVARCHAR(100)
    , @LastName       NVARCHAR(100)
    , @Email          NVARCHAR(200)
    , @MobileNumber   NVARCHAR(15)
    , @PasswordHash   NVARCHAR(500)
)
AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        INSERT INTO Security.Users
        (
              FirstName
            , LastName
            , Email
            , MobileNumber
            , PasswordHash
            , IsEmailVerified
            , IsActive
            , CreatedDate
        )
        VALUES
        (
              @FirstName
            , @LastName
            , @Email
            , @MobileNumber
            , @PasswordHash
            , 0
            , 1
            , GETDATE()
        );

        SELECT
            CAST(SCOPE_IDENTITY() AS INT) AS UserId;

    END TRY

    BEGIN CATCH

        THROW;

    END CATCH

END;
GO


/**************************************************************************************************
Procedure Name : Security.usp_GetUserByEmail

Description
---------------------------------------------------------------------------------------------------
Retrieves complete user details using the supplied email address.
This procedure is primarily used during user login for password verification
and JWT token generation.

Parameters
---------------------------------------------------------------------------------------------------
@Email - Registered email address.

Execution
---------------------------------------------------------------------------------------------------
EXEC Security.usp_GetUserByEmail
     @Email = 'admin@ecommerce.com';

**************************************************************************************************/

CREATE OR ALTER PROCEDURE Security.usp_GetUserByEmail
(
      @Email NVARCHAR(200)
)
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
          UserId
        , FirstName
        , LastName
        , Email
        , MobileNumber
        , PasswordHash
        , IsEmailVerified
        , IsActive
        , LastLogin
        , CreatedDate
        , ModifiedDate
    FROM Security.Users
    WHERE Email = @Email
      AND IsActive = 1;

END;
GO