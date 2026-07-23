/**************************************************************************************************
Project        : ECommerce Web API
Module         : Catalog
Script Name    : 001_ProductCRUD_StoredProcedures.sql
Author         : Milind Sonawane
Created Date   : 21-Jul-2026

Description
---------------------------------------------------------------------------------------------------
This script creates all stored procedures required for Product CRUD operations.

Procedures Included
---------------------------------------------------------------------------------------------------
1. Catalog.usp_GetAllProducts
2. Catalog.usp_GetProductById
3. Catalog.usp_InsertProduct
4. Catalog.usp_UpdateProduct
5. Catalog.usp_DeleteProduct

**************************************************************************************************/

/**************************************************************************************************
Procedure Name : Catalog.usp_GetAllProducts

Description
---------------------------------------------------------------------------------------------------
Retrieves all active products.

Execution
---------------------------------------------------------------------------------------------------
EXEC Catalog.usp_GetAllProducts;
**************************************************************************************************/

CREATE OR ALTER PROCEDURE Catalog.usp_GetAllProducts
AS
BEGIN

    SET NOCOUNT ON;

    SELECT
       p.ProductId as Id,
        p.ProductName as Name,
        p.Price,
        c.CategoryId,
        p.IsActive,
        c.CreatedDate,
		c.CategoryName as Category
    FROM Catalog.Products p inner join Catalog.Categories c  on p.CategoryId=c.CategoryId
    WHERE IsActive = 1
    ORDER BY ProductName;

END;
GO


/**************************************************************************************************
Procedure Name : Catalog.usp_GetProductById

Description
---------------------------------------------------------------------------------------------------
Retrieves a product by ProductId.

Execution
---------------------------------------------------------------------------------------------------
EXEC Catalog.usp_GetProductById
     @ProductId = 1;
**************************************************************************************************/

CREATE OR ALTER PROCEDURE Catalog.usp_GetProductById

    @ProductId INT

AS
BEGIN

    SET NOCOUNT ON;

    SELECT
        ProductId,
        ProductName,
        Price,
        CategoryId,
        IsActive,
        CreatedDate
    FROM Catalog.Products
    WHERE ProductId = @ProductId;

END;
GO


/**************************************************************************************************
Procedure Name : Catalog.usp_InsertProduct

Description
---------------------------------------------------------------------------------------------------
Inserts a new product into Catalog.Products.

Execution
---------------------------------------------------------------------------------------------------
EXEC Catalog.usp_InsertProduct
     @ProductName = 'Gaming Laptop',
     @Price = 95000,
     @CategoryId = 1;
**************************************************************************************************/

CREATE OR ALTER PROCEDURE Catalog.usp_InsertProduct

    @ProductName NVARCHAR(200),
    @Price DECIMAL(18,2),
    @CategoryId INT

AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        INSERT INTO Catalog.Products
        (
            ProductName,
            Price,
            CategoryId
        )
        VALUES
        (
            @ProductName,
            @Price,
            @CategoryId
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT) AS ProductId;

    END TRY

    BEGIN CATCH

        THROW;

    END CATCH

END;
GO


/**************************************************************************************************
Procedure Name : Catalog.usp_UpdateProduct

Description
---------------------------------------------------------------------------------------------------
Updates an existing product.

Execution
---------------------------------------------------------------------------------------------------
EXEC Catalog.usp_UpdateProduct
     @ProductId = 1,
     @ProductName = 'Gaming Laptop',
     @Price = 99000;
**************************************************************************************************/

CREATE OR ALTER PROCEDURE Catalog.usp_UpdateProduct

    @ProductId INT,
    @ProductName NVARCHAR(200),
    @Price DECIMAL(18,2)

AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE Catalog.Products
        SET
            ProductName = @ProductName,
            Price = @Price
        WHERE ProductId = @ProductId;

    END TRY

    BEGIN CATCH

        THROW;

    END CATCH

END;
GO


/**************************************************************************************************
Procedure Name : Catalog.usp_DeleteProduct

Description
---------------------------------------------------------------------------------------------------
Deletes a product by ProductId.

Execution
---------------------------------------------------------------------------------------------------
EXEC Catalog.usp_DeleteProduct
     @ProductId = 1;
**************************************************************************************************/

CREATE OR ALTER PROCEDURE Catalog.usp_DeleteProduct

    @ProductId INT

AS
BEGIN

    SET NOCOUNT ON;

    BEGIN TRY

        UPDATE Catalog.Products
SET
    IsActive = 0
WHERE ProductId = @ProductId;
    END TRY

    BEGIN CATCH

        THROW;

    END CATCH

END;
GO