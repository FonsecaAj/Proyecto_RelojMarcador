using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _repo;
    public AreaService(IAreaRepository repo) => _repo = repo;

    public Task<IEnumerable<Area>> ListarAsync(string? filtro) => _repo.ListarAsync(filtro);
    public Task<Area?> ObtenerAsync(int id) => _repo.ObtenerAsync(id);
    public Task<int> CrearAsync(Area a) => _repo.CrearAsync(a);
    public Task<bool> ActualizarAsync(Area a) => _repo.ActualizarAsync(a);
    public Task<bool> EliminarAsync(int id) => _repo.EliminarAsync(id);
}
