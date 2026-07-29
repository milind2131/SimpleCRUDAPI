CREATE   PROCEDURE [Catalog].[usp_GetAllProducts]
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
    WHERE p.IsActive = 1
    ORDER BY ProductName;

END
