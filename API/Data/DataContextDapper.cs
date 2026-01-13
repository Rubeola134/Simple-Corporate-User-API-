using System.Data;                 // IDbConnection
using Microsoft.Data.SqlClient;    // SqlConnection
using Dapper;

namespace API.Data
{
    public class DataContextDapper(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration;

        public IEnumerable<T> LoadData<T>(string sql, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(
                _configuration.GetConnectionString("DefaultConnection")
            );

            return db.Query<T>(sql, parameters);
        }

        public T LoadDataSingle<T>(string sql, object? parameters = null)
        {
            using IDbConnection db = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection"));
            return db.QuerySingle<T>(sql, parameters);
        }


        public int ExecuteSqlwithRowCount(string sql, object? parameters = null)
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
        
        public bool ExecuteSqlWithParameter(string sql, List<SqlParameter> sqlParameters)
        {
            SqlCommand command = new SqlCommand(sql);
            foreach (SqlParameter param in sqlParameters)
            {
                command.Parameters.Add(param);
            }
            using SqlConnection db = new SqlConnection(
        _configuration.GetConnectionString("DefaultConnection")
    );      
            db.Open();
            command.Connection = db;
            
            int rowsAffected = command.ExecuteNonQuery();
            db.Close();
            return rowsAffected > 0;
            
        }
    }
}
