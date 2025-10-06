using EjemploCoreWeb.Entities;

namespace EjemploCoreWeb.Services.Interfaces;

public interface IUsuarioAreaService
{
    Task<(Usuario? usuario, IEnumerable<UsuarioArea> asociadas, IEnumerable<Area> disponibles)> CargarAsync(int idUsuario);
    Task AsociarAsync(int idUsuario, int idArea);
    Task<bool> DesasociarAsync(int idUsuario, int idArea);
}

