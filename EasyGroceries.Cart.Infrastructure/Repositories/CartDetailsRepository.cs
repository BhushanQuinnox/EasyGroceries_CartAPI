using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Infrastructure.Repositories
{
    public class CartDetailsRepository : ICartDetailsRepository
    {
        // private readonly IConfiguration _configuration;
        private readonly string _connectionString;

        public CartDetailsRepository(IConfiguration configuration)
        {
            // _configuration = configuration;
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task AddCartDetails(CartDetails cartDetails)
        {
            var sqlCommand = "Insert into CartDetails (CartDetailsId, CartHeaderId, ProductId, Count) values (@CartDetailsId, @CartHeaderId, @ProductId, @Count)";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sqlCommand, new
                {
                    CartDetailsId = cartDetails.CartDetailsId,
                    CartHeaderId = cartDetails.CartHeaderId,
                    ProductId = cartDetails.ProductId,
                    Count = cartDetails.Count,
                });

                connection.Close();
            }
        }

        public Task Delete(CartDetails cartDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<CartDetails>> GetAllCartDetails()
        {
            var sqlCommand = "SELECT * FROM CartDetails";
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                var result = await connection.QueryAsync<CartDetails>(sqlCommand);
                return result.ToList();
            }
        }


        public async Task UpdateCount(CartDetails cartDetails)
        {
            var sqlCommand = "Update CartDetails set Count = @Count Where CartDetailsId = @CartDetailsId";
            var parameters = new DynamicParameters();
            parameters.Add("CartDetailsId", cartDetails.CartDetailsId);
            parameters.Add("Count", cartDetails.Count);

            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                await connection.ExecuteAsync(sqlCommand, parameters);
                connection.Close();
            }
        }
    }
}
