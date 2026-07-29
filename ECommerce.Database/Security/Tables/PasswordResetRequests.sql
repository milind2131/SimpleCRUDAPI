CREATE TABLE [Security].[PasswordResetRequests] (
    [PasswordResetRequestId] INT            IDENTITY (1, 1) NOT NULL,
    [UserId]                 INT            NOT NULL,
    [OTPHash]                NVARCHAR (500) NOT NULL,
    [OTPExpiry]              DATETIME       NOT NULL,
    [IsVerified]             BIT            DEFAULT ((0)) NOT NULL,
    [CreatedDate]            DATETIME       DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([PasswordResetRequestId] ASC),
    CONSTRAINT [FK_PasswordResetRequests_Users] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([UserId])
);

