CREATE TABLE [Logging].[Logs]
(
    [LogId] BIGINT IDENTITY (1,1) NOT NULL,

    [LogLevel] NVARCHAR(20) NOT NULL,

    [Message] NVARCHAR(MAX) NOT NULL,

    [ExceptionType] NVARCHAR(500) NULL,

    [ExceptionMessage] NVARCHAR(MAX) NULL,

    [StackTrace] NVARCHAR(MAX) NULL,

    [Source] NVARCHAR(300) NULL,

    [FileName] NVARCHAR(500) NULL,

    [LineNumber] INT NULL,

    [MethodName] NVARCHAR(500) NULL,

    [RequestMethod] NVARCHAR(20) NULL,

    [RequestPath] NVARCHAR(500) NULL,

    [TraceId] NVARCHAR(100) NULL,

    [UserId] INT NULL,

    [IpAddress] NVARCHAR(50) NULL,

    [MachineName] NVARCHAR(200) NULL,

    [LoggedOn] DATETIME2(7)
        DEFAULT SYSUTCDATETIME() NOT NULL,

    PRIMARY KEY CLUSTERED ([LogId] ASC)
);