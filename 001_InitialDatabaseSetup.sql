/**************************************************************************************************
Project        : ECommerce Web API
Module         : Security
Script Name    : 001_CreateSecuritySchema.sql
Author         : Milind Sonawane
Created Date   : 21-Jul-2026

Description
---------------------------------------------------------------------------------------------------
This script performs the following activities:

1. Creates ECommerceDB database (if not exists)
2. Creates Security schema (if not exists)
3. Creates Security tables
       - Roles
       - Users
       - UserRoles
       - RefreshTokens
4. Inserts default master data
5. Verifies inserted data

Execution
---------------------------------------------------------------------------------------------------
Execute this script only once during initial database setup.

**************************************************************************************************/

/**************************************************************************************************
STEP 1 - Create Database
**************************************************************************************************/
IF DB_ID('ECommerceDB') IS NULL
BEGIN
    PRINT 'Creating database ECommerceDB...';

    CREATE DATABASE ECommerceDB;

    PRINT 'Database created successfully.';
END
ELSE
BEGIN
    PRINT 'Database already exists.';
END
GO

USE ECommerceDB;
GO

/**************************************************************************************************
STEP 2 - Create Security Schema
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM sys.schemas
    WHERE name = 'Security'
)
BEGIN
    EXEC ('CREATE SCHEMA Security');

    PRINT 'Security schema created.';
END
ELSE
BEGIN
    PRINT 'Security schema already exists.';
END
GO

/**************************************************************************************************
STEP 3 - Create Roles Table
**************************************************************************************************/
IF OBJECT_ID('Security.Roles', 'U') IS NULL
BEGIN

    CREATE TABLE Security.Roles
    (
        RoleId INT IDENTITY(1,1) PRIMARY KEY,

        RoleName NVARCHAR(50) NOT NULL UNIQUE,

        Description NVARCHAR(250) NULL,

        IsActive BIT NOT NULL
            DEFAULT (1),

        CreatedDate DATETIME2 NOT NULL
            DEFAULT (GETDATE())
    );

    PRINT 'Roles table created.';
END
ELSE
BEGIN
    PRINT 'Roles table already exists.';
END
GO

/**************************************************************************************************
STEP 4 - Insert Default Roles
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM Security.Roles
)
BEGIN

    INSERT INTO Security.Roles
    (
        RoleName,
        Description
    )
    VALUES
    ('Admin','System Administrator'),
    ('Seller','Product Seller'),
    ('Customer','Application Customer');

    PRINT 'Default roles inserted.';
END
ELSE
BEGIN
    PRINT 'Roles already available.';
END
GO

/**************************************************************************************************
STEP 5 - Create Users Table
**************************************************************************************************/
IF OBJECT_ID('Security.Users', 'U') IS NULL
BEGIN

    CREATE TABLE Security.Users
    (
        UserId INT IDENTITY(1,1) PRIMARY KEY,

        FirstName NVARCHAR(100) NOT NULL,

        LastName NVARCHAR(100) NULL,

        Email NVARCHAR(200) NOT NULL UNIQUE,

        MobileNumber NVARCHAR(15) NULL,

        PasswordHash NVARCHAR(500) NOT NULL,

        IsEmailVerified BIT NOT NULL
            DEFAULT(0),

        IsActive BIT NOT NULL
            DEFAULT(1),

        LastLogin DATETIME2 NULL,

        CreatedDate DATETIME2 NOT NULL
            DEFAULT(GETDATE()),

        ModifiedDate DATETIME2 NULL
    );

    PRINT 'Users table created.';
END
ELSE
BEGIN
    PRINT 'Users table already exists.';
END
GO

/**************************************************************************************************
STEP 6 - Create UserRoles Table
**************************************************************************************************/
IF OBJECT_ID('Security.UserRoles', 'U') IS NULL
BEGIN

    CREATE TABLE Security.UserRoles
    (
        UserRoleId INT IDENTITY(1,1) PRIMARY KEY,

        UserId INT NOT NULL,

        RoleId INT NOT NULL,

        AssignedDate DATETIME2 NOT NULL
            DEFAULT(GETDATE()),

        CONSTRAINT FK_UserRoles_User
            FOREIGN KEY(UserId)
            REFERENCES Security.Users(UserId),

        CONSTRAINT FK_UserRoles_Role
            FOREIGN KEY(RoleId)
            REFERENCES Security.Roles(RoleId),

        CONSTRAINT UQ_UserRole
            UNIQUE(UserId, RoleId)
    );

    PRINT 'UserRoles table created.';
END
ELSE
BEGIN
    PRINT 'UserRoles table already exists.';
END
GO

/**************************************************************************************************
STEP 7 - Create RefreshTokens Table
**************************************************************************************************/
IF OBJECT_ID('Security.RefreshTokens', 'U') IS NULL
BEGIN

    CREATE TABLE Security.RefreshTokens
    (
        RefreshTokenId INT IDENTITY(1,1) PRIMARY KEY,

        UserId INT NOT NULL,

        Token NVARCHAR(500) NOT NULL,

        ExpiryDate DATETIME2 NOT NULL,

        CreatedDate DATETIME2 NOT NULL
            DEFAULT(GETDATE()),

        IsRevoked BIT NOT NULL
            DEFAULT(0),

        CONSTRAINT FK_RefreshTokens_User
            FOREIGN KEY(UserId)
            REFERENCES Security.Users(UserId)
    );

    PRINT 'RefreshTokens table created.';
END
ELSE
BEGIN
    PRINT 'RefreshTokens table already exists.';
END
GO

/**************************************************************************************************
STEP 8 - Insert Default Users
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM Security.Users
)
BEGIN

    INSERT INTO Security.Users
    (
        FirstName,
        LastName,
        Email,
        MobileNumber,
        PasswordHash
    )
    VALUES
    ('Milind','Sonawane','admin@ecommerce.com','9876543210','TempHash123'),
    ('Rahul','Patil','seller@ecommerce.com','9876543211','TempHash123'),
    ('Amit','Sharma','customer@ecommerce.com','9876543212','TempHash123');

    PRINT 'Default users inserted.';
END
ELSE
BEGIN
    PRINT 'Users already available.';
END
GO

/**************************************************************************************************
STEP 9 - Assign Roles To Users
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM Security.UserRoles
)
BEGIN

    INSERT INTO Security.UserRoles
    (
        UserId,
        RoleId
    )
    VALUES
    (1,1),
    (2,2),
    (3,3);

    PRINT 'User roles assigned.';
END
ELSE
BEGIN
    PRINT 'User roles already assigned.';
END
GO

/**************************************************************************************************
STEP 10 - Verify Data
**************************************************************************************************/

PRINT 'Roles';
SELECT * FROM Security.Roles;

PRINT 'Users';
SELECT * FROM Security.Users;

PRINT 'UserRoles';
SELECT * FROM Security.UserRoles;

PRINT 'RefreshTokens';
SELECT * FROM Security.RefreshTokens;
GO