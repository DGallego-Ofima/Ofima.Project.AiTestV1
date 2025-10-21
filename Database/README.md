# Base de Datos - Sistema ERP Módulo de Pedidos

## Descripción

Este directorio contiene todos los scripts SQL necesarios para crear y configurar la base de datos del sistema de gestión de pedidos ERP.

## Estructura de Archivos

```
Database/
├── 00_ExecuteAll.sql          # Script maestro que ejecuta todos los demás
├── 01_CreateDatabase.sql      # Creación de BD y esquemas
├── 02_CreateSecurityTables.sql # Tablas del esquema 'sec'
├── 03_CreateERPTables.sql     # Tablas del esquema 'erp'
├── 04_SeedData.sql           # Datos iniciales y de prueba
└── README.md                 # Este archivo
```

## Requisitos Previos

- Microsoft SQL Server 2019 o superior
- SQL Server Management Studio (SSMS) o sqlcmd
- Permisos de administrador (sysadmin) en SQL Server

## Instalación Rápida

### Opción 1: Script Maestro (Recomendado)

```bash
sqlcmd -S localhost -E -i "00_ExecuteAll.sql"
```

### Opción 2: Scripts Individuales

```bash
sqlcmd -S localhost -E -d master -i "01_CreateDatabase.sql"
sqlcmd -S localhost -E -d OfimaPedidosERP -i "02_CreateSecurityTables.sql"
sqlcmd -S localhost -E -d OfimaPedidosERP -i "03_CreateERPTables.sql"
sqlcmd -S localhost -E -d OfimaPedidosERP -i "04_SeedData.sql"
```

### Opción 3: SQL Server Management Studio

1. Abrir SSMS y conectarse al servidor
2. Ejecutar los scripts en el siguiente orden:
   - `01_CreateDatabase.sql`
   - `02_CreateSecurityTables.sql`
   - `03_CreateERPTables.sql`
   - `04_SeedData.sql`

## Configuración de Conexión

### Cadena de Conexión .NET

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OfimaPedidosERP;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

### Cadena de Conexión con Usuario/Contraseña

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OfimaPedidosERP;User Id=sa;Password=YourPassword;TrustServerCertificate=true;"
  }
}
```

## Estructura de la Base de Datos

### Esquemas

- **`sec`**: Seguridad, usuarios y autenticación
- **`erp`**: Lógica de negocio del módulo de pedidos

### Tablas Principales

#### Esquema `sec`
- `Users`: Usuarios del sistema
- `UserSessions`: Sesiones JWT activas

#### Esquema `erp`
- `Customers`: Clientes
- `Products`: Productos
- `Stocks`: Control de inventario
- `Orders`: Pedidos
- `OrderLines`: Líneas de pedido
- `AuditLog`: Auditoría del sistema

## Datos de Prueba

### Usuarios Predefinidos

| Usuario | Contraseña | Rol | Estado |
|---------|------------|-----|--------|
| admin | 123456 | Admin | Activo |
| manager | 123456 | Manager | Activo |
| vendedor1 | 123456 | User | Activo |
| vendedor2 | 123456 | User | Activo |
| viewer | 123456 | Viewer | Activo |

### Datos de Ejemplo

- 6 clientes de prueba
- 10 productos con stock inicial
- 4 pedidos en diferentes estados
- Registros de auditoría

## Verificación de Instalación

### Consulta de Verificación

```sql
USE OfimaPedidosERP;

-- Verificar esquemas
SELECT name FROM sys.schemas WHERE name IN ('sec', 'erp');

-- Verificar tablas
SELECT 
    SCHEMA_NAME(schema_id) AS Esquema,
    name AS Tabla
FROM sys.tables 
WHERE SCHEMA_NAME(schema_id) IN ('sec', 'erp')
ORDER BY Esquema, Tabla;

-- Verificar datos
SELECT 'Users' AS Tabla, COUNT(*) AS Registros FROM sec.Users
UNION ALL
SELECT 'Customers', COUNT(*) FROM erp.Customers
UNION ALL
SELECT 'Products', COUNT(*) FROM erp.Products
UNION ALL
SELECT 'Orders', COUNT(*) FROM erp.Orders;
```

### Resultado Esperado

```
Esquema | Tabla
--------|--------
erp     | AuditLog
erp     | Customers
erp     | OrderLines
erp     | Orders
erp     | Products
erp     | Stocks
sec     | Users
sec     | UserSessions

Tabla     | Registros
----------|----------
Users     | 6
Customers | 6
Products  | 10
Orders    | 4
```

## Mantenimiento

### Backup Recomendado

```sql
-- Backup completo
BACKUP DATABASE OfimaPedidosERP 
TO DISK = 'C:\Backups\OfimaPedidosERP_Full.bak'
WITH FORMAT, INIT;

-- Backup diferencial
BACKUP DATABASE OfimaPedidosERP 
TO DISK = 'C:\Backups\OfimaPedidosERP_Diff.bak'
WITH DIFFERENTIAL;
```

### Limpieza de Datos de Prueba

```sql
-- Limpiar solo datos, mantener estructura
USE OfimaPedidosERP;

DELETE FROM erp.AuditLog;
DELETE FROM erp.OrderLines;
DELETE FROM erp.Orders;
DELETE FROM erp.Stocks;
DELETE FROM erp.Products;
DELETE FROM erp.Customers;
DELETE FROM sec.UserSessions;
DELETE FROM sec.Users WHERE Username != 'admin';

-- Resetear identities
DBCC CHECKIDENT ('sec.Users', RESEED, 1);
DBCC CHECKIDENT ('erp.Customers', RESEED, 0);
DBCC CHECKIDENT ('erp.Products', RESEED, 0);
DBCC CHECKIDENT ('erp.Orders', RESEED, 0);
```

### Recreación Completa

```sql
-- Eliminar base de datos (¡CUIDADO!)
USE master;
DROP DATABASE IF EXISTS OfimaPedidosERP;

-- Luego ejecutar scripts de creación nuevamente
```

## Troubleshooting

### Error: "Database already exists"

```sql
-- Verificar si existe
SELECT name FROM sys.databases WHERE name = 'OfimaPedidosERP';

-- Si existe y quiere recrear
USE master;
ALTER DATABASE OfimaPedidosERP SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
DROP DATABASE OfimaPedidosERP;
```

### Error: "Login failed"

- Verificar que SQL Server esté ejecutándose
- Confirmar permisos del usuario
- Para autenticación Windows: ejecutar como administrador
- Para autenticación SQL: verificar usuario y contraseña

### Error: "Object already exists"

Los scripts incluyen verificaciones `IF NOT EXISTS`, por lo que es seguro ejecutarlos múltiples veces.

## Contacto y Soporte

Para problemas o dudas sobre la base de datos:

1. Revisar la documentación en `Docs/Architecture/DatabaseArchitecture.md`
2. Verificar los logs de SQL Server
3. Consultar el plan de trabajo en `Docs/AdministrativePlans/WorkPlan.md`

---

**Creado**: 2025-10-21  
**Proyecto**: Ofima.Project.AiTestV1  
**Módulo**: Base de Datos ERP Pedidos
