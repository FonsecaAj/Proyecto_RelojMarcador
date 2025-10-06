using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _repo;
    private readonly IUsuarioRepository _usuarios;

    public AreaService(IAreaRepository repo, IUsuarioRepository usuarios)
    {
        _repo = repo;
        _usuarios = usuarios;
    }

    public Task<IEnumerable<Area>> ListarAsync(string? filtro) => _repo.ListarAsync(filtro);
    public Task<Area?> ObtenerAsync(int id) => _repo.ObtenerAsync(id);
    public Task<int> CrearAsync(Area a) => _repo.CrearAsync(a);
    public Task<bool> ActualizarAsync(Area a) => _repo.ActualizarAsync(a);
    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);

    public async Task<IEnumerable<(int Id, string Nombre)>> ListarFuncionariosAsync()
        => (await _usuarios.ListarAsync(null))
            .Select(u => (u.ID_Usuario, $"{u.Nombre} {u.Apellido_1} {u.Apellido_2}".Trim()));
}
