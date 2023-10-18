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
        private readonly IConfiguration _configuration;

        public CartDetailsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AddCartDetails(CartDetails cartDetails)
        {
            var sqlCommand = "Insert into CartDetails (CartDetailsId, CartHeaderId, ProductId, Count) values (@CartDetailsId, @CartHeaderId, @ProductId, @Count)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
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
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QueryAsync<CartDetails>(sqlCommand);
                return result.ToList();
            }
        }


        public async Task Update(CartDetails cartDetails)
        {
            var sqlCommand = "Update CartDetails set CartDetailsId = @CartDetailsId, CartHeaderId = @CartHeaderId, ProductId = @ProductId, Count = @Count Where Id = @Id";
            var parameters = new DynamicParameters();
            parameters.Add("Id", cartDetails.CartDetailsId);
            parameters.Add("CartDetailsId", cartDetails.CartDetailsId);
            parameters.Add("CartHeaderId", cartDetails.CartHeaderId);
            parameters.Add("ProductId", cartDetails.ProductId);
            parameters.Add("Count", cartDetails.Count);

            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                await connection.ExecuteAsync(sqlCommand, parameters);
                connection.Close();
            }
        }
    }
}
