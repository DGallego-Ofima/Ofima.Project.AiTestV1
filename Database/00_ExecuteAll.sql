-- =============================================
-- Script Maestro de Ejecución
-- Proyecto: Ofima.Project.AiTestV1
-- Fecha: 2025-10-21
-- Descripción: Ejecuta todos los scripts de creación de base de datos en orden
-- =============================================

PRINT '=== INICIANDO CREACIÓN DE BASE DE DATOS ERP - MÓDULO DE PEDIDOS ===';
PRINT 'Fecha y hora: ' + CONVERT(VARCHAR(20), GETDATE(), 120);
PRINT '';

-- =============================================
-- Script 1: Crear Base de Datos y Esquemas
-- =============================================
PRINT '1. Ejecutando creación de base de datos y esquemas...';
:r "01_CreateDatabase.sql"
PRINT 'Script 1 completado.';
PRINT '';

-- =============================================
-- Script 2: Crear Tablas de Seguridad
-- =============================================
PRINT '2. Ejecutando creación de tablas de seguridad...';
:r "02_CreateSecurityTables.sql"
PRINT 'Script 2 completado.';
PRINT '';

-- =============================================
-- Script 3: Crear Tablas ERP
-- =============================================
PRINT '3. Ejecutando creación de tablas ERP...';
:r "03_CreateERPTables.sql"
PRINT 'Script 3 completado.';
PRINT '';

-- =============================================
-- Script 4: Insertar Datos Semilla
-- =============================================
PRINT '4. Ejecutando inserción de datos semilla...';
:r "04_SeedData.sql"
PRINT 'Script 4 completado.';
PRINT '';

-- =============================================
-- Verificación Final
-- =============================================
USE [OfimaPedidosERP];
GO

PRINT '=== VERIFICACIÓN FINAL DE LA BASE DE DATOS ===';

-- Verificar esquemas
SELECT 'ESQUEMAS' AS Tipo, name AS Nombre FROM sys.schemas 
WHERE name IN ('sec', 'erp')
ORDER BY name;

-- Verificar tablas por esquema
SELECT 
    'TABLAS' AS Tipo,
    SCHEMA_NAME(schema_id) AS Esquema,
    name AS Tabla
FROM sys.tables 
WHERE SCHEMA_NAME(schema_id) IN ('sec', 'erp')
ORDER BY SCHEMA_NAME(schema_id), name;

-- Verificar conteo de registros
SELECT 
    'REGISTROS' AS Tipo,
    'sec.Users' AS Tabla,
    COUNT(*) AS Cantidad
FROM [sec].[Users]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.Customers',
    COUNT(*)
FROM [erp].[Customers]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.Products',
    COUNT(*)
FROM [erp].[Products]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.Orders',
    COUNT(*)
FROM [erp].[Orders]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.OrderLines',
    COUNT(*)
FROM [erp].[OrderLines]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.Stocks',
    COUNT(*)
FROM [erp].[Stocks]
UNION ALL
SELECT 
    'REGISTROS',
    'erp.AuditLog',
    COUNT(*)
FROM [erp].[AuditLog];

PRINT '';
PRINT '=== CREACIÓN DE BASE DE DATOS COMPLETADA EXITOSAMENTE ===';
PRINT 'Base de datos: OfimaPedidosERP';
PRINT 'Esquemas: sec (seguridad), erp (lógica de negocio)';
PRINT 'Estado: Lista para desarrollo y pruebas';
PRINT 'Fecha y hora de finalización: ' + CONVERT(VARCHAR(20), GETDATE(), 120);
