-- =============================================
-- Script de Creación de Base de Datos ERP - Módulo de Pedidos
-- Proyecto: Ofima.Project.AiTestV1
-- Fecha: 2025-10-21
-- Descripción: Creación de base de datos para sistema de gestión de pedidos
-- Stack: SQL Server
-- =============================================

-- Crear la base de datos
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = N'OfimaPedidosERP')
BEGIN
    CREATE DATABASE [OfimaPedidosERP]
    COLLATE SQL_Latin1_General_CP1_CI_AS;
END
GO

USE [OfimaPedidosERP];
GO

-- Configurar opciones de la base de datos
ALTER DATABASE [OfimaPedidosERP] SET RECOVERY SIMPLE;
ALTER DATABASE [OfimaPedidosERP] SET AUTO_CLOSE OFF;
ALTER DATABASE [OfimaPedidosERP] SET AUTO_SHRINK OFF;
GO

-- =============================================
-- Crear Esquemas
-- =============================================

-- Esquema para seguridad y usuarios
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'sec')
BEGIN
    EXEC('CREATE SCHEMA [sec]');
END
GO

-- Esquema para lógica de negocio ERP
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = N'erp')
BEGIN
    EXEC('CREATE SCHEMA [erp]');
END
GO

PRINT 'Base de datos OfimaPedidosERP creada exitosamente con esquemas sec y erp';
