📦 GestorMat

GestorMat es un sistema de gestión de materiales pensado para controlar stock, depósitos y movimientos de manera simple, ordenada y escalable.

El objetivo del proyecto es simular un entorno real de negocio aplicando buenas prácticas de desarrollo como Clean Architecture, separación de responsabilidades y diseño orientado a mantenibilidad.

🚀 ¿Qué permite hacer?

📊 Gestión de stock
Consultar saldos de materiales por depósito
Filtrar por material y/o depósito
Ver stock disponible y stock mínimo
Identificar rápidamente faltantes o niveles críticos

🏢 Gestión de depósitos
Alta y consulta de depósitos
Manejo de estados (habilitado / inactivo)

📦 Gestión de materiales
Registro de materiales con código y nombre
Definición de unidad de medida
Configuración de stock mínimo

🔄 Movimientos de stock
Incremento y decremento de stock
Actualización automática del saldo
Registro de fecha de última modificación

📄 Reportes en PDF
Exportación de saldos a PDF
Filtros incluidos en el reporte
Diseño tipo reporte empresarial

🔐 Autenticación y roles
Login con JWT
Manejo de sesión en frontend
Control de acceso por roles (ej: Admin)
Renderizado condicional de UI (AuthorizeView)


🧱 Arquitectura

El proyecto está basado en Clean Architecture, lo que permite:
Separar lógica de negocio de infraestructura
Facilitar testing
Escalar el sistema sin acoplamientos fuertes

Capas principales:
Domain → Entidades y reglas de negocio
Application → DTOs, interfaces, queries
Infrastructure → Base de datos, repositorios, servicios (PDF, etc.)
API → Endpoints REST
Frontend (Blazor WebAssembly) → Interfaz de usuario

🛠️ Tecnologías utilizadas

- Backend
.NET 10
ASP.NET Core Web API
Entity Framework Core
SQL Server

- Frontend
Blazor WebAssembly
Bootstrap

- Seguridad
JWT (JSON Web Tokens)
Blazored.SessionStorage

- Reportes
QuestPDF

- Buenas prácticas
Clean Architecture
SOLID
DTO pattern
Repository pattern
Inyección de dependencias


📡 API

La API permite:

Consultar saldos
Filtrar información
Exportar reportes en PDF

Ejemplo:

GET /api/saldo?idMaterial=1&idDeposito=2
GET /api/saldo/pdf?idMaterial=1