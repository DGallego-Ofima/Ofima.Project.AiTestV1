# Frontend Visual FoxPro — Organización Inicial

Este directorio contiene la estructura base para el cliente Visual FoxPro del módulo de pedidos ERP.

## Estructura de carpetas
- `src/ui/forms` — Formularios principales (`frmLogin`, `frmPedidos`, `frmPedidoEdit`, etc.).
- `src/ui/components` — Controles reutilizables, menús, diálogos y layouts.
- `src/services/http` — Módulos de comunicación HTTP/JSON con la API .NET.
- `src/services/integration` — Adaptadores para interoperabilidad (por ejemplo, wrappers WinHTTP, COM, DLL propias).
- `src/services/api` — Servicios organizados por dominio (`Auth`, `Customers`, `Orders`) que encapsulan las llamadas REST.
- `src/models` — Definiciones de DTOs y estructuras de datos compartidas con la API.
- `src/state` — Manejo de sesión, token, preferencias y cache local.
- `src/logs` — Salida de bitácoras (`app.log`, errores y auditorías de cliente).
- `src/utils` — Funciones auxiliares (serialización JSON, validaciones, conversores).
- `src/tests` — Escenarios de pruebas funcionales/automatizadas para formularios y servicios.
- `config` — Archivos de configuración (por ejemplo `app.config`, rutas, endpoints).
- `lib` — Dependencias externas (DLLs, OCXs, librerías de terceros).
- `scripts` — Scripts de compilación, despliegue o tareas repetitivas.
- `docs` — Documentación específica del frontend (manuales, guías de uso, decision log).

## Próximos pasos sugeridos
1. Definir contratos DTO compartidos con el backend y almacenarlos en `src/models`.
2. Crear adaptadores de comunicación HTTP en `src/services/http`, evaluando WinHTTP, MSXML2 o wrappers .NET.
3. Preparar plantillas de formularios en `src/ui/forms` según las historias de usuario.
4. Documentar en `docs/` cualquier decisión de interoperabilidad y pruebas realizadas.

Actualiza esta guía conforme avance el desarrollo.
