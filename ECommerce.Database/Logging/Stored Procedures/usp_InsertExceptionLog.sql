CREATE   PROCEDURE Logging.usp_InsertExceptionLog
(
      @LogLevel          NVARCHAR(20)
    , @Message           NVARCHAR(1000)
    , @ExceptionMessage  NVARCHAR(MAX) = NULL
    , @StackTrace        NVARCHAR(MAX) = NULL
    , @Source            NVARCHAR(300) = NULL
    , @MethodName        NVARCHAR(200) = NULL
    , @RequestPath       NVARCHAR(500) = NULL
    , @UserId            INT = NULL
    , @IpAddress         NVARCHAR(50) = NULL
    , @MachineName       NVARCHAR(200) = NULL
)
AS
BEGIN

    SET NOCOUNT ON;

    INSERT INTO Logging.Logs
    (
          LogLevel
        , Message
        , ExceptionMessage
        , StackTrace
        , Source
        , MethodName
        , RequestPath
        , UserId
        , IpAddress
        , MachineName
    )
    VALUES
    (
          @LogLevel
        , @Message
        , @ExceptionMessage
        , @StackTrace
        , @Source
        , @MethodName
        , @RequestPath
        , @UserId
        , @IpAddress
        , @MachineName
    );

END;
