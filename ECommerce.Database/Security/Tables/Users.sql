CREATE TABLE [Security].[Users] (
    [UserId]          INT            IDENTITY (1, 1) NOT NULL,
    [FirstName]       NVARCHAR (100) NOT NULL,
    [LastName]        NVARCHAR (100) NULL,
    [Email]           NVARCHAR (200) NOT NULL,
    [MobileNumber]    NVARCHAR (15)  NULL,
    [PasswordHash]    NVARCHAR (500) NOT NULL,
    [IsEmailVerified] BIT            DEFAULT ((0)) NOT NULL,
    [IsActive]        BIT            DEFAULT ((1)) NOT NULL,
    [LastLogin]       DATETIME2 (7)  NULL,
    [CreatedDate]     DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [ModifiedDate]    DATETIME2 (7)  NULL,
    PRIMARY KEY CLUSTERED ([UserId] ASC),
    UNIQUE NONCLUSTERED ([Email] ASC)
);

