-- =============================================
-- Script de Datos Semilla (Seeds)
-- Proyecto: Ofima.Project.AiTestV1
-- Fecha: 2025-10-21
-- Descripción: Datos iniciales para pruebas y desarrollo del sistema
-- =============================================

USE [OfimaPedidosERP];
GO

-- =============================================
-- Insertar Usuarios del Sistema
-- =============================================
PRINT 'Insertando usuarios del sistema...';

-- Limpiar datos existentes (solo para desarrollo)
DELETE FROM [erp].[AuditLog];
DELETE FROM [erp].[OrderLines];
DELETE FROM [erp].[Orders];
DELETE FROM [erp].[Stocks];
DELETE FROM [erp].[Products];
DELETE FROM [erp].[Customers];
DELETE FROM [sec].[UserSessions];
DELETE FROM [sec].[Users];

-- Resetear identities
DBCC CHECKIDENT ('[sec].[Users]', RESEED, 0);
DBCC CHECKIDENT ('[erp].[Customers]', RESEED, 0);
DBCC CHECKIDENT ('[erp].[Products]', RESEED, 0);
DBCC CHECKIDENT ('[erp].[Orders]', RESEED, 0);
DBCC CHECKIDENT ('[erp].[OrderLines]', RESEED, 0);
DBCC CHECKIDENT ('[erp].[AuditLog]', RESEED, 0);

-- Usuarios (Password: "123456" - hash simple para desarrollo)
INSERT INTO [sec].[Users] ([Username], [PasswordHash], [Role], [IsActive], [CreatedAt])
VALUES 
    ('admin', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'Admin', 1, SYSUTCDATETIME()),
    ('manager', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'Manager', 1, SYSUTCDATETIME()),
    ('vendedor1', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'User', 1, SYSUTCDATETIME()),
    ('vendedor2', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'User', 1, SYSUTCDATETIME()),
    ('viewer', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'Viewer', 1, SYSUTCDATETIME()),
    ('inactive_user', 0x8D969EEF6ECAD3C29A3A629280E686CF0C3F5D5A86AFF3CA12020C923ADC6C92, 'User', 0, SYSUTCDATETIME());

PRINT 'Usuarios insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Insertar Clientes
-- =============================================
PRINT 'Insertando clientes...';

INSERT INTO [erp].[Customers] ([Name], [TaxId], [Email], [Phone], [Address], [IsActive], [CreatedAt])
VALUES 
    ('Distribuidora El Sol S.A.S', '900123456-1', 'contacto@elsol.com', '3001234567', 'Calle 123 #45-67, Bogotá', 1, SYSUTCDATETIME()),
    ('Comercializadora Luna Ltda', '800987654-2', 'ventas@luna.com', '3109876543', 'Carrera 45 #12-34, Medellín', 1, SYSUTCDATETIME()),
    ('Supermercados Estrella', '700456789-3', 'compras@estrella.com', '3201122334', 'Avenida 68 #23-45, Cali', 1, SYSUTCDATETIME()),
    ('Tiendas Cometa S.A.S', '600321654-4', 'pedidos@cometa.com', '3154567890', 'Calle 50 #78-90, Barranquilla', 1, SYSUTCDATETIME()),
    ('Mayorista Planeta', '500789123-5', 'info@planeta.com', '3187654321', 'Carrera 15 #34-56, Bucaramanga', 1, SYSUTCDATETIME()),
    ('Cliente Inactivo S.A.S', '400111222-6', 'inactivo@test.com', '3000000000', 'Dirección Inactiva', 0, SYSUTCDATETIME());

PRINT 'Clientes insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Insertar Productos
-- =============================================
PRINT 'Insertando productos...';

