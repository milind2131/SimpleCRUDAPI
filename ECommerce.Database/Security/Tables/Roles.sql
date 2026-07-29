CREATE TABLE [Security].[Roles] (
    [RoleId]      INT            IDENTITY (1, 1) NOT NULL,
    [RoleName]    NVARCHAR (50)  NOT NULL,
    [Description] NVARCHAR (250) NULL,
    [IsActive]    BIT            DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([RoleId] ASC),
    UNIQUE NONCLUSTERED ([RoleName] ASC)
);

