CREATE TABLE [Security].[UserRoles] (
    [UserRoleId]   INT           IDENTITY (1, 1) NOT NULL,
    [UserId]       INT           NOT NULL,
    [RoleId]       INT           NOT NULL,
    [AssignedDate] DATETIME2 (7) DEFAULT (getdate()) NOT NULL,
    PRIMARY KEY CLUSTERED ([UserRoleId] ASC),
    CONSTRAINT [FK_UserRoles_Role] FOREIGN KEY ([RoleId]) REFERENCES [Security].[Roles] ([RoleId]),
    CONSTRAINT [FK_UserRoles_User] FOREIGN KEY ([UserId]) REFERENCES [Security].[Users] ([UserId]),
    CONSTRAINT [UQ_UserRole] UNIQUE NONCLUSTERED ([UserId] ASC, [RoleId] ASC)
);

