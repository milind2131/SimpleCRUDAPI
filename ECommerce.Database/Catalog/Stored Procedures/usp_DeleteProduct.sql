CREATE   PROCEDURE Catalog.usp_DeleteProduct

    @ProductId INT

AS
BEGIN

    SET NOCOUNT ON;

   UPDATE Catalog.Products
SET
    IsActive = 0
WHERE ProductId = @ProductId;

END
