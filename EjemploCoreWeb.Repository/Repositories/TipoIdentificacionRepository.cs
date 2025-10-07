using System.Data;
using Dapper;
using EjemploCoreWeb.Entities;
using EjemploCoreWeb.Repository.Interfaces;

namespace EjemploCoreWeb.Repository.Repositories;

public class TipoIdentificacionRepository : ITipoIdentificacionRepository
{
    private readonly IDbConnectionFactory _factory;
    public TipoIdentificacionRepository(IDbConnectionFactory factory) => _factory = factory;

    public async Task<IEnumerable<TipoIdentificacion>> ListarAsync()
    {
        using var con = _factory.CreateConnection();
        const string sql = @"
SELECT ID_Tipo_Identificacion, Tipo_Identificacion
FROM Tipos_Identificacion
ORDER BY Tipo_Identificacion;";
        return await con.QueryAsync<TipoIdentificacion>(sql);
    }
}
