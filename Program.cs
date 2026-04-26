using Microsoft.EntityFrameworkCore;
using VeterinariaApp.Data;
using VeterinariaApp.Services.Interfaces;
using VeterinariaApp.Services.Implementations;
using VeterinariaApp.Helpers; // 👈 IMPORTANTE

var builder = WebApplication.CreateBuilder(args);

// 🔹 1. Conexión a MySQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<MySqlDBContext>(options =>
    options.UseMySql(
        connectionString,
        ServerVersion.AutoDetect(connectionString)
    )
);

// 🔹 2. CONFIGURACIÓN DE EMAIL (🔥 NUEVO)
builder.Services.Configure<EmailSettings>(
    builder.Configuration.GetSection("EmailSettings")
);

builder.Services.AddScoped<EmailHelper>();

// 🔹 3. Inyección de dependencias (Services)
builder.Services.AddScoped<IPropietarioService, PropietarioService>();
builder.Services.AddScoped<ICitaService, CitaService>();
builder.Services.AddScoped<IVeterinarioService, VeterinarioService>();
builder.Services.AddScoped<IReporteService, ReporteService>();

// 🔹 (cuando avances puedes agregar más)
builder.Services.AddScoped<IMascotaService, MascotaService>();
builder.Services.AddScoped<ITratamientoService, TratamientoService>();
builder.Services.AddScoped<IMedicamentoService, MedicamentoService>();

// 🔹 4. MVC
builder.Services.AddControllersWithViews();

// 🔹 5. Build
var app = builder.Build();

// 🔹 6. Manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// 🔹 7. Middlewares
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// (si luego usas login)
// app.UseAuthentication();
// app.UseAuthorization();

// 🔹 8. Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// 🔹 9. Run
app.Run();