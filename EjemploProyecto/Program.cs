using EjemploCoreWeb.Repository;                       // DbConnectionFactory
using EjemploCoreWeb.Repository.Interfaces;            // repos interfaces
using EjemploCoreWeb.Repository.Repositories;          // repos impl

// === Servicios NUEVOS (HU7, HU8, HU9) ===
using SvcIf = EjemploCoreWeb.Services.Interfaces;     // IUsuarioService, IAreaService, IUsuarioAreaService, IRolService
using SvcImpl = EjemploCoreWeb.Services.Services;      // UsuarioService, AreaService, UsuarioAreaService, RolService

// === Servicio del ejemplo del profe (Personas) ===
// OJO: este es el árbol “viejo”, lo aliasamos para no chocar nombres
using PersonaAbstr = EjemploCoreWeb.Services.Abstract; // IPersonaService
using PersonaImpl = EjemploCoreWeb.Services;          // PersonaService

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Factory de conexión
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ===== Ejemplo del profe (Personas) =====
builder.Services.AddScoped<PersonaRepository>();
builder.Services.AddScoped<PersonaAbstr.IPersonaService, PersonaImpl.PersonaService>();

// ===== HU7 / HU8 / HU9 =====
// Repos
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAreaRepository, AreaRepository>();
builder.Services.AddScoped<IUsuarioAreaRepository, UsuarioAreaRepository>();
builder.Services.AddScoped<IRolRepository, RolRepository>();

// Services
builder.Services.AddScoped<SvcIf.IUsuarioService, SvcImpl.UsuarioService>();
builder.Services.AddScoped<SvcIf.IAreaService, SvcImpl.AreaService>();
builder.Services.AddScoped<SvcIf.IUsuarioAreaService, SvcImpl.UsuarioAreaService>();
builder.Services.AddScoped<SvcIf.IRolService, SvcImpl.RolService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.Run();
