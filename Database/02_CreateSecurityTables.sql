-- =============================================
-- Script de Creación de Tablas - Esquema de Seguridad
-- Proyecto: Ofima.Project.AiTestV1
-- Fecha: 2025-10-21
-- Descripción: Creación de tablas para autenticación y autorización
-- =============================================

USE [OfimaPedidosERP];
GO

-- =============================================
-- Tabla: sec.Users
-- Descripción: Almacena usuarios del sistema con autenticación
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sec].[Users]') AND type in (N'U'))
BEGIN
    CREATE TABLE [sec].[Users] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Username] NVARCHAR(80) NOT NULL,
        [PasswordHash] VARBINARY(256) NOT NULL,
        [Role] NVARCHAR(40) NOT NULL DEFAULT('User'),
        [IsActive] BIT NOT NULL DEFAULT(1),
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [UpdatedAt] DATETIME2(7) NULL,
        [LastLoginAt] DATETIME2(7) NULL,
        
        -- Constraints
        CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Users_Username] UNIQUE NONCLUSTERED ([Username] ASC),
        CONSTRAINT [CK_Users_Role] CHECK ([Role] IN ('Admin', 'Manager', 'User', 'Viewer')),
        CONSTRAINT [CK_Users_Username_Length] CHECK (LEN([Username]) >= 3)
    );
    
    -- Índices para optimizar consultas
    CREATE NONCLUSTERED INDEX [IX_Users_IsActive] ON [sec].[Users] ([IsActive] ASC);
    CREATE NONCLUSTERED INDEX [IX_Users_Role] ON [sec].[Users] ([Role] ASC);
    CREATE NONCLUSTERED INDEX [IX_Users_CreatedAt] ON [sec].[Users] ([CreatedAt] ASC);
    
    PRINT 'Tabla sec.Users creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla sec.Users ya existe';
END
GO

-- =============================================
-- Tabla: sec.UserSessions (Opcional para tracking de sesiones JWT)
-- Descripción: Registro de sesiones activas para auditoría
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sec].[UserSessions]') AND type in (N'U'))
BEGIN
    CREATE TABLE [sec].[UserSessions] (
        [Id] BIGINT IDENTITY(1,1) NOT NULL,
        [UserId] INT NOT NULL,
        [TokenId] NVARCHAR(100) NOT NULL,
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [ExpiresAt] DATETIME2(7) NOT NULL,
        [IsRevoked] BIT NOT NULL DEFAULT(0),
        [RevokedAt] DATETIME2(7) NULL,
        [IpAddress] NVARCHAR(45) NULL,
        [UserAgent] NVARCHAR(500) NULL,
        
        -- Constraints
        CONSTRAINT [PK_UserSessions] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_UserSessions_Users] FOREIGN KEY ([UserId]) 
            REFERENCES [sec].[Users]([Id]) ON DELETE CASCADE,
        CONSTRAINT [UQ_UserSessions_TokenId] UNIQUE NONCLUSTERED ([TokenId] ASC)
    );
    
    -- Índices para optimizar consultas
    CREATE NONCLUSTERED INDEX [IX_UserSessions_UserId] ON [sec].[UserSessions] ([UserId] ASC);
    CREATE NONCLUSTERED INDEX [IX_UserSessions_ExpiresAt] ON [sec].[UserSessions] ([ExpiresAt] ASC);
    CREATE NONCLUSTERED INDEX [IX_UserSessions_IsRevoked] ON [sec].[UserSessions] ([IsRevoked] ASC);
    
    PRINT 'Tabla sec.UserSessions creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla sec.UserSessions ya existe';
END
GO
