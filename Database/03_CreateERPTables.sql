-- =============================================
-- Script de Creación de Tablas - Esquema ERP
-- Proyecto: Ofima.Project.AiTestV1
-- Fecha: 2025-10-21
-- Descripción: Creación de tablas para lógica de negocio del módulo de pedidos
-- =============================================

USE [OfimaPedidosERP];
GO

-- =============================================
-- Tabla: erp.Customers
-- Descripción: Clientes del sistema
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[Customers]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[Customers] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Name] NVARCHAR(120) NOT NULL,
        [TaxId] NVARCHAR(32) NOT NULL,
        [Email] NVARCHAR(100) NULL,
        [Phone] NVARCHAR(20) NULL,
        [Address] NVARCHAR(200) NULL,
        [IsActive] BIT NOT NULL DEFAULT(1),
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [UpdatedAt] DATETIME2(7) NULL,
        
        -- Constraints
        CONSTRAINT [PK_Customers] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Customers_TaxId] UNIQUE NONCLUSTERED ([TaxId] ASC),
        CONSTRAINT [CK_Customers_Name_Length] CHECK (LEN([Name]) >= 2),
        CONSTRAINT [CK_Customers_TaxId_Length] CHECK (LEN([TaxId]) >= 5)
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_Customers_IsActive] ON [erp].[Customers] ([IsActive] ASC);
    CREATE NONCLUSTERED INDEX [IX_Customers_Name] ON [erp].[Customers] ([Name] ASC);
    CREATE NONCLUSTERED INDEX [IX_Customers_CreatedAt] ON [erp].[Customers] ([CreatedAt] ASC);
    
    PRINT 'Tabla erp.Customers creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.Customers ya existe';
END
GO

-- =============================================
-- Tabla: erp.Products
-- Descripción: Productos disponibles para venta
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[Products]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[Products] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Sku] NVARCHAR(40) NOT NULL,
        [Name] NVARCHAR(120) NOT NULL,
        [Description] NVARCHAR(500) NULL,
        [Price] DECIMAL(18,2) NOT NULL,
        [IsActive] BIT NOT NULL DEFAULT(1),
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [UpdatedAt] DATETIME2(7) NULL,
        
        -- Constraints
        CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Products_Sku] UNIQUE NONCLUSTERED ([Sku] ASC),
        CONSTRAINT [CK_Products_Price] CHECK ([Price] >= 0),
        CONSTRAINT [CK_Products_Name_Length] CHECK (LEN([Name]) >= 2),
        CONSTRAINT [CK_Products_Sku_Length] CHECK (LEN([Sku]) >= 3)
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_Products_IsActive] ON [erp].[Products] ([IsActive] ASC);
    CREATE NONCLUSTERED INDEX [IX_Products_Name] ON [erp].[Products] ([Name] ASC);
    CREATE NONCLUSTERED INDEX [IX_Products_Price] ON [erp].[Products] ([Price] ASC);
    
    PRINT 'Tabla erp.Products creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.Products ya existe';
END
GO

-- =============================================
-- Tabla: erp.Stocks
-- Descripción: Control de inventario por producto
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[Stocks]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[Stocks] (
        [ProductId] INT NOT NULL,
        [OnHand] INT NOT NULL DEFAULT(0),
        [Reserved] INT NOT NULL DEFAULT(0),
        [Available] AS ([OnHand] - [Reserved]) PERSISTED,
        [LastUpdatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [RowVersion] ROWVERSION NOT NULL,
        
        -- Constraints
        CONSTRAINT [PK_Stocks] PRIMARY KEY CLUSTERED ([ProductId] ASC),
        CONSTRAINT [FK_Stocks_Products] FOREIGN KEY ([ProductId]) 
            REFERENCES [erp].[Products]([Id]) ON DELETE CASCADE,
        CONSTRAINT [CK_Stocks_OnHand] CHECK ([OnHand] >= 0),
        CONSTRAINT [CK_Stocks_Reserved] CHECK ([Reserved] >= 0),
        CONSTRAINT [CK_Stocks_Reserved_LTE_OnHand] CHECK ([Reserved] <= [OnHand])
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_Stocks_Available] ON [erp].[Stocks] ([Available] ASC);
    CREATE NONCLUSTERED INDEX [IX_Stocks_LastUpdatedAt] ON [erp].[Stocks] ([LastUpdatedAt] ASC);
    
    PRINT 'Tabla erp.Stocks creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.Stocks ya existe';
END
GO

-- =============================================
-- Tabla: erp.Orders
-- Descripción: Pedidos del sistema
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[Orders]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[Orders] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [Number] NVARCHAR(30) NOT NULL,
        [CustomerId] INT NOT NULL,
        [Status] TINYINT NOT NULL DEFAULT(0), -- 0=Nuevo, 1=Confirmado, 2=Anulado
        [SubTotal] DECIMAL(18,2) NOT NULL DEFAULT(0),
        [TaxAmount] DECIMAL(18,2) NOT NULL DEFAULT(0),
        [Total] DECIMAL(18,2) NOT NULL DEFAULT(0),
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [ConfirmedAt] DATETIME2(7) NULL,
        [CanceledAt] DATETIME2(7) NULL,
        [CreatedBy] INT NOT NULL,
        [Notes] NVARCHAR(500) NULL,
        [RowVersion] ROWVERSION NOT NULL,
        
        -- Constraints
        CONSTRAINT [PK_Orders] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [UQ_Orders_Number] UNIQUE NONCLUSTERED ([Number] ASC),
        CONSTRAINT [FK_Orders_Customers] FOREIGN KEY ([CustomerId]) 
            REFERENCES [erp].[Customers]([Id]),
        CONSTRAINT [FK_Orders_CreatedBy] FOREIGN KEY ([CreatedBy]) 
            REFERENCES [sec].[Users]([Id]),
        CONSTRAINT [CK_Orders_Status] CHECK ([Status] IN (0, 1, 2)),
        CONSTRAINT [CK_Orders_SubTotal] CHECK ([SubTotal] >= 0),
        CONSTRAINT [CK_Orders_TaxAmount] CHECK ([TaxAmount] >= 0),
        CONSTRAINT [CK_Orders_Total] CHECK ([Total] >= 0),
        CONSTRAINT [CK_Orders_ConfirmedAt] CHECK (
            ([Status] = 1 AND [ConfirmedAt] IS NOT NULL) OR 
            ([Status] != 1 AND [ConfirmedAt] IS NULL)
        ),
        CONSTRAINT [CK_Orders_CanceledAt] CHECK (
            ([Status] = 2 AND [CanceledAt] IS NOT NULL) OR 
            ([Status] != 2 AND [CanceledAt] IS NULL)
        )
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_Orders_CustomerId] ON [erp].[Orders] ([CustomerId] ASC);
    CREATE NONCLUSTERED INDEX [IX_Orders_Status] ON [erp].[Orders] ([Status] ASC);
    CREATE NONCLUSTERED INDEX [IX_Orders_CreatedAt] ON [erp].[Orders] ([CreatedAt] ASC);
    CREATE NONCLUSTERED INDEX [IX_Orders_CreatedBy] ON [erp].[Orders] ([CreatedBy] ASC);
    
    PRINT 'Tabla erp.Orders creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.Orders ya existe';
