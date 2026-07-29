CREATE TABLE [Security].[PendingUsers] (
    [PendingUserId] INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]     NVARCHAR (100) NOT NULL,
    [LastName]      NVARCHAR (100) NOT NULL,
    [Email]         NVARCHAR (200) NOT NULL,
    [MobileNumber]  NVARCHAR (15)  NOT NULL,
    [PasswordHash]  NVARCHAR (500) NOT NULL,
    [OTPExpiry]     DATETIME       NOT NULL,
    [CreatedDate]   DATETIME       DEFAULT (getdate()) NOT NULL,
    [OTPHash]       NVARCHAR (500) NOT NULL,
    PRIMARY KEY CLUSTERED ([PendingUserId] ASC)
);

