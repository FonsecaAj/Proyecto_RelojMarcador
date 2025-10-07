using EjemploCoreWeb.Entities;

namespace EjemploCoreWeb.Repository.Interfaces;

public interface IUsuarioRepository
{
    Task<IEnumerable<Usuario>> ListarAsync(string? filtro);
    Task<Usuario?> ObtenerAsync(int id);
    Task<int> CrearAsync(Usuario u, string plainPassword);
    Task<bool> ActualizarAsync(Usuario u, string? plainPassword);
    Task<bool> EliminarAsync(int id); // false si hay FK relacionadas
}

