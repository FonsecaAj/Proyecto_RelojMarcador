using EjemploCoreWeb.Repository;                    // IDbConnectionFactory, DbConnectionFactory, repos que ya existían (Inconsistencia, Usuario, etc.)
using EjemploCoreWeb.Repository.Interfaces;         // IUsuarioRepository, IAreaRepository, IUsuarioAreaRepository (HU7–HU9)
using EjemploCoreWeb.Repository.Repositories;       // UsuarioRepository, AreaRepository, UsuarioAreaRepository (HU7–HU9)

using EjemploCoreWeb.Services;                      // PersonaService, UsuarioService, AreaService, UsuarioAreaService, BitacoraService, etc.
using EjemploCoreWeb.Services.Abstract;             // IPersonaService, IUsuarioService, IInconsistenciaService, IBitacoraService, etc.
using EjemploCoreWeb.Services.Interfaces;           // IAreaService, IUsuarioAreaService (HU7–HU9)

using System.Data;

var builder = WebApplication.CreateBuilder(args);

// ===== Razor Pages =====
builder.Services.AddRazorPages();

// ===== Conexión a BD (existente) =====
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// ===== Servicios/Repos EXISTENTES en Develop (NO TOCAR) =====
builder.Services.AddScoped<IInconsistenciaRepository, InconsistenciaRepository>();
builder.Services.AddScoped<IInconsistenciaService, InconsistenciaService>();

builder.Services.AddScoped<UsuarioRepository>();             // ya lo tenían así
builder.Services.AddScoped<IUsuarioService, UsuarioService>();

builder.Services.AddScoped<AusenciasRepository>();
builder.Services.AddScoped<IMotivos_Ausencia, Motivos_Services>();

builder.Services.AddScoped<AdmHorariosRepository>();
builder.Services.AddScoped<IHorarios, HorariosServices>();

//SE LLAMA INYECCION DE DEPENDENCIAS BROTHER

// servicio de bit�cora
builder.Services.AddScoped<IBitacoraService, BitacoraService>();

// (Opcional/ejemplo del profe: lo dejamos activo, no estorba)
builder.Services.AddScoped<PersonaRepository>();
builder.Services.AddScoped<IPersonaService, PersonaService>();

// ===== NUEVO: HU7–HU9 (tus historias) =====
// Repositorios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();          // HU7
builder.Services.AddScoped<IAreaRepository, AreaRepository>();                // HU8
builder.Services.AddScoped<IUsuarioAreaRepository, UsuarioAreaRepository>();  // HU9

// Servicios
builder.Services.AddScoped<IAreaService, AreaService>();                      // HU8
builder.Services.AddScoped<IUsuarioAreaService, UsuarioAreaService>();        // HU9

var app = builder.Build();

// ===== Pipeline HTTP =====
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Redirección temporal de pruebas (venía en develop)
app.MapGet("/", context =>
{
    context.Response.Redirect("/InconsistenciasM/Index");
    return Task.CompletedTask;
});

app.MapRazorPages();

app.Run();

