using EjemploCoreWeb.Entities;

namespace EjemploCoreWeb.Services.Interfaces;

public interface IUsuarioService
{
    Task<IEnumerable<Usuario>> ListarAsync(string? filtro);
    Task<Usuario?> ObtenerAsync(int id);
    Task<int> CrearAsync(Usuario u, string plainPassword);
    Task<bool> ActualizarAsync(Usuario u, string? plainPassword);
    Task<bool> EliminarAsync(int id);
}
