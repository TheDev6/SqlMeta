namespace SqlMeta.Data.Wrappers
{
    using System;
    using System.Data;
    using System.Data.SqlClient;
    using Base;

    public class DatabaseWrapper : IDatabaseWrapper
    {
        private readonly string _connectionString;

        public DatabaseWrapper(string connectionString)
        {
            _connectionString = connectionString;
        }

        public virtual IDbConnection GetOpenDbConnection()
        {
            var result = new SqlConnection(_connectionString);
            result.Open();
            return result;
        }

        public TResult Call<TResult>(Func<IDbConnection, TResult> func)
        {
            using (var sqlConnection = GetOpenDbConnection())
            {
                return func(sqlConnection);
            }
        }
    }
}