INSERT INTO [erp].[Products] ([Sku], [Name], [Description], [Price], [IsActive], [CreatedAt])
VALUES 
    ('LAPTOP001', 'Laptop Dell Inspiron 15', 'Laptop Dell Inspiron 15 - Intel i5, 8GB RAM, 256GB SSD', 2500000.00, 1, SYSUTCDATETIME()),
    ('MOUSE001', 'Mouse Inalámbrico Logitech', 'Mouse inalámbrico Logitech M705 con batería de larga duración', 85000.00, 1, SYSUTCDATETIME()),
    ('KEYB001', 'Teclado Mecánico Gaming', 'Teclado mecánico RGB para gaming con switches Cherry MX', 320000.00, 1, SYSUTCDATETIME()),
    ('MON001', 'Monitor LED 24 pulgadas', 'Monitor LED Full HD 24" con conexión HDMI y VGA', 450000.00, 1, SYSUTCDATETIME()),
    ('PRINT001', 'Impresora Multifuncional HP', 'Impresora HP DeskJet 2775 - Imprime, escanea y copia', 280000.00, 1, SYSUTCDATETIME()),
    ('CABLE001', 'Cable HDMI 2 metros', 'Cable HDMI de alta velocidad 2 metros', 25000.00, 1, SYSUTCDATETIME()),
    ('USB001', 'Memoria USB 32GB', 'Memoria USB 3.0 de 32GB Kingston', 35000.00, 1, SYSUTCDATETIME()),
    ('WEBCAM001', 'Cámara Web HD', 'Cámara web HD 1080p con micrófono integrado', 120000.00, 1, SYSUTCDATETIME()),
    ('SPEAKER001', 'Altavoces Bluetooth', 'Altavoces Bluetooth portátiles con sonido estéreo', 180000.00, 1, SYSUTCDATETIME()),
    ('TABLET001', 'Tablet Android 10"', 'Tablet Android 10 pulgadas, 4GB RAM, 64GB almacenamiento', 650000.00, 1, SYSUTCDATETIME());

PRINT 'Productos insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Insertar Stock Inicial
-- =============================================
PRINT 'Insertando stock inicial...';

INSERT INTO [erp].[Stocks] ([ProductId], [OnHand], [Reserved], [LastUpdatedAt])
SELECT 
    p.[Id],
    CASE 
        WHEN p.[Sku] LIKE 'LAPTOP%' THEN 15
        WHEN p.[Sku] LIKE 'TABLET%' THEN 25
        WHEN p.[Sku] LIKE 'MON%' THEN 30
        WHEN p.[Sku] LIKE 'PRINT%' THEN 20
        ELSE 50
    END AS [OnHand],
    0 AS [Reserved],
    SYSUTCDATETIME()
FROM [erp].[Products] p
WHERE p.[IsActive] = 1;

PRINT 'Stock inicial insertado: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Insertar Pedidos de Ejemplo
-- =============================================
PRINT 'Insertando pedidos de ejemplo...';

-- Pedido 1: Estado Nuevo (CreatedBy = vendedor1 = ID 3)
DECLARE @OrderId1 INT;
INSERT INTO [erp].[Orders] ([Number], [CustomerId], [Status], [SubTotal], [TaxAmount], [Total], [CreatedBy], [Notes], [CreatedAt])
VALUES ('PED-2024-001', 1, 0, 2585000.00, 491150.00, 3076150.00, 3, 'Pedido inicial para pruebas', SYSUTCDATETIME());
SET @OrderId1 = SCOPE_IDENTITY();

INSERT INTO [erp].[OrderLines] ([OrderId], [ProductId], [Qty], [UnitPrice])
VALUES 
    (@OrderId1, 1, 1, 2500000.00), -- Laptop
    (@OrderId1, 2, 1, 85000.00);   -- Mouse

-- Pedido 2: Estado Confirmado (CreatedBy = vendedor1 = ID 3)
DECLARE @OrderId2 INT;
INSERT INTO [erp].[Orders] ([Number], [CustomerId], [Status], [SubTotal], [TaxAmount], [Total], [CreatedBy], [Notes], [CreatedAt], [ConfirmedAt])
VALUES ('PED-2024-002', 2, 1, 1000000.00, 190000.00, 1190000.00, 3, 'Pedido confirmado', DATEADD(HOUR, -2, SYSUTCDATETIME()), DATEADD(HOUR, -1, SYSUTCDATETIME()));
SET @OrderId2 = SCOPE_IDENTITY();

