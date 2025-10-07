using EjemploCoreWeb.Repository;
using EjemploCoreWeb.Services;
using EjemploCoreWeb.Services.Abstract;
using System.Data;

var builder = WebApplication.CreateBuilder(args);

// ---------------------------
// Servicios principales
// ---------------------------
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();

// ---------------------------
// Conexi�n a base de datos
// ---------------------------
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ---------------------------
// Inyecci�n de dependencias (repositories y services)
// ---------------------------
builder.Services.AddScoped<IInconsistenciaRepository, InconsistenciaRepository>();
builder.Services.AddScoped<IInconsistenciaService, InconsistenciaService>();

builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<IBitacoraService, BitacoraService>();

// ---------------------------
// Configuraci�n de sesiones
// ---------------------------
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Tiempo de expiraci�n
    options.Cookie.HttpOnly = true;                 // Solo accesible v�a HTTP
    options.Cookie.IsEssential = true;              // Necesario para GDPR
});

// ---------------------------
// Construcci�n del app
// ---------------------------
var app = builder.Build();

// ---------------------------
//  Configuraci�n del pipeline HTTP
// ---------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();

app.UseSession();

app.UseAuthorization();

// ---------------------------
// Redirecci�n inicial (ruta por defecto)
// ---------------------------
app.MapGet("/", context =>
{
    context.Response.Redirect("/InconsistenciasM/Index");
    return Task.CompletedTask;
});

// ---------------------------
// Razor Pages
// ---------------------------
app.MapRazorPages();

// ---------------------------
// Ejecutar la app
// ---------------------------
app.Run();
