using EjemploCoreWeb.Repository;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Repository.Repositories;

// Alias para Personas (ejemplo del profe)
using PersonaAbstr = EjemploCoreWeb.Services.Abstract;
using PersonaImpl = EjemploCoreWeb.Services;

// Alias HU7/8/9
using SvcIf = EjemploCoreWeb.Services.Interfaces;
using SvcImpl = EjemploCoreWeb.Services.Services;

var builder = WebApplication.CreateBuilder(args);

// Razor Pages
builder.Services.AddRazorPages();

// Conexión
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
builder.Services.AddScoped<ITipoIdentificacionRepository, TipoIdentificacionRepository>(); // 👈 FALTABA

// Servicios
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
