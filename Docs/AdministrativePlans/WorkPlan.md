# Plan de trabajo — Reto Gestion de Pedidos ERP

## 1. Proposito y contexto
- Documento fuente: `Docs/INS-ERP-IA-01.pdf`.
- Objetivo: modernizar el modulo de Pedidos del ERP asegurando flujo O2C consistente, integracion con Visual FoxPro y control de inventario en SQL Server.
- Alcance temporal estimado: iteracion intensiva de 8 horas efectivas, con posibilidad de fraccionar en bloques de trabajo.
- Criterios de exito: historias de usuario cumplidas, interoperabilidad VFP ↔ .NET, integridad de datos, uso documentado de IA y entrega del repositorio estructurado.

## 2. Alcance
- Backend: API .NET 8 (Clean Architecture) con autenticacion JWT, control transaccional de stock, auditoria y documentacion Swagger.
- Frontend: cliente Visual FoxPro con pantallas de login, gestion de pedidos, confirmacion/anulacion y paginacion.
- Base de datos: modelo relacional SQL Server con esquemas `sec` y `erp`, seeds reproducibles y restricciones de integridad.
- DevOps liviano: scripts de despliegue (init-db, build VFP opcional), README y prompts IA.
- Excluido: facturacion, reporteria avanzada, integracion con sistemas externos distintos a VFP.

## 3. Objetivos especificos
1. Digitalizar el registro completo de pedidos con estados Nuevo, Confirmado, Anulado.
2. Garantizar control atomico de stock y auditoria de operaciones.
3. Entregar API REST consumible por clientes futuros.
4. Validar interoperabilidad con Visual FoxPro via JSON.
5. Documentar uso de herramientas de IA en `prompts.md`.

## 4. Entregables clave
| Entregable | Contenido resumido | Responsable sugerido | Hito |
|------------|--------------------|----------------------|------|
| Arquitectura funcional | Diagramas logicos, definicion de capas, contratos JSON | Arquitecto / Tech Lead | Fin Fase 2 |
| Proyecto backend | Solucion .NET con controladores, servicios, pruebas, Swagger | Equipo backend | Fin Fase 3 |
| Scripts base datos | `schema.sql`, `seeds.sql`, `init-db.ps1` | DBA / Backend | Fin Fase 3 |
| Cliente VFP | Formularios, servicios HTTP, validaciones | Equipo frontend VFP | Fin Fase 4 |
| Pruebas y evidencias | Suite automatizada, checklists funcionales | QA | Fin Fase 5 |
| Documentacion final | README, prompts.md, guias de ejecucion | PM / Todos | Fin Fase 6 |

## 5. Organizacion y roles
- **Product Owner / Sponsor**: prioriza historias y valida entregables.
- **Arquitecto / Tech Lead**: define lineamientos de Clean Architecture, contratos API y estrategia de integración.
- **Equipo backend .NET**: implementa API, reglas de negocio y pruebas.
- **Equipo frontend VFP**: construye interfaces, servicios HTTP y experiencia de usuario.
- **DBA / Ingeniero de datos**: diseña modelo relacional, seeds y tuning.
- **QA / Tester**: diseña y ejecuta pruebas funcionales e integracion.
- **Scrum Master / PM**: coordina avances, riesgos y comunicacion.

## 6. Plan por fases
### Fase 1. Inicio y alineacion (h 0.5)
- Confirmar objetivos, restricciones y accesos.
- Revisar historias de usuario, criterios de aceptacion y rubrica.
- Definir herramientas, convenciones Git y canales de comunicacion.
- Salidas: acta breve de inicio, tablero de tareas, agenda de checkpoints.

### Fase 2. Analisis y diseno (h 1.5)
- Elaborar modelo de dominio y diagramas de componentes.
- Definir contratos DTO/JSON compartidos (autenticacion, pedidos, errores).
- Especificar reglas de negocio detalladas (stock, estados, idempotencia).
- Actualizar este plan con decisiones y riesgos detectados.
- Salidas: artefactos de arquitectura, backlog priorizado, decision log.

### Fase 3. Plataforma backend y base de datos (h 2.5)
- Generar migraciones y scripts `schema.sql` + `seeds.sql`.
- Implementar nucleo de API: autenticacion, endpoints de clientes y pedidos.
- Incorporar servicios de dominio con transacciones, auditoria y validaciones.
- Crear pruebas unitarias de confirmacion/anulacion y pruebas de integracion basicas.
- Salidas: API funcional cubierta por pruebas, scripts reproducibles, Swagger operativo.

