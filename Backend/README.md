# Ofima Orders API - Backend

## Descripción

API REST para el sistema de gestión de pedidos ERP desarrollado con .NET 8 siguiendo Clean Architecture.

## Arquitectura

```
Backend/
├── Ofima.Orders.Domain/          # Entidades, interfaces, reglas de negocio
├── Ofima.Orders.Application/     # Casos de uso, DTOs, servicios
├── Ofima.Orders.Infrastructure/  # EF Core, repositorios, servicios externos
├── Ofima.Orders.API/            # Controllers, middleware, configuración
└── Ofima.Orders.Tests/          # Pruebas unitarias e integración
```

## Tecnologías Utilizadas

- **.NET 8** - Framework principal
- **Entity Framework Core** - ORM para acceso a datos
- **SQL Server** - Base de datos
- **JWT Bearer** - Autenticación
- **Swagger/OpenAPI** - Documentación de API
- **AutoMapper** - Mapeo de DTOs (futuro)
- **FluentValidation** - Validaciones (futuro)

## Configuración

### 1. Base de Datos

Asegúrate de que la base de datos `OfimaPedidosERP` esté creada y configurada. Los scripts están en la carpeta `../Database/`.

### 2. Cadena de Conexión

Actualiza la cadena de conexión en `appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=OfimaPedidosERP;Integrated Security=true;TrustServerCertificate=true;"
  }
}
```

### 3. JWT Configuration

La clave JWT está configurada en `appsettings.json`. Para producción, usa una clave más segura:

```json
{
  "Jwt": {
    "Key": "your-secret-key-here-must-be-at-least-32-characters-long-for-security"
  }
}
```

## Ejecución

### Desarrollo Local

```bash
# Navegar al directorio del proyecto API
cd Ofima.Orders.API

# Restaurar dependencias
dotnet restore

# Ejecutar la aplicación
dotnet run
```

La API estará disponible en:
- **HTTP**: `http://localhost:5000`
- **HTTPS**: `https://localhost:5001`
- **Swagger UI**: `http://localhost:5000` (raíz)

### Compilación

```bash
# Compilar toda la solución
dotnet build

# Compilar en modo Release
dotnet build --configuration Release
```

## Endpoints Principales

### Autenticación

- `POST /api/v1/auth/login` - Iniciar sesión
- `POST /api/v1/auth/validate` - Validar token

### Clientes

- `GET /api/v1/customers?active=true` - Obtener clientes activos
- `GET /api/v1/customers/{id}` - Obtener cliente por ID
- `POST /api/v1/customers` - Crear cliente
- `PUT /api/v1/customers/{id}` - Actualizar cliente
- `DELETE /api/v1/customers/{id}` - Eliminar cliente (soft delete)

### Pedidos

- `GET /api/v1/orders` - Obtener pedidos con filtros y paginación
- `GET /api/v1/orders/{id}` - Obtener pedido por ID
- `POST /api/v1/orders` - Crear pedido
- `PUT /api/v1/orders/{id}` - Actualizar pedido (solo estado Nuevo)
- `POST /api/v1/orders/{id}/confirm` - Confirmar pedido
- `POST /api/v1/orders/{id}/cancel` - Anular pedido

### Health Check

- `GET /health` - Estado de la API

## Autenticación

### Obtener Token

```bash
curl -X POST "http://localhost:5000/api/v1/auth/login" \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "123456"
  }'
```

### Usar Token en Requests

```bash
curl -X GET "http://localhost:5000/api/v1/customers?active=true" \
  -H "Authorization: Bearer YOUR_JWT_TOKEN"
```

## Usuarios de Prueba

| Usuario | Contraseña | Rol | Estado |
|---------|------------|-----|--------|
| admin | 123456 | Admin | Activo |
| manager | 123456 | Manager | Activo |
| vendedor1 | 123456 | User | Activo |
| vendedor2 | 123456 | User | Activo |
| viewer | 123456 | Viewer | Activo |

## Estructura de Respuestas

### Respuesta Exitosa

```json
{
  "success": true,
  "data": { ... },
  "message": "Operation completed successfully"
}
```

### Respuesta con Error

```json
{
  "success": false,
  "message": "Error message",
  "errors": ["Detailed error 1", "Detailed error 2"]
}
```

### Respuesta Paginada

```json
{
  "success": true,
  "data": {
    "items": [...],
    "page": 1,
    "pageSize": 10,
    "total": 50,
    "totalPages": 5,
    "hasPreviousPage": false,
    "hasNextPage": true
  }
}
```

## Estados de Pedido

- **0 (Nuevo)**: Pedido creado, puede ser editado
- **1 (Confirmado)**: Pedido confirmado, stock reservado
- **2 (Anulado)**: Pedido cancelado, stock liberado

## Reglas de Negocio

### Pedidos

1. Solo pedidos en estado "Nuevo" pueden ser editados
2. Solo pedidos en estado "Nuevo" pueden ser confirmados
3. Solo pedidos en estado "Confirmado" pueden ser anulados
4. Al confirmar un pedido se reserva automáticamente el stock
5. Al anular un pedido se libera automáticamente el stock reservado

### Stock

1. No se puede confirmar un pedido si no hay stock suficiente
2. El stock disponible = OnHand - Reserved
3. Las operaciones de stock son transaccionales

### Auditoría

1. Todas las operaciones críticas se registran en AuditLog
2. Se almacena usuario, fecha, acción y payload JSON

## CORS

La API está configurada para permitir requests desde cualquier origen para facilitar la integración con Visual FoxPro:

```csharp
policy.AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader()
      .WithExposedHeaders("X-Total-Count");
```

## Próximos Pasos

1. **Validaciones**: Implementar FluentValidation para validaciones más robustas
2. **Logging**: Agregar Serilog para logging estructurado
3. **Pruebas**: Crear pruebas unitarias e integración
4. **Documentación**: Mejorar documentación XML para Swagger
5. **Caching**: Implementar caching para consultas frecuentes
6. **Rate Limiting**: Agregar limitación de requests

## Troubleshooting

### Error de Conexión a Base de Datos

1. Verificar que SQL Server esté ejecutándose
2. Confirmar que la base de datos `OfimaPedidosERP` existe
3. Verificar la cadena de conexión en `appsettings.json`

### Error de Autenticación JWT

1. Verificar que la clave JWT tenga al menos 32 caracteres
2. Confirmar que el token no haya expirado (8 horas de validez)
3. Verificar que el header Authorization tenga el formato: `Bearer {token}`

### Error 409 en Operaciones de Pedido

- **Confirmar**: Verificar que haya stock suficiente
- **Anular**: Verificar que el pedido esté en estado "Confirmado"
- **Editar**: Verificar que el pedido esté en estado "Nuevo"

---

**Desarrollado por**: Equipo Ofima  
**Fecha**: 2025-10-21  
**Versión**: 1.0
