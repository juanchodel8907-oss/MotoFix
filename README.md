# 🏍️ MotoFix — Sistema de Gestión de Talleres de Motocicletas

**MotoFix** es una aplicación web desarrollada con ASP.NET Core MVC orientada a la gestión administrativa y operativa de talleres de motocicletas.

El sistema busca centralizar la información de clientes, motocicletas, usuarios y permisos, ofreciendo una interfaz intuitiva y un sistema de acceso basado en roles.

---

## 🚀 Tecnologías utilizadas

- **C#** — Lenguaje de programación.
- **ASP.NET Core MVC (.NET 10)** — Framework de desarrollo web.
- **Entity Framework Core 10** — ORM para el acceso y gestión de datos.
- **SQL Server LocalDB** — Sistema de gestión de bases de datos.
- **ASP.NET Core Identity** — Autenticación y gestión de usuarios.
- **Razor Views** — Construcción de interfaces dinámicas.
- **Bootstrap** — Diseño responsive.
- **HTML5 y CSS3** — Estructura y presentación visual.
- **JavaScript** — Funcionalidades del frontend.
- **Git y GitHub** — Control de versiones.

---

## ⚙️ Funcionalidades implementadas

### 1. Gestión de clientes
- Registrar clientes.
- Consultar el listado de clientes.
- Visualizar información detallada.
- Editar información.
- Eliminar clientes.
- Protección contra la eliminación de clientes que tengan motocicletas asociadas.

### 2. Gestión de motocicletas
- Registrar motocicletas.
- Consultar motocicletas registradas.
- Visualizar información técnica.
- Editar información.
- Eliminar motocicletas.
- Asociación de motocicletas con sus respectivos clientes.

### 3. Autenticación y gestión de usuarios
- Inicio y cierre de sesión.
- Registro de usuarios mediante el módulo administrativo.
- Edición de información de usuarios.
- Asignación de roles.
- Gestión del perfil personal.

### 4. Gestión de roles y permisos
- Crear roles.
- Editar roles.
- Asignar múltiples permisos a cada rol.
- Administrar permisos disponibles.
- Controlar el acceso a funcionalidades según los permisos asignados.

### 5. Control de acceso
- Autorización dinámica mediante políticas.
- Validación de permisos en el servidor.
- Menús y acciones visibles según los permisos del usuario.
- Protección frente al acceso directo a funcionalidades no autorizadas.

---

## 👥 Sistema de roles

MotoFix cuenta con tres roles principales:

| Rol | Descripción |
|---|---|
| Administrador | Acceso completo al sistema, gestión de usuarios, roles, permisos, clientes y motocicletas. |
| Supervisor | Puede consultar y editar clientes y motocicletas. |
| Empleado | Puede consultar clientes y motocicletas. |

Los permisos se administran dinámicamente, permitiendo configurar las funcionalidades disponibles para cada rol.

---

## 🗄️ Base de datos

El proyecto utiliza SQL Server LocalDB y Entity Framework Core para la persistencia de información.

Entre las principales tablas se encuentran:

- `Clientes`
- `Motocicletas`
- `AspNetUsers`
- `AspNetRoles`
- `Permisos`
- `RolPermisos`

También se utilizan las tablas auxiliares de ASP.NET Core Identity para administrar usuarios, roles, reclamaciones y autenticación.

Las migraciones de Entity Framework Core permiten mantener sincronizada la estructura de la base de datos con los modelos del proyecto.

---

## 💻 Instalación y ejecución

### Requisitos previos

- Visual Studio o Visual Studio Code.
- .NET 10 SDK.
- SQL Server LocalDB.
- Git.

### 1. Clonar el repositorio

```bash
git clone https://github.com/juanchodel8907-oss/MotoFix.git
```

### 2. Entrar al directorio

```bash
cd MotoFix
```

### 3. Restaurar dependencias

```bash
dotnet restore
```

### 4. Configurar la conexión a la base de datos

Verificar la cadena de conexión en `appsettings.json` y configurar SQL Server LocalDB:

```text
(localdb)\MSSQLLocalDB
```

### 5. Aplicar las migraciones

```bash
dotnet ef database update
```

### 6. Ejecutar la aplicación

```bash
dotnet run
```

La aplicación estará disponible en la dirección HTTPS indicada por la consola.

---

## 📁 Estructura del proyecto

```text
MotoFix/
│
├── Controllers/       # Controladores MVC
├── Data/              # Contexto de base de datos
├── Models/            # Modelos y ViewModels
├── Services/          # Servicios y lógica de negocio
├── Views/             # Vistas Razor
├── wwwroot/           # Archivos estáticos
├── Migrations/        # Migraciones de EF Core
│
├── Program.cs         # Configuración de la aplicación
├── appsettings.json   # Configuración general
└── MotoFix.csproj     # Archivo del proyecto
```

---

## 🔐 Seguridad

MotoFix implementa ASP.NET Core Identity para la autenticación y un sistema de autorización basado en permisos dinámicos.

Las restricciones de acceso se verifican en el servidor, evitando depender únicamente de la visibilidad de botones o elementos de navegación.

---

## 📌 Estado del proyecto

El proyecto cuenta actualmente con los siguientes módulos funcionales:

- Gestión de clientes.
- Gestión de motocicletas.
- Autenticación y perfil de usuario.
- Administración de usuarios.
- Administración de roles y permisos.
- Control de acceso dinámico.
- Interfaz web personalizada y responsive.

---

## 👨‍💻 Desarrollo

Proyecto académico desarrollado como aplicación web de gestión de talleres de motocicletas, utilizando arquitectura MVC y tecnologías del ecosistema .NET.

**MotoFix — Gestión inteligente para talleres de motocicletas.** 🏍️