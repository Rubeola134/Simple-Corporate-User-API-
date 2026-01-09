using System.Data;                 // IDbConnection
using Microsoft.Data.SqlClient;    // SqlConnection
using Dapper;

namespace API.Data
{
    public class DataContextDapper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public IEnumerable<T> LoadData<T>(string sql)
        {
            using IDbConnection db = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            return db.Query<T>(sql);
        }

        public T LoadDataSingle<T>(string sql)
        {
            using IDbConnection db = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            return db.QuerySingle<T>(sql);
        }

        public int ExecuteSqlwithRowCount(string sql, object parameters)
        {
            using IDbConnection db = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection")
    );
            return db.Execute(sql, parameters);
        }
    
        public bool ExecuteSql(string sql, object parameters)
        {
            using IDbConnection db = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection")
    );
            return db.Execute(sql, parameters) > 0;
        }
    }
}
