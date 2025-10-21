-- Script de prueba para inserción de usuarios
USE [OfimaPedidosERP];
GO

-- Limpiar usuarios existentes
DELETE FROM [sec].[Users];
DBCC CHECKIDENT ('[sec].[Users]', RESEED, 0);

-- Insertar usuarios con hash directo
INSERT INTO [sec].[Users] ([Username], [PasswordHash], [Role], [IsActive], [CreatedAt])
VALUES 
    ('admin', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'Admin', 1, SYSUTCDATETIME()),
    ('vendedor1', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'User', 1, SYSUTCDATETIME());

-- Verificar inserción
SELECT 
    Id, 
    Username, 
    Role, 
    IsActive,
    LEN(PasswordHash) as HashLength,
    CreatedAt
FROM [sec].[Users];

PRINT 'Usuarios insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));
