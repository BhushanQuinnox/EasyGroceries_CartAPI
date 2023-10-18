using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Domain;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;

namespace EasyGroceries.Cart.Infrastructure.Repositories
{
    public class CartHeaderRepository : ICartHeaderRepository
    {
        private readonly IConfiguration _configuration;
        public CartHeaderRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task Add(CartHeader cartHeader)
        {
            var sql = "Insert into CartHeader (CartHeaderId, UserId, LoyaltyMembershipOpted, CartTotal) values (@CartHeaderId, @UserId, @LoyaltyMembershipOpted, @CartTotal)";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new
                {
                    CartHeaderId = cartHeader.CartHeaderId,
                    UserId = cartHeader.UserId,
                    LoyaltyMembershipOpted = cartHeader.LoyaltyMembershipOpted,
                    CartTotal = cartHeader.CartTotal,
                });

                connection.Close();
            }
        }

        public async Task Delete(int cartHeaderId)
        {
            var sql = "Delete FROM CartHeader WHERE CartHeaderId = @id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.ExecuteAsync(sql, new { cartHeaderId });
                connection.Close();
            }
        }

        public async Task<CartHeader> GetCartHeaderByUserId(int id)
        {
            var sql = "SELECT * FROM CartHeader WHERE UserId = @id";
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();
                var result = await connection.QuerySingleOrDefaultAsync<CartHeader>(sql, new { id });
                return result;
            }
        }
    }
}
