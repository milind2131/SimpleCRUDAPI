CREATE TABLE [Logging].[Logs] (
    [LogId]            BIGINT          IDENTITY (1, 1) NOT NULL,
    [LogLevel]         NVARCHAR (20)   NOT NULL,
    [Message]          NVARCHAR (1000) NOT NULL,
    [ExceptionMessage] NVARCHAR (MAX)  NULL,
    [StackTrace]       NVARCHAR (MAX)  NULL,
    [Source]           NVARCHAR (300)  NULL,
    [MethodName]       NVARCHAR (200)  NULL,
    [RequestPath]      NVARCHAR (500)  NULL,
    [UserId]           INT             NULL,
    [IpAddress]        NVARCHAR (50)   NULL,
    [MachineName]      NVARCHAR (200)  NULL,
    [LoggedOn]         DATETIME2 (7)   DEFAULT (sysutcdatetime()) NOT NULL,
    PRIMARY KEY CLUSTERED ([LogId] ASC)
);

