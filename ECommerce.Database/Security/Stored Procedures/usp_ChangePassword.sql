CREATE   PROCEDURE Security.usp_ChangePassword
(
    @UserId INT,
    @PasswordHash NVARCHAR(MAX)
)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Security.Users
    SET
        PasswordHash = @PasswordHash,
        ModifiedDate = GETDATE()
    WHERE UserId = @UserId
      AND IsActive = 1;

    SELECT CAST(
        CASE
            WHEN @@ROWCOUNT > 0 THEN 1
            ELSE 0
        END AS BIT
    );
END
