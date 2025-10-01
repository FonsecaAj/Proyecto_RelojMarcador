﻿using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace EjemploCoreWeb.Repository
{
    public class DbConnectionFactory : IDbConnectionFactory
    {


        private readonly IConfiguration _configuration;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public IDbConnection CreateConnection()
        {

            return new SqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        }
    
    
    }


}
