CREATE PROCEDURE Logging.usp_InsertExceptionLog
(
      @LogLevel NVARCHAR(20)
    , @Message NVARCHAR(MAX)
    , @ExceptionType NVARCHAR(500) = NULL
    , @ExceptionMessage NVARCHAR(MAX) = NULL
    , @StackTrace NVARCHAR(MAX) = NULL
    , @Source NVARCHAR(300) = NULL
    , @FileName NVARCHAR(500) = NULL
    , @LineNumber INT = NULL
    , @MethodName NVARCHAR(500) = NULL
    , @RequestMethod NVARCHAR(20) = NULL
    , @RequestPath NVARCHAR(500) = NULL
    , @TraceId NVARCHAR(100) = NULL
    , @UserId INT = NULL
    , @IpAddress NVARCHAR(50) = NULL
    , @MachineName NVARCHAR(200) = NULL
)
AS
BEGIN

    SET NOCOUNT ON;

    INSERT INTO Logging.Logs
    (
          LogLevel
        , Message
        , ExceptionType
        , ExceptionMessage
        , StackTrace
        , Source
        , FileName
        , LineNumber
        , MethodName
        , RequestMethod
        , RequestPath
        , TraceId
        , UserId
        , IpAddress
        , MachineName
    )
    VALUES
    (
          @LogLevel
        , @Message
        , @ExceptionType
        , @ExceptionMessage
        , @StackTrace
        , @Source
        , @FileName
        , @LineNumber
        , @MethodName
        , @RequestMethod
        , @RequestPath
        , @TraceId
        , @UserId
        , @IpAddress
        , @MachineName
    );

END;