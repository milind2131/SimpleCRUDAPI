CREATE   PROCEDURE Catalog.usp_InsertProduct

    @ProductName NVARCHAR(200),
    @Price DECIMAL(18,2),
    @CategoryId INT

AS
BEGIN

    SET NOCOUNT ON;

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

    SELECT SCOPE_IDENTITY();

END
