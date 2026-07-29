CREATE TABLE [Security].[RefreshTokens] (
    [RefreshTokenId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]         INT            NOT NULL,
    [Token]          NVARCHAR (500) NOT NULL,
    [ExpiryDate]     DATETIME2 (7)  NOT NULL,
    [CreatedDate]    DATETIME2 (7)  DEFAULT (getdate()) NOT NULL,
    [IsRevoked]      BIT            DEFAULT ((0)) NOT NULL,
    PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC),
    CONSTRAINT [FK_RefreshTokens_User] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([UserId])
);

