using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services;

public class UsuarioAreaService : IUsuarioAreaService
{
    private readonly IUsuarioRepository _usuarios;
    private readonly IUsuarioAreaRepository _ua;

    public UsuarioAreaService(IUsuarioRepository usuarios, IUsuarioAreaRepository ua)
    {
        _usuarios = usuarios;
        _ua = ua;
    }

    public async Task<(Usuario? usuario, IEnumerable<UsuarioArea> asociadas, IEnumerable<Area> disponibles)> CargarAsync(int idUsuario)
    {
        var u = await _usuarios.ObtenerAsync(idUsuario);
        var asociadas = await _ua.ListarPorUsuarioAsync(idUsuario);
        var disponibles = await _ua.ListarNoAsociadasAsync(idUsuario);
        return (u, asociadas, disponibles);
    }

    public Task AsociarAsync(int idUsuario, int idArea) => _ua.AsociarAsync(idUsuario, idArea);
    public Task<bool> DesasociarAsync(int idUsuario, int idArea) => _ua.DesasociarAsync(idUsuario, idArea);
}