INSERT INTO [erp].[OrderLines] ([OrderId], [ProductId], [Qty], [UnitPrice])
VALUES 
    (@OrderId2, 3, 2, 320000.00), -- Teclados
    (@OrderId2, 4, 1, 450000.00), -- Monitor
    (@OrderId2, 6, 4, 25000.00);  -- Cables

-- Actualizar stock reservado para pedido confirmado
UPDATE [erp].[Stocks] SET [Reserved] = 2, [LastUpdatedAt] = SYSUTCDATETIME() WHERE [ProductId] = 3;
UPDATE [erp].[Stocks] SET [Reserved] = 1, [LastUpdatedAt] = SYSUTCDATETIME() WHERE [ProductId] = 4;
UPDATE [erp].[Stocks] SET [Reserved] = 4, [LastUpdatedAt] = SYSUTCDATETIME() WHERE [ProductId] = 6;

-- Pedido 3: Estado Anulado (CreatedBy = vendedor2 = ID 4)
DECLARE @OrderId3 INT;
INSERT INTO [erp].[Orders] ([Number], [CustomerId], [Status], [SubTotal], [TaxAmount], [Total], [CreatedBy], [Notes], [CreatedAt], [CanceledAt])
VALUES ('PED-2024-003', 3, 2, 460000.00, 87400.00, 547400.00, 4, 'Pedido anulado por cliente', DATEADD(HOUR, -5, SYSUTCDATETIME()), DATEADD(HOUR, -3, SYSUTCDATETIME()));
SET @OrderId3 = SCOPE_IDENTITY();

INSERT INTO [erp].[OrderLines] ([OrderId], [ProductId], [Qty], [UnitPrice])
VALUES 
    (@OrderId3, 5, 1, 280000.00), -- Impresora
    (@OrderId3, 8, 1, 120000.00), -- Webcam
    (@OrderId3, 7, 2, 35000.00);  -- USB

-- Pedido 4: Otro pedido nuevo (CreatedBy = vendedor1 = ID 3)
DECLARE @OrderId4 INT;
INSERT INTO [erp].[Orders] ([Number], [CustomerId], [Status], [SubTotal], [TaxAmount], [Total], [CreatedBy], [Notes], [CreatedAt])
VALUES ('PED-2024-004', 4, 0, 830000.00, 157700.00, 987700.00, 3, 'Pedido para equipos de oficina', SYSUTCDATETIME());
SET @OrderId4 = SCOPE_IDENTITY();

INSERT INTO [erp].[OrderLines] ([OrderId], [ProductId], [Qty], [UnitPrice])
VALUES 
    (@OrderId4, 9, 2, 180000.00), -- Speakers
    (@OrderId4, 10, 1, 650000.00); -- Tablet

PRINT 'Pedidos de ejemplo insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Insertar Registros de Auditoría
-- =============================================
PRINT 'Insertando registros de auditoría...';

