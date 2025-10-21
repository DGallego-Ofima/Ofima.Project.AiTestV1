# Ofima Orders — Proyecto Integral

## Descripción general
Este repositorio contiene una solución completa para el módulo de pedidos del ERP Ofima, compuesta por:
- **Backend (`Backend/`)**: API REST construida con ASP.NET Core 8, capa de aplicación, infraestructura EF Core y entidades de dominio.
- **Frontend (`Frontend/`)**: Cliente Visual FoxPro encargado del login y futuros flujos operativos de pedidos.
- **Database (`Database/`)**: Scripts y utilidades SQL para inicializar la base de datos (`OfimaPedidosERP`).
- **Docs (`Docs/`)**: Guías, prompts y recursos de apoyo.

La API expone endpoints autenticados con JWT, mientras que el cliente VFP consume dichos servicios mediante WinHTTP.

## Backend
- Proyecto principal: `Backend/Ofima.Orders.API/`
- Tecnologías: ASP.NET Core 8, Entity Framework Core, SQL Server, JWT.
- Capas:
  - `Ofima.Orders.API`: Exposición de endpoints (`/api/v1/...`).
  - `Ofima.Orders.Application`: Servicios, DTOs y contratos (`IAuthService`, `LoginRequest`, etc.).
  - `Ofima.Orders.Infrastructure`: Repositorios, `UnitOfWork`, contexto EF Core.
  - `Ofima.Orders.Domain`: Entidades (`User`, `Order`, etc.).

### Configuración
1. Variables clave en `Backend/Ofima.Orders.API/appsettings.json`:
   - `ConnectionStrings:DefaultConnection`
   - `Jwt:Key` (32+ caracteres)
2. Restaurar paquetes y ejecutar:
   ```bash
   cd Backend/Ofima.Orders.API
   dotnet restore
   dotnet ef database update   # si existen migraciones
   dotnet run
   ```
3. La API quedará disponible en:
   - `https://localhost:59500`
   - `http://localhost:59502`
   - Swagger: `https://localhost:59500/swagger/index.html`

> **Nota**: Para consumir HTTPS desde Visual FoxPro, confía el certificado de desarrollo con `dotnet dev-certs https --trust`.

## Frontend (Visual FoxPro)
- Ubicación: `Frontend/`
- Archivos relevantes:
  - `frmlogin.prg`: Formulario de autenticación.
  - `ApiServiceLoader.prg`: Registra las clases de servicio.
  - `HttpClient.prg`, `BaseApiService.prg`, `AuthApiService.prg`, `CustomersApiService.prg`, `OrdersApiService.prg`.
- El flujo de login:
  1. `frmLogin` carga servicios con `LoadApiServices`.
  2. Construye JSON y envía `POST /api/v1/auth/login`.
  3. Al recibir token, lo guarda en `_SCREEN.cAuthToken` y configura el `HttpClient`.

### Ejecución en VFP
1. Abrir Visual FoxPro apuntando a la carpeta `Frontend/`.
2. Ejecutar:
   ```foxpro
   DO frmlogin.prg
   ```
3. Ingresar credenciales válidas (por ejemplo `admin` / `123456`).

> Si ves “Dirección URL no válida”, revisa que `cApiBaseUrl` en `frmlogin.prg` apunte al host correcto y que el certificado HTTPS esté confiado.

## Base de datos
- Scripts en `Database/` para crear el esquema `sec.Users` y demás tablas.
- La conexión por defecto usa `DESKTOP-MSGM53U\SQLEXPRESS`; ajusta según tu entorno.

## Pruebas manuales recomendadas
- **API**: Desde Swagger o `curl`:
  ```bash
  curl -k -X POST "https://localhost:59500/api/v1/auth/login" \
    -H "Content-Type: application/json" \
    -d '{"username":"admin","password":"123456"}'
  ```
- **Cliente VFP**: Verificar mensajes de éxito/error del formulario y revisar `_SCREEN.cAuthToken` tras autenticación.

## Notas adicionales
- Los PRG del front se ubican en la misma carpeta para simplificar `SET PROCEDURE TO`.
- Se configuró `HttpClient.prg` para construir URLs con `/` y agregar encabezado `Content-Type` con `charset=utf-8`.
- El proyecto usa `WinHttp.WinHttpRequest` para las peticiones; asegúrate de tener permisos para COM.

## Próximos pasos sugeridos
- Implementar formularios de gestión de pedidos y clientes reutilizando `CustomersApiService.prg` y `OrdersApiService.prg`.
- Añadir almacenamiento seguro de tokens (por ejemplo en `src/state/`).
- Crear pruebas automatizadas para los servicios del front (`Frontend/src/tests/`).
