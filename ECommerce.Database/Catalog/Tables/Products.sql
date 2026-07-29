CREATE TABLE [Catalog].[Products] (
    [ProductId]   INT             IDENTITY (1, 1) NOT NULL,
    [ProductName] NVARCHAR (200)  NOT NULL,
    [Price]       DECIMAL (18, 2) NOT NULL,
    [CategoryId]  INT             NOT NULL,
    [IsActive]    BIT             DEFAULT ((1)) NOT NULL,
    [CreatedDate] DATETIME2 (7)   DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([ProductId] ASC),
    CONSTRAINT [FK_Product_Category] FOREIGN KEY ([CategoryId]) REFERENCES [Catalog].[Categories] ([CategoryId])
);

