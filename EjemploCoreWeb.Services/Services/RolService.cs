using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using EjemploCoreWeb.Services.Interfaces;

namespace EjemploCoreWeb.Services.Services
{
    public class RolService : IRolService
    {
        private readonly IRolRepository _repo;
        public RolService(IRolRepository repo) => _repo = repo;

        public Task<IEnumerable<Rol>> ListarAsync() => _repo.ListarAsync();
        public Task<bool> ExisteAsync(int idRol) => _repo.ExisteAsync(idRol);
    }
}
