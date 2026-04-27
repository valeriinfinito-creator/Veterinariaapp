VeterinariaApp
Sistema web de gestión veterinaria desarrollado en ASP.NET Core MVC, con arquitectura por capas, base de datos MySQL y envío automático de correos.

Descripción
VeterinariaApp permite administrar todo el flujo de una clínica veterinaria:
Registro de mascotas y propietarios
Gestión de veterinarios
Creación y control de citas médicas
Tratamientos y medicamentos
Reportes estadísticos
Notificaciones por correo electrónico

Tecnologías
ASP.NET Core MVC (.NET 8/10)
Entity Framework Core
MySQL
LINQ
Razor Views
SMTP (Email Service)
Dependency Injection

Arquitectura
Controllers
Services
Models
ViewModels
Data (DbContext)
Helpers (Email, etc)
Validators
Views (Razor)

Funcionalidades

Mascotas
Crear, editar, eliminar y listar mascotas
Relación con propietario
Historial de citas

Propietarios
CRUD completo
Validación de datos

Veterinarios
Registro con especialidad
Horarios de atención

Citas
Creación de citas médicas
Validación de conflictos de horario
Máximo 2 citas activas por mascota
Bloqueo por inasistencias
Estados:
Programada
Atendida
Cancelada
No asistió

Notificaciones
Envío automático de correo al crear cita
Plantilla HTML personalizada
Incluye:
Nombre de mascota
Veterinario
Fecha

Reportes
Veterinario con más citas
Mascotas más atendidas
Medicamentos más usados
Tasa de inasistencia

Reglas de negocio
Máximo 2 citas activas por mascota
Bloqueo con 3 inasistencias
No conflictos de horario en veterinarios
Estado automático: "Programada"

Instalación
1. Clonar proyecto
git clone https://github.com/tuusuario/VeterinariaApp.git

2. Base de datos
CREATE DATABASE VeterinariaDB;

3. Configurar conexión
"ConnectionStrings": {
  "DefaultConnection": "server=localhost;database=VeterinariaDB;user=root;password=1234;"
}

4. Migraciones
dotnet ef migrations add InitialCreate
dotnet ef database update

5. Ejecutar
dotnet run

Configuración correo
"EmailSettings": {
  "SmtpServer": "smtp.gmail.com",
  "Port": 587,
  "SenderEmail": "correo@gmail.com",
  "Password": "app_password"
}

Autor

Valeria Coy Ibarra - Cohorte 6 pm - C#

Estado

Backend completo
Base de datos funcional
Emails automáticos
Reportes implementados
Listo para producción académica


