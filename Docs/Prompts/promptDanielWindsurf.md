# Cascade Chat Conversation

  Note: _This is purely the output of the chat conversation and does not contain any raw data, codebase snippets, etc. used to generate the output._

### User Input

@[c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Docs\ChatsIA\CodexNativo.jsonl] Estaba teniendo esta charla con Codex en otro chat pero ya no tengo tokens en ese espacio ahora necesito que me ayudes a continuar en este espacio contigo nuevamente pero en este char y continuar en el ultimo chat que ibamos

que decia así:
"Todo funciono muy bien ahora crea la clase que consuma APIs, y separa las clases que cunsuman cada grupo de apis para solo llamar las que necesitemos, seria como el reflejo de los controladores en el proyecto frontend"

Puedes hacerlo para que continuemos o comenzamos desde 0?

*Listed directory [AiTestV1](file:///c:/Dev/AITest/AiTestV1) *

*Updated todo list*

*Listed directory [Ofima.Project.AiTestV1](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1) *

*Listed directory [Backend](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend) *

*Listed directory [Ofima.Orders.Application](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.Application) *

*Found code context*

*Listed directory [src](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src) *

*Updated todo list*

*Viewed [BaseApiService.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/BaseApiService.prg) *

*Listed directory [Services](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.Application/Services) *

*Viewed [CodexNativo.jsonl](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Docs/ChatsIA/CodexNativo.jsonl) *

*Listed directory [Ofima.Orders.API](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API) *

*Updated todo list*

### Planner Response

# Findings
- **Servicios existentes**: En `Frontend/src/services/api/` ya están [BaseApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/BaseApiService.prg:0:0-0:0), `AuthApiService.prg`, `CustomersApiService.prg`, `OrdersApiService.prg` y `ApiServiceLoader.prg`, que organizan el consumo de la API siguiendo la idea de “reflejar” los controladores del frontend.
- **HttpClient**: El acceso HTTP común está centralizado en `Frontend/src/services/http/HttpClient.prg`.

# Clarify
- **¿Continuamos sobre esta base?** ¿Quieres que extendamos esta estructura (por ejemplo, añadir más dominios o refactorizar) o prefieres que empecemos desde cero y rediseñemos las clases consumidoras? Indícame para avanzar en la implementación.

### User Input

No se como continuar solo creamos un formulario Login y ahora debemos consumir las Apis de Login para validar la entrada a otro formulario puedes hacerlo así como lo hacias en el chat anterior?

*Updated todo list*

*Viewed [frmLogin.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms/frmLogin.prg) *

*Found code context*

*Listed directory [forms](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms) *

*Found code context*

*Found code context*

*Found code context*

*Searched filesystem*

*Updated todo list*

*Found code context*

*Found code context*

*Viewed [AuthController.cs](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API/Controllers/AuthController.cs) *

*Listed directory [src](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src) *

*Found code context*

*Found code context*

*Found code context*

*Viewed [LoginResponse.cs](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.Application/DTOs/Auth/LoginResponse.cs) *

*Found code context*

*Found code context*

*Viewed [ApiResponse.cs](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.Application/DTOs/Common/ApiResponse.cs) *

*Found code context*

*Found code context*

*Updated todo list*

*Listed directory [state](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/state) *

*Found code context*

*Updated todo list*

*Viewed [README.md](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/README.md) *

*Edited relevant file*

### Planner Response

# Blocker
- **File lock**: No pude guardar cambios en [Frontend/src/forms/frmLogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms/frmLogin.prg:0:0-0:0) porque otra aplicación/instancia lo mantiene abierto. El sistema devolvió “The process cannot access the file because it is being used by another process.”

# Recommended Actions
- **Cerrar archivo**: Cierra [frmLogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms/frmLogin.prg:0:0-0:0) (y cualquier vista previa) en procesos externos/IDE que lo estén utilizando para liberar el bloqueo.

# Next Steps
- **Reintentar actualización**: Una vez liberado el archivo, avísame para intentar de nuevo la integración del formulario con `AuthApiService` (incluye carga de servicios, llamada [Login()](cci:1://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API/Controllers/AuthController.cs:18:4-48:5) y manejo del token/respuesta).

### User Input

Ya lo libere porfavor continua con el proceso

*Updated todo list*

*Edited relevant file*

*Updated todo list*

*Updated todo list*

*Updated todo list*

### Planner Response

# Summary
- **Integración Login**: [Frontend/src/forms/frmLogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms/frmLogin.prg:0:0-0:0) ahora inicializa `AuthApiService`, valida credenciales vía [Login()](cci:1://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API/Controllers/AuthController.cs:18:4-48:5), maneja tokens y estados de autenticación.
- **Persistencia de sesión**: Guarda `cAuthToken`, `cApiBaseUrl` y `cAuthUser` en `_SCREEN` tras respuesta válida; muestra mensajes claros en éxito/error.
- **Utilidades VFP**: Incluye helpers `EnsureServices`, `ProcessLoginResponse`, `BuildErrorMessage` y `ExtractJsonString` para cargar clases, interpretar JSON y formatear mensajes.

# Testing
- No se ejecutaron pruebas automáticas. Ejecuta manualmente el formulario y prueba credenciales válidas/ inválidas contra la API en `http://localhost:5000`.

# Recommended Actions
- **Configurar**: Ajusta `cApiBaseUrl` en `frmLogin` si el backend corre en otro host/puerto.
- **Validar**: Corrobora que `ApiServiceLoader.prg` esté disponible y que la API devuelva JSON con campos `token` y `username`.
- **Extender**: Al avanzar al siguiente formulario, usa `_SCREEN.cAuthToken` y `_SCREEN.cApiBaseUrl` para llamadas autenticadas.

### User Input

En esta url estan las API https://localhost:59500/swagger/index.html configura las clases para que consuman de esta url

*Updated todo list*

*Found code context*

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Base URL actualizado**: `cApiBaseUrl` en [Frontend/src/forms/frmLogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/forms/frmLogin.prg:0:0-0:0) ahora apunta a `https://localhost:59500`, alineado con la URL de Swagger indicada.
- **Servicios reutilizarán URL**: `EnsureServices()` continúa pasando la nueva base al `AuthApiService`, garantizando que el resto de clientes hereden este host.

# Testing
- No se realizaron tests automáticos. Ejecuta manualmente `frmLogin` con la API corriendo en `https://localhost:59500`.

# Recommended Actions
- **Verificar API**: Asegúrate de que el backend esté disponible en `https://localhost:59500` (ejecuta `dotnet run` en [Ofima.Orders.API](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API:0:0-0:0) si es necesario).
- **Extender configuración**: Si otros servicios necesitan la misma URL, considera moverla a un archivo de configuración compartido para evitar duplicaciones.

*Viewed [ApiServiceLoader.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/ApiServiceLoader.prg) *

### User Input

@[c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\api\apiserviceloader.prg] No fue capaz de iniciar los set procedure solto error "File "httpClient" no found"

*Updated todo list*

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Ruta corregida**: `LoadApiServices()` en [Frontend/src/services/api/ApiServiceLoader.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/ApiServiceLoader.prg:0:0-0:0) ahora acepta un directorio raíz opcional y genera rutas absolutas confiables usando `FULLPATH`.  
- **Dependencias cargadas**: Se ajustaron los `SET PROCEDURE TO` para apuntar explícitamente a `http\HttpClient.prg`, [api\BaseApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/BaseApiService.prg:0:0-0:0), `api\AuthApiService.prg`, `api\CustomersApiService.prg` y `api\OrdersApiService.prg`.

# Testing
- Sin pruebas automáticas. Ejecuta `DO LoadApiServices` desde el formulario (`EnsureServices()`) para verificar que ya no aparezca “File 'HttpClient' not found”.

# Recommended Actions
- **Llamada manual**: Si cargas servicios desde otra ruta, pasa el directorio base explícito: `DO LoadApiServices WITH FULLPATH("Frontend\\src\\services\\")`.

*Viewed [ApiServiceLoader.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services/api/ApiServiceLoader.prg) *

### User Input

Sigue sin encontrarlo...

Deja todos los archivos en una misma carpeta para evitar este problema no pasa nada

*Updated todo list*

*Listed directory [services](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/src/services) *

*User accepted the command `move "c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\http\HttpClient.prg" "c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\api\HttpClient.prg"`*

*Checked command status*

*User accepted the command `move "Frontend\src\services\http\HttpClient.prg" "Frontend\src\services\api\HttpClient.prg"`*

*User accepted the command `Move-Item -Path "Frontend\src\services\http\HttpClient.prg" -Destination "Frontend\src\services\api\HttpClient.prg"`*

*User accepted the command `PowerShell -NoProfile -Command "Move-Item -LiteralPath 'Frontend\src\services\http\HttpClient.prg' -Destination 'Frontend\src\services\api\HttpClient.prg'"`*

*User accepted the command `powershell -NoLogo -NoProfile -Command "Move-Item -LiteralPath 'Frontend\src\services\http\HttpClient.prg' -Destination 'Frontend\src\services\api\HttpClient.prg'"`*

*User accepted the command `powershell.exe -NoLogo -NoProfile -Command "Move-Item -LiteralPath 'c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\http\HttpClient.prg' -Destination 'c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\api\HttpClient.prg'"`*

*User accepted the command `Move-Item -LiteralPath "c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\http\HttpClient.prg" -Destination "c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\src\services\api\HttpClient.prg"`*

*Viewed [HttpClient.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg) *

*Edited relevant file*

*Edited relevant file*

### User Input

Ya los coloque todo en una misma carpeta @[c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend] ahora organiza las clases para que trabajen con esto

*Listed directory [Frontend](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend) *

*Updated todo list*

*Viewed [apiserviceloader.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/apiserviceloader.prg) *

*Viewed [frmlogin.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/frmlogin.prg) *

*Viewed [BaseApiService.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/BaseApiService.prg) *

*Edited relevant file*

*Viewed [AuthApiService.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/AuthApiService.prg) *

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Servicios planos**: Mover todos los PRG de servicios al directorio [Frontend/](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend:0:0-0:0) simplificó la carga.
- **`LoadApiServices()`** ([Frontend/ApiServiceLoader.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/ApiServiceLoader.prg:0:0-0:0)) ahora:
  - Acepta un directorio (por defecto el actual con `SYS(16,0)`).
  - Hace `SET PROCEDURE TO` sobre [HttpClient.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg:0:0-0:0), [BaseApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/BaseApiService.prg:0:0-0:0), [AuthApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/AuthApiService.prg:0:0-0:0), [CustomersApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/CustomersApiService.prg:0:0-0:0) y [OrdersApiService.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/OrdersApiService.prg:0:0-0:0) ubicados en la carpeta plana.
- **`frmLogin`** ([Frontend/frmlogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/frmlogin.prg:0:0-0:0)) ajustado:
  - Calcula `lcDir` del propio PRG.
  - Ejecuta `DO LoadApiServices WITH lcDir` para registrar dependencias antes de crear `AuthApiService`.

# Testing
- No pruebas automáticas; ejecutar `DO frmlogin` manualmente y validar que el formulario abra y consulte la API en `https://localhost:59500`.

# Recommended Actions
- **Verificar rutas**: Confirma que todos los PRG necesarios estén realmente en [c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Frontend\](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend:0:0-0:0).
- **Probar autenticación**: Con el backend activo, intenta iniciar sesión para asegurar que las rutas cargadas reconocen `HttpClient` y consumen correctamente la API.
- **Considerar configuración**: A futuro, centraliza la URL base en un archivo de configuración para facilitar cambios de entorno.

*Edited relevant file*

*Edited relevant file*

*Viewed [apiserviceloader.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/apiserviceloader.prg) *

### User Input

Listo corregido el problema, ahora tengo otro problema entregado por el login

no es de codigo si no del proceso normal

*Updated todo list*

*Viewed [HttpClient.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg) *

*Updated todo list*

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Causa**: WinHTTP rechazaba la URL porque `HttpClient.BuildUrl()` concatenaba rutas con `addbs()`, produciendo barras invertidas (`\`) para esquemas HTTPS.
- **Solución**: Ajusté [Frontend/HttpClient.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg:0:0-0:0) para normalizar la base (`lcBase`), quitar barras invertidas finales y garantizar una única barra `/` antes del endpoint. Con `cApiBaseUrl = "https://localhost:59500"` y endpoint `"/api/v1/auth/login"`, la URL resultante ahora es `https://localhost:59500/api/v1/auth/login`.

# Testing
- No pruebas automáticas. Ejecuta `DO frmlogin` y prueba el inicio de sesión con la API disponible para confirmar que el error de “Dirección URL no válida” desaparece.

# Recommended Actions
- **Verificar API HTTPS**: Asegúrate de que `https://localhost:59500` esté activo y con certificado confiable (importa el certificado de desarrollo si WinHTTP lo requiere).
- **Seguir flujo**: Tras autenticación exitosa, reutiliza `_SCREEN.cAuthToken` para llamadas posteriores.

### User Input

El proyecto esta en linea

info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:59500
info: Microsoft.Hosting.Lifetime[14]
      Now listening on: http://localhost:59502
info: Microsoft.Hosting.Lifetime[0]
      Application started. Press Ctrl+C to shut down.
info: Microsoft.Hosting.Lifetime[0]
      Hosting environment: Development
info: Microsoft.Hosting.Lifetime[0]
      Content root path: C:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\Backend\Ofima.Orders.API


Esto es lo que suelta la consola si no te deja ingresar revisa si pudes cambiar algo para que funcione

*Found code context*

### Planner Response

# Causa probable
- **Certificado HTTPS no confiable**: `WinHttp.WinHttpRequest` (usado en [Frontend/HttpClient.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg:0:0-0:0)) valida el certificado del host. Las plantillas de ASP.NET sirven `https://localhost:59500` con un certificado de desarrollo que solo queda confiado tras ejecutar `dotnet dev-certs https --trust`. Si WinHTTP no confía en ese certificado, lanza errores de conexión aun cuando el navegador funcione.

# Recommended Actions
- **Confiar certificado**  
  Ejecuta en PowerShell (modo usuario) dentro de [Ofima.Project.AiTestV1/Backend/Ofima.Orders.API](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API:0:0-0:0):
  ```powershell
  dotnet dev-certs https --trust
  ```
  Acepta el diálogo para instalar el certificado en el almacén de “Entidades de certificación raíz de confianza”.

- **Reiniciar WinHTTP si es necesario**  
  Si la primera ejecución no basta, reinicia el backend (`dotnet run`) y vuelve a lanzar el formulario [frmlogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/frmlogin.prg:0:0-0:0).

- **Probar alternativa temporal**  
  Como verificación, cambia momentáneamente `cApiBaseUrl` en [Frontend/frmlogin.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/frmlogin.prg:0:0-0:0) a `http://localhost:59502` (HTTP sin TLS). Si ahí funciona, confirma que la causa era la confianza del certificado y vuelve a HTTPS tras confiarlo.

- **Validar con curl/Postman**  
  Envía un `POST https://localhost:59500/api/v1/auth/login` para verificar que la API responde 200 con token; así descartas problemas de credenciales o datos.

# Estado actual
- Backend ([Ofima.Orders.API](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Backend/Ofima.Orders.API:0:0-0:0)) está escuchando en `https://localhost:59500` y `http://localhost:59502`.
- Frontend ya arma correctamente la URL (`HttpClient.BuildUrl()` normaliza `/`). Falta que Windows confíe en el certificado o que utilices el puerto HTTP mientras pruebas.

### User Input

Los Try Cath en fox no pueden tener return

*Updated todo list*

*Viewed [HttpClient.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg) *

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Retornos en TRY/CATCH**: [Frontend/HttpClient.prg](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/HttpClient.prg:0:0-0:0) ya no usa `RETURN` dentro del bloque `TRY`. La respuesta se construye con variables `llSuccess`, `lnStatus` y `lcResponse` y se devuelve una vez finalizado `EndTry`, cumpliendo las reglas de VFP.
- **URL normalizada**: Se mantiene el ajuste previo en `BuildUrl()` para evitar barras invertidas al consumir `https://localhost:59500`.

# Recommended Actions
- **Reintentar login**: Ejecuta `DO frmlogin` y prueba credenciales. Si aún falla por certificado, aplica la recomendación anterior (`dotnet dev-certs https --trust` o usa `http://localhost:59502` temporalmente).

### User Input



me devuelve esto pero las credenciales en el swagger sirven

{
  "username": "admin",
  "password": "123456"
}

Pero desde el form no sirven

*Found code context*

*Updated todo list*

*Edited relevant file*

*Found code context*

*Updated todo list*

*Found code context*

*Found code context*

*Edited relevant file*

*Found code context*

### User Input

continue

*Viewed [frmlogin.prg](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/Frontend/frmlogin.prg) *

### User Input

@[c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1] En la carpeta que continiene todo crea un README que explique todo lo del proyecto

*Updated todo list*

*Listed directory [Ofima.Project.AiTestV1](file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1) *

*Edited relevant file*

*Updated todo list*

### Planner Response

# Summary
- **Readme creado**: Documenté el proyecto completo en [README.md](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1/README.md:0:0-0:0) en [c:\Dev\AITest\AiTestV1\Ofima.Project.AiTestV1\](cci:7://file:///c:/Dev/AITest/AiTestV1/Ofima.Project.AiTestV1:0:0-0:0).  
  Incluye descripción general, detalles de backend (`Ofima.Orders.API