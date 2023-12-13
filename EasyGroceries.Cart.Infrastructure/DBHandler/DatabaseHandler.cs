

using System.Data;
using System.Data.SqlClient;
using Dapper;
using EasyGroceries.Cart.Infrastructure.Contracts;
using Microsoft.Extensions.Configuration;

namespace EasyGroceries.Cart.Infrastructure.DBHandler
{
    public class DatabaseHandler : IDapper
    {
        private readonly string _connectionString;

        public DatabaseHandler(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public void Dispose()
        {

        }

        public int Execute<T>(string sp, DynamicParameters parameters, CommandType commandType = CommandType.StoredProcedure)
        {
            int result = 0;

            using IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Execute(sp, parameters, transaction: tran);
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Get<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.Text)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(sp, parms, commandType: commandType).FirstOrDefault();
        }

        public List<T> GetAll<T>(string sp, T data, CommandType commandType = CommandType.StoredProcedure)
        {
            using IDbConnection db = new SqlConnection(_connectionString);
            return db.Query<T>(sp, data, commandType: commandType).ToList();
        }

        public T Insert<T>(string sp, T data, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, data, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        public T Update<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        {
            T result;
            using IDbConnection db = new SqlConnection(_connectionString);
            try
            {
                if (db.State == ConnectionState.Closed)
                    db.Open();

                using var tran = db.BeginTransaction();
                try
                {
                    result = db.Query<T>(sp, parms, commandType: commandType, transaction: tran).FirstOrDefault();
                    tran.Commit();
                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    throw ex;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (db.State == ConnectionState.Open)
                    db.Close();
            }

            return result;
        }

        // public int Delete<T>(string sp, DynamicParameters parms, CommandType commandType = CommandType.StoredProcedure)
        // {
        //     int result;
        //     using IDbConnection db = new SqlConnection(_connectionString);
        //     try
        //     {
        //         if (db.State == ConnectionState.Closed)
        //             db.Open();

        //         using var tran = db.BeginTransaction();
        //         try
        //         {
        //             result = db.Execute(sp, parms, commandType: commandType, transaction: tran);
        //             tran.Commit();
        //         }
        //         catch (Exception ex)
        //         {
        //             tran.Rollback();
        //             throw ex;
        //         }
        //     }
        //     catch (Exception ex)
        //     {
        //         throw ex;
        //     }
        //     finally
        //     {
        //         if (db.State == ConnectionState.Open)
        //             db.Close();
        //     }

        //     return result;
        // }

    }
}