### Fase 4. Cliente Visual FoxPro (h 2.0)
- Preparar estructura `/ui`, `/services`, `/models`, `/state`, `/logs`.
- Desarrollar formularios (login, grid pedidos, formulario detalle).
- Implementar servicios HTTP con manejo de errores, retries y token.
- Validar escenarios clave: creacion, confirmacion, anulacion, paginacion.
- Salidas: ejecutable o fuentes VFP, configuracion `app.config`, instrucciones de despliegue.

### Fase 5. Integracion, pruebas y ajustes (h 1.0)
- Ejecutar pruebas cruzadas VFP ↔ API, incluyendo casos de error (401, 409).
- Verificar idempotencia de `/confirm` y `/cancel`, y consistencia de auditoria.
- Afinar rendimiento (indices, logging) y observabilidad.
- Salidas: reporte de pruebas, issues resueltos, checklist de rubrica.

### Fase 6. Cierre y entrega (h 0.5)
- Completar README con pasos de instalacion, credenciales demo y flujos.
- Consolidar `prompts.md` con evidencias de IA.
- Revisar cumplimiento de estructura exigida y preparar demo final.
- Salidas: repositorio validado, release candidate, agenda de presentacion.

## 7. Gestion de riesgos
| Riesgo | Prob. | Impacto | Mitigacion |
|--------|-------|---------|------------|
| Falta de interoperabilidad VFP ↔ API | Media | Alta | Prototipo temprano de llamadas HTTP, pruebas con datos reales. |
| Retrasos por curva de aprendizaje VFP | Alta | Media | Asignar desarrollador con experiencia previa, documentar snippets reutilizables. |
| Inconsistencias de stock por concurrencia | Media | Alta | Usar transacciones y RowVersion, pruebas de carga basicas. |
| Dependencia de servicios externos (JWT, DB) | Baja | Media | Mocking en desarrollo, scripts de aprovisionamiento local. |
| Uso insuficiente de IA documentada | Media | Media | Registrar prompts desde Fase 1, revisar al cierre de cada fase. |

## 8. Plan de comunicacion y seguimiento
- Reuniones rapidas: al inicio de cada bloque de fase, 10 minutos.
- Checkpoint formal: cierre de Fases 2, 3, 4 y 5 con demo corta.
- Tablero Kanban: columnas Backlog, En progreso, Bloqueado, Hecho.
- Reporte final: resumen de historias entregadas, cobertura de pruebas, decisiones tecnicas clave.

## 9. Estrategia de calidad y pruebas
- Unit tests: servicios de dominio (confirmar/anular, reglas de stock).
- Integration tests: flujos API REST con repositorio en memoria o DB local.
- Pruebas funcionales manuales: escenarios VFP listados en el documento base.
- Observabilidad: logs estructurados con traceId, integracion con `app.log` en cliente.
- Criterios de aceptacion: matrice de historias vs evidencias.

## 10. Gestion de IA
- Registro continuo en `prompts.md`: fecha, objetivo, prompt, resultado, decision tomada.
- Revisiones de calidad de respuestas IA antes de aplicar cambios.
- Uso sugerido: generacion de esqueletos de controladores, validaciones, plantillas VFP, scripts SQL.

## 11. Cronograma orientativo
| Fase | Duracion | Ventana sugerida |
|------|----------|------------------|
| 1. Inicio | 0.5 h | Hora 0 a 0.5 |
| 2. Analisis | 1.5 h | 0.5 a 2.0 |
| 3. Backend y DB | 2.5 h | 2.0 a 4.5 |
| 4. VFP | 2.0 h | 4.5 a 6.5 |
| 5. Integracion | 1.0 h | 6.5 a 7.5 |
| 6. Cierre | 0.5 h | 7.5 a 8.0 |

## 12. Siguientes pasos inmediatos
1. Validar este plan con el equipo y aprobar ajustes de alcance.
2. Configurar repositorio y ramas principales (main, develop, feature).
3. Preparar ambiente de desarrollo (SQL Server local, API template, VFP runtime).
4. Agendar checkpoints de fin de fase y definir responsables por entregable.

---

Revisar y actualizar este plan al finalizar cada fase o ante cambios significativos en alcance, restricciones o prioridades.
