using Dapper;
using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;
using System.Data;

namespace EjemploCoreWeb.Repository.Repositories
{
    public class RolRepository : IRolRepository
    {
        private readonly IDbConnectionFactory _factory;
        public RolRepository(IDbConnectionFactory factory) => _factory = factory;

        public async Task<IEnumerable<Rol>> ListarAsync()
        {
            using var con = _factory.CreateConnection();
            var sql = "SELECT ID_Rol_Usuario, Nombre_Rol FROM Roles ORDER BY Nombre_Rol;";
            return await con.QueryAsync<Rol>(sql);
        }

        public async Task<bool> ExisteAsync(int idRol)
        {
            using var con = _factory.CreateConnection();
            var sql = "SELECT COUNT(1) FROM Roles WHERE ID_Rol_Usuario=@id;";
            var n = await con.ExecuteScalarAsync<long>(sql, new { id = idRol });
            return n > 0;
        }
    }
}