INSERT INTO [erp].[AuditLog] ([UserId], [Entity], [EntityId], [Action], [At], [Payload])
VALUES 
    (3, 'Order', @OrderId1, 'CREATE', SYSUTCDATETIME(), '{"orderId": ' + CAST(@OrderId1 AS VARCHAR) + ', "customerId": 1, "total": 3076150.00}'),
    (3, 'Order', @OrderId2, 'CREATE', DATEADD(HOUR, -2, SYSUTCDATETIME()), '{"orderId": ' + CAST(@OrderId2 AS VARCHAR) + ', "customerId": 2, "total": 1190000.00}'),
    (3, 'Order', @OrderId2, 'CONFIRM', DATEADD(HOUR, -1, SYSUTCDATETIME()), '{"orderId": ' + CAST(@OrderId2 AS VARCHAR) + ', "status": "Confirmed", "stockReserved": true}'),
    (4, 'Order', @OrderId3, 'CREATE', DATEADD(HOUR, -5, SYSUTCDATETIME()), '{"orderId": ' + CAST(@OrderId3 AS VARCHAR) + ', "customerId": 3, "total": 547400.00}'),
    (4, 'Order', @OrderId3, 'CANCEL', DATEADD(HOUR, -3, SYSUTCDATETIME()), '{"orderId": ' + CAST(@OrderId3 AS VARCHAR) + ', "status": "Canceled", "reason": "Cliente canceló"}'),
    (3, 'Order', @OrderId4, 'CREATE', SYSUTCDATETIME(), '{"orderId": ' + CAST(@OrderId4 AS VARCHAR) + ', "customerId": 4, "total": 987700.00}');

PRINT 'Registros de auditoría insertados: ' + CAST(@@ROWCOUNT AS VARCHAR(10));

-- =============================================
-- Verificar Datos Insertados
-- =============================================
PRINT '=== RESUMEN DE DATOS INSERTADOS ===';

DECLARE @UserCount INT, @CustomerCount INT, @ProductCount INT, @StockCount INT, @OrderCount INT, @OrderLineCount INT, @AuditCount INT;

SELECT @UserCount = COUNT(*) FROM [sec].[Users];
SELECT @CustomerCount = COUNT(*) FROM [erp].[Customers];
SELECT @ProductCount = COUNT(*) FROM [erp].[Products];
SELECT @StockCount = COUNT(*) FROM [erp].[Stocks];
SELECT @OrderCount = COUNT(*) FROM [erp].[Orders];
SELECT @OrderLineCount = COUNT(*) FROM [erp].[OrderLines];
SELECT @AuditCount = COUNT(*) FROM [erp].[AuditLog];

PRINT 'Usuarios: ' + CAST(@UserCount AS VARCHAR(10));
PRINT 'Clientes: ' + CAST(@CustomerCount AS VARCHAR(10));
PRINT 'Productos: ' + CAST(@ProductCount AS VARCHAR(10));
PRINT 'Stock: ' + CAST(@StockCount AS VARCHAR(10));
PRINT 'Pedidos: ' + CAST(@OrderCount AS VARCHAR(10));
PRINT 'Líneas de pedido: ' + CAST(@OrderLineCount AS VARCHAR(10));
PRINT 'Registros de auditoría: ' + CAST(@AuditCount AS VARCHAR(10));

-- =============================================
-- Consultas de Verificación
-- =============================================
PRINT '=== CONSULTAS DE VERIFICACIÓN ===';

-- Usuarios activos
SELECT 'USUARIOS ACTIVOS' AS Tipo, Username, Role FROM [sec].[Users] WHERE IsActive = 1;

-- Clientes activos
SELECT 'CLIENTES ACTIVOS' AS Tipo, Name, TaxId FROM [erp].[Customers] WHERE IsActive = 1;

-- Stock disponible
SELECT 
    'STOCK DISPONIBLE' AS Tipo,
    p.Sku, 
    p.Name, 
    s.OnHand, 
    s.Reserved, 
    s.Available
FROM [erp].[Products] p
INNER JOIN [erp].[Stocks] s ON p.Id = s.ProductId
WHERE p.IsActive = 1
ORDER BY p.Sku;

-- Resumen de pedidos por estado
SELECT 
    'PEDIDOS POR ESTADO' AS Tipo,
    CASE Status 
        WHEN 0 THEN 'Nuevo'
        WHEN 1 THEN 'Confirmado'
        WHEN 2 THEN 'Anulado'
    END AS Estado,
    COUNT(*) AS Cantidad,
    SUM(Total) AS TotalVentas
FROM [erp].[Orders]
GROUP BY Status
ORDER BY Status;

PRINT 'Datos semilla insertados exitosamente';
