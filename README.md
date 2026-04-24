# 📦 GestorMat - Sistema de Gestión de Materiales

## ¿Qué es GestorMat?

**GestorMat** es una aplicación web moderna para gestionar el inventario de materiales en una organización. Permite realizar un seguimiento completo de productos, movimientos de stock, depósitos y usuarios, con autenticación segura y reportes de importación/exportación.

## 🎯 Características Principales

### ✅ Gestión de Materiales
- Crear, editar, visualizar y eliminar materiales
- Definir precios, unidades de medida y stock mínimo
- Importar materiales desde archivos Excel y XML
- Control de activación/desactivación de productos

### 📦 Movimientos de Inventario
- Registrar ingresos, egresos y traslados entre depósitos
- Seguimiento automático de saldos por material y depósito
- Manejo transaccional con rollback en caso de error
- Historial completo de operaciones

### 👥 Gestión de Usuarios
- Crear y gestionar usuarios con roles
- Roles con permisos diferenciados
- Autenticación segura con JWT
- Contraseñas hasheadas

### 📂 Gestión de Depósitos
- Crear y mantener múltiples depósitos
- Localización e información de contacto
- Control de activación/desactivación

### 📤 Importación y Exportación
- Importar materiales desde Excel (.xlsx)
- Importar datos desde XML
- Validación automática de datos
- Reportes de errores en importación

## 🛠️ Tecnología Utilizada

| Aspecto | Tecnología |
|--------|-----------|
| **Framework .NET** | .NET 10 |
| **Frontend** | Blazor WebAssembly |
| **Estilos** | Bootstrap 5 |
| **Backend** | ASP.NET Core |
| **Base de Datos** | SQL Server (Entity Framework) |
| **Autenticación** | JWT (JSON Web Tokens) |
| **Mapeo de Objetos** | Mapster |
| **Procesamiento Excel** | ClosedXML |
| **Procesamiento XML** | System.Xml.Linq |
| **Testing** | xUnit + Moq |

## 📋 Estructura del Proyecto

```
GestorMat/
├── GestorMat.Domain/           # Entidades de dominio
│   └── Entidades/              # Clases del modelo de datos
├── GestorMat.Application/       # Lógica de negocio
│   ├── Servicios/              # Servicios de aplicación
│   ├── DTOs/                   # Data Transfer Objects
│   ├── Interfaces/             # Contratos de repositorios
│   └── Configuracion/          # Configuración de Mapster
├── GestorMat.Infrastructure/    # Persistencia y repositorios
│   ├── Contextos/              # DbContext de Entity Framework
│   └── Repositorios/           # Implementaciones de repositorios
├── GestorMat.API/              # Endpoints REST
│   └── Controllers/            # Controladores de API
├── GestorMat.Frontend/         # Interfaz web
│   └── Pages/                  # Componentes Blazor Razor
└── GestorMat.Tests/            # Pruebas unitarias
    └── GestorMat.ApplicationTest/
```

## 🚀 Cómo Ejecutar

### Prerequisitos
- .NET 10 SDK instalado
- SQL Server (local o remoto)
- Visual Studio 2026 o Visual Studio Code

### Pasos de Instalación

1. **Clonar el repositorio**
   ```bash
   git clone https://github.com/bck-developer/gestor-mat.git
   cd gestor-mat
   ```

2. **Configurar la base de datos**
   - Actualizar la cadena de conexión en `appsettings.json`
   - Ejecutar migraciones de Entity Framework:
     ```bash
     dotnet ef database update --project GestorMat.Infrastructure
     ```

3. **Instalar dependencias**
   ```bash
   dotnet restore
   ```

4. **Ejecutar la aplicación**
   ```bash
   dotnet run --project GestorMat.API
   ```
   La aplicación se abrirá en `https://localhost:5001`

## 💡 Beneficios de Mapster

Esta aplicación utiliza **Mapster** para mapear entre entidades de dominio y DTOs. Los principales beneficios son:

### 🚀 **Rendimiento Superior**
- Mapster utiliza árboles de expresiones compilados, lo que resulta en mapeos muy rápidos
- Mejor rendimiento que AutoMapper en aplicaciones de alto volumen
- Sin reflexión en tiempo de ejecución después de la compilación

### 📝 **Menos Código Boilerplate**
- Eliminación de mapeos manuales con `Select()` y constructores de objetos
- Reducción de ~15-20 líneas de código por servicio
- Mapeos automáticos cuando los nombres de propiedades coinciden

### 🔒 **Type-Safe**
- Verificación en tiempo de compilación
- Refactorización segura
- Detección automática de errores de mapeo

### 🧹 **Código Más Limpio y Mantenible**
- Servicios más legibles y enfocados en lógica de negocio
- Configuración centralizada de mapeos en `Configuracion/MappingConfig.cs`
- Fácil de testear

### 🛠️ **Funcionalidades Avanzadas**
- Mapeo automático de propiedades relacionadas (nested mapping)
- Manejo automático de valores nulos
- Transformaciones personalizadas cuando es necesario
- Mapeos bidireccionales


## 🧪 Testing

La aplicación incluye 88+ pruebas unitarias con cobertura >80% en la capa de aplicación.

```bash
# Ejecutar todas las pruebas
dotnet test GestorMat.Tests

# Ejecutar pruebas con cobertura
dotnet test GestorMat.Tests /p:CollectCoverage=true
```

## 📚 Servicios Principales

| Servicio | Responsabilidad |
|----------|-----------------|
| **MaterialService** | Gestión de productos |
| **UsuarioService** | Gestión de usuarios y autenticación |
| **MovimientoMaterialService** | Operaciones de inventario |
| **ExcelService** | Importación/exportación Excel |
| **XmlService** | Procesamiento de XML |
| **AuthService** | Autenticación y tokens JWT |
| **UnidadMedidaService** | Gestión de unidades |
| **DepositoService** | Gestión de ubicaciones |

## 🔐 Seguridad

- ✅ Autenticación basada en JWT
- ✅ Contraseñas hasheadas (nunca en texto plano)
- ✅ Roles y permisos por usuario
- ✅ Validación de entrada en todos los endpoints

## 📖 Arquitectura

GestorMat sigue una **arquitectura en capas**:

1. **Domain** - Reglas de negocio puras
2. **Application** - Lógica de casos de uso
3. **Infrastructure** - Persistencia y servicios externos
4. **API** - Endpoints REST
5. **Frontend** - Interfaz de usuario Blazor

## 📝 Licencia

Este proyecto está bajo licencia MIT.

## 📞 Contacto

Para preguntas o sugerencias, contacta con el equipo de desarrollo en:
- Issues: [GitHub Issues](https://github.com/bck-developer/gestor-mat/issues)

---

**GestorMat** - Gestión de materiales simplificada ✨