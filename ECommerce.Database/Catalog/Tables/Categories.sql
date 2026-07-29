CREATE TABLE [Catalog].[Categories] (
    [CategoryId]   INT            IDENTITY (1, 1) NOT NULL,
    [CategoryName] NVARCHAR (100) NOT NULL,
    [Description]  NVARCHAR (250) NULL,
    [IsActive]     BIT            DEFAULT ((1)) NOT NULL,
    [CreatedDate]  DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([CategoryId] ASC),
    UNIQUE NONCLUSTERED ([CategoryName] ASC)
);

