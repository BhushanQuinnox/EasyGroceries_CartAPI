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
using EasyGroceries.Cart.Infrastructure.Contracts;
using System.Data;

namespace EasyGroceries.Cart.Infrastructure.Repositories
{
    public class CartHeaderRepository : ICartHeaderRepository
    {
        private readonly IDapper _dapper;

        public CartHeaderRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task Add(CartHeader cartHeader)
        {
            var query = "Insert into CartHeader (CartHeaderId, UserId, LoyaltyMembershipOpted, CartTotal) values (@CartHeaderId, @UserId, @LoyaltyMembershipOpted, @CartTotal)";
            await Task.FromResult(_dapper.Insert<CartHeader>(query, cartHeader, commandType: CommandType.Text));
        }

        public async Task Delete(int cartHeaderId)
        {
            var query = "Delete FROM CartHeader WHERE CartHeaderId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", cartHeaderId, DbType.Int32, ParameterDirection.Input);
            await Task.FromResult(_dapper.Execute<CartHeader>(query, parameters, CommandType.Text));
        }

        public async Task<CartHeader> GetCartHeaderByUserId(int id)
        {
            var query = "SELECT * FROM CartHeader WHERE UserId = @id";
            var parameters = new DynamicParameters();
            parameters.Add("@id", id, DbType.Int32, ParameterDirection.Input);
            var cartHeader = await Task.FromResult(_dapper.Get<CartHeader>(query, parameters, CommandType.Text));
            return cartHeader;
        }
    }
}
