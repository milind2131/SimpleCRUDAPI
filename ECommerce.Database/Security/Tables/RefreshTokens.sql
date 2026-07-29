CREATE TABLE [Security].[RefreshTokens] (
    [RefreshTokenId]  INT            IDENTITY (1, 1) NOT NULL,
    [UserId]          INT            NOT NULL,
    [RefreshToken]    NVARCHAR (500) NOT NULL,
    [ExpiryDate]      DATETIME       NOT NULL,
    [IsRevoked]       BIT            DEFAULT ((0)) NOT NULL,
    [CreatedDate]     DATETIME       DEFAULT (getdate()) NOT NULL,
    [CreatedByIp]     NVARCHAR (50)  NULL,
    [RevokedDate]     DATETIME       NULL,
    [ReplacedByToken] NVARCHAR (500) NULL,
    PRIMARY KEY CLUSTERED ([RefreshTokenId] ASC),
    CONSTRAINT [FK_RefreshTokens_Users] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([UserId])
);

