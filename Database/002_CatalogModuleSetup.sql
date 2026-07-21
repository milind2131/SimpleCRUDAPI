/**************************************************************************************************
Project        : ECommerce Web API
Module         : Catalog
Script Name    : 002_CatalogModuleSetup.sql
Author         : Milind Sonawane
Created Date   : 21-Jul-2026

Description
---------------------------------------------------------------------------------------------------
This script performs the following activities:

1. Creates Catalog schema (if not exists)
2. Creates Categories table
3. Creates Products table
4. Inserts default category master data
5. Inserts default product data
6. Verifies inserted data

Execution
---------------------------------------------------------------------------------------------------
Execute this script after:
001_InitialDatabaseSetup.sql

**************************************************************************************************/

USE ECommerceDB;
GO

/**************************************************************************************************
STEP 1 - Create Catalog Schema
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM sys.schemas
    WHERE name = 'Catalog'
)
BEGIN

    EXEC ('CREATE SCHEMA Catalog');

    PRINT 'Catalog schema created successfully.';

END
ELSE
BEGIN

    PRINT 'Catalog schema already exists.';

END
GO

/**************************************************************************************************
STEP 2 - Create Categories Table
**************************************************************************************************/
IF OBJECT_ID('Catalog.Categories', 'U') IS NULL
BEGIN

    CREATE TABLE Catalog.Categories
    (
        CategoryId INT IDENTITY(1,1) PRIMARY KEY,

        CategoryName NVARCHAR(100) NOT NULL UNIQUE,

        Description NVARCHAR(250) NULL,

        IsActive BIT NOT NULL
            DEFAULT(1),

        CreatedDate DATETIME2 NOT NULL
            DEFAULT(GETDATE())
    );

    PRINT 'Categories table created successfully.';

END
ELSE
BEGIN

    PRINT 'Categories table already exists.';

END
GO

/**************************************************************************************************
STEP 3 - Insert Default Categories
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM Catalog.Categories
)
BEGIN

    INSERT INTO Catalog.Categories
    (
        CategoryName,
        Description
    )
    VALUES
    ('Electronics', 'Electronic Devices'),
    ('Fashion', 'Clothing & Footwear'),
    ('Books', 'Books and Stationery');

    PRINT 'Default categories inserted successfully.';

END
ELSE
BEGIN

    PRINT 'Categories already available.';

END
GO

/**************************************************************************************************
STEP 4 - Create Products Table
**************************************************************************************************/
IF OBJECT_ID('Catalog.Products', 'U') IS NULL
BEGIN

    CREATE TABLE Catalog.Products
    (
        ProductId INT IDENTITY(1,1) PRIMARY KEY,

        ProductName NVARCHAR(200) NOT NULL,

        Price DECIMAL(18,2) NOT NULL,

        CategoryId INT NOT NULL,

        IsActive BIT NOT NULL
            DEFAULT(1),

        CreatedDate DATETIME2 NOT NULL
            DEFAULT(GETDATE()),

        CONSTRAINT FK_Products_Categories
            FOREIGN KEY(CategoryId)
            REFERENCES Catalog.Categories(CategoryId)
    );

    PRINT 'Products table created successfully.';

END
ELSE
BEGIN

    PRINT 'Products table already exists.';

END
GO

/**************************************************************************************************
STEP 5 - Insert Default Products
**************************************************************************************************/
IF NOT EXISTS
(
    SELECT 1
    FROM Catalog.Products
)
BEGIN

    INSERT INTO Catalog.Products
    (
        ProductName,
        Price,
        CategoryId
    )
    VALUES
    ('Laptop', 65000.00, 1),
    ('Mobile', 25000.00, 1),
    ('Shoes', 2000.00, 2);

    PRINT 'Default products inserted successfully.';

END
ELSE
BEGIN

    PRINT 'Products already available.';

END
GO

/**************************************************************************************************
STEP 6 - Verify Data
**************************************************************************************************/

PRINT 'Categories';
SELECT *
FROM Catalog.Categories;

PRINT 'Products';
SELECT *
FROM Catalog.Products;
GO