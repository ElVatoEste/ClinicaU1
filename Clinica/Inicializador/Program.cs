using Clinica.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Clinica.Common;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Configurar cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
// Configuración global de cultura
var cultureInfo = new CultureInfo("en-US");
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

// Configurar DbContext para Identity
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Configurar ASP.NET Core Identity
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = true;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.SignIn.RequireConfirmedAccount = false; // Ajusta según tus requisitos
})
.AddRoles<IdentityRole>() // Soporte para roles
.AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.LoginPath = "/Account/Login"; // Página de inicio de sesión
    options.AccessDeniedPath = "/Account/AccessDenied"; // Página de acceso denegado
});

// Agregar el inicializador
builder.Services.AddScoped<Initializer>();
builder.Services.AddScoped<StoredProcedureHelper>();

// Agregar controladores y vistas
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Ejecutar migraciones automáticamente (opcional)
await ApplyMigrationsAsync(app);

// Ejecutar el inicializador
await InitializeAppAsync(app);

// Configurar el middleware
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Middleware de autenticación
app.UseAuthorization();  // Middleware de autorización

// Rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}");

// Ejecutar la aplicación
app.Run();

// Función para aplicar migraciones automáticamente (opcional)
static async Task ApplyMigrationsAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        if (context.Database.IsSqlServer())
        {
            await context.Database.MigrateAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error al aplicar migraciones: {ex.Message}");
        throw;
    }
}

// Función para ejecutar la inicialización de roles y usuarios
static async Task InitializeAppAsync(WebApplication app)
{
    using var scope = app.Services.CreateScope();
    var services = scope.ServiceProvider;
    try
    {
        var initializer = services.GetRequiredService<Initializer>();
        await initializer.InitializeAsync();
        Console.WriteLine("Inicialización completada.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error durante la inicialización: {ex.Message}");
        throw;
    }
}