END
GO

-- =============================================
-- Tabla: erp.OrderLines
-- Descripción: Líneas de detalle de los pedidos
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[OrderLines]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[OrderLines] (
        [Id] INT IDENTITY(1,1) NOT NULL,
        [OrderId] INT NOT NULL,
        [ProductId] INT NOT NULL,
        [Qty] INT NOT NULL,
        [UnitPrice] DECIMAL(18,2) NOT NULL,
        [LineTotal] AS ([Qty] * [UnitPrice]) PERSISTED,
        [CreatedAt] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        
        -- Constraints
        CONSTRAINT [PK_OrderLines] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_OrderLines_Orders] FOREIGN KEY ([OrderId]) 
            REFERENCES [erp].[Orders]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_OrderLines_Products] FOREIGN KEY ([ProductId]) 
            REFERENCES [erp].[Products]([Id]),
        CONSTRAINT [CK_OrderLines_Qty] CHECK ([Qty] > 0),
        CONSTRAINT [CK_OrderLines_UnitPrice] CHECK ([UnitPrice] >= 0),
        CONSTRAINT [UQ_OrderLines_OrderId_ProductId] UNIQUE NONCLUSTERED ([OrderId] ASC, [ProductId] ASC)
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_OrderLines_OrderId] ON [erp].[OrderLines] ([OrderId] ASC);
    CREATE NONCLUSTERED INDEX [IX_OrderLines_ProductId] ON [erp].[OrderLines] ([ProductId] ASC);
    
    PRINT 'Tabla erp.OrderLines creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.OrderLines ya existe';
END
GO

-- =============================================
-- Tabla: erp.AuditLog
-- Descripción: Registro de auditoría del sistema
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[erp].[AuditLog]') AND type in (N'U'))
BEGIN
    CREATE TABLE [erp].[AuditLog] (
        [Id] BIGINT IDENTITY(1,1) NOT NULL,
        [UserId] INT NOT NULL,
        [Entity] NVARCHAR(40) NOT NULL,
        [EntityId] INT NOT NULL,
        [Action] NVARCHAR(30) NOT NULL,
        [At] DATETIME2(7) NOT NULL DEFAULT(SYSUTCDATETIME()),
        [Payload] NVARCHAR(MAX) NULL,
        [IpAddress] NVARCHAR(45) NULL,
        [UserAgent] NVARCHAR(500) NULL,
        
        -- Constraints
        CONSTRAINT [PK_AuditLog] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_AuditLog_Users] FOREIGN KEY ([UserId]) 
            REFERENCES [sec].[Users]([Id]),
        CONSTRAINT [CK_AuditLog_Action] CHECK ([Action] IN (
            'CREATE', 'UPDATE', 'DELETE', 'CONFIRM', 'CANCEL', 'LOGIN', 'LOGOUT'
        ))
    );
    
    -- Índices
    CREATE NONCLUSTERED INDEX [IX_AuditLog_UserId] ON [erp].[AuditLog] ([UserId] ASC);
    CREATE NONCLUSTERED INDEX [IX_AuditLog_Entity] ON [erp].[AuditLog] ([Entity] ASC);
    CREATE NONCLUSTERED INDEX [IX_AuditLog_EntityId] ON [erp].[AuditLog] ([EntityId] ASC);
    CREATE NONCLUSTERED INDEX [IX_AuditLog_Action] ON [erp].[AuditLog] ([Action] ASC);
    CREATE NONCLUSTERED INDEX [IX_AuditLog_At] ON [erp].[AuditLog] ([At] ASC);
    
    PRINT 'Tabla erp.AuditLog creada exitosamente';
END
ELSE
BEGIN
    PRINT 'Tabla erp.AuditLog ya existe';
END
GO

PRINT 'Todas las tablas del esquema ERP han sido creadas exitosamente';
