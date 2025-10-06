using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    public UsuarioService(IUsuarioRepository repo) => _repo = repo;

    public Task<IEnumerable<Usuario>> ListarAsync(string? filtro) => _repo.ListarAsync(filtro);
    public Task<Usuario?> ObtenerAsync(int id) => _repo.ObtenerAsync(id);
    public Task<int> CrearAsync(Usuario u, string plainPassword) => _repo.CrearAsync(u, plainPassword);
    public Task<bool> ActualizarAsync(Usuario u, string? plainPassword) => _repo.ActualizarAsync(u, plainPassword);
    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);
}
