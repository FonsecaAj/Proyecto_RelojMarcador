using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services.Services;

public class UsuarioService : IUsuarioService
{
    private readonly IUsuarioRepository _repo;
    private readonly IRolRepository _rolRepo;
    private readonly ITipoIdentificacionRepository _tipoRepo;

    public UsuarioService(
        IUsuarioRepository repo,
        IRolRepository rolRepo,
        ITipoIdentificacionRepository tipoRepo)
    {
        _repo = repo;
        _rolRepo = rolRepo;
        _tipoRepo = tipoRepo;
    }

    public Task<IEnumerable<Usuario>> ListarAsync(string? filtro)
        => _repo.ListarAsync(filtro);

    public Task<(IEnumerable<Usuario> Items, int Total)> ListarPaginadoAsync(string? filtro, int page, int size)
        => _repo.ListarPaginadoAsync(filtro, page, size);

    // Si no tienes Contar en el repo, lo calculamos con la consulta paginada
    public async Task<int> ContarAsync(string? filtro)
    {
        var (_, total) = await _repo.ListarPaginadoAsync(filtro, 1, 1);
        return total;
    }

    public Task<Usuario?> ObtenerAsync(int id)
        => _repo.ObtenerAsync(id);

    public Task<int> CrearAsync(Usuario u, string password)
        => _repo.CrearAsync(u, password);

    public Task<bool> ActualizarAsync(Usuario u, string? nuevaPassword)
        => _repo.ActualizarAsync(u, nuevaPassword);

    public Task<bool> EliminarAsync(int id)
        => _repo.EliminarAsync(id);

    public Task<IEnumerable<Rol>> ListarRolesAsync()
        => _rolRepo.ListarAsync();

    public Task<IEnumerable<TipoIdentificacion>> ListarTiposAsync()
        => _tipoRepo.ListarAsync();
}
