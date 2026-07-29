CREATE   PROCEDURE Catalog.usp_UpdateProduct

    @ProductId INT,
    @ProductName NVARCHAR(200),
    @Price DECIMAL(18,2)
    --@CategoryId INT

AS
BEGIN

    SET NOCOUNT ON;

    UPDATE Catalog.Products
    SET

        ProductName = @ProductName,

        Price = @Price

       -- CategoryId = @CategoryId

    WHERE ProductId = @ProductId;

END