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
using EasyGroceries.Cart.Infrastructure.Contracts;
using System.Data;

namespace EasyGroceries.Cart.Infrastructure.Repositories
{
    public class CartDetailsRepository : ICartDetailsRepository
    {
        private readonly IDapper _dapper;

        public CartDetailsRepository(IDapper dapper)
        {
            _dapper = dapper;
        }

        public async Task AddCartDetails(CartDetails cartDetails)
        {
            var query = "Insert into CartDetails (CartDetailsId, CartHeaderId, ProductId, Count) values (@CartDetailsId, @CartHeaderId, @ProductId, @Count)";
            await Task.FromResult(_dapper.Insert<CartDetails>(query, cartDetails, commandType: CommandType.Text));
        }

        public Task Delete(CartDetails cartDetails)
        {
            throw new NotImplementedException();
        }

        public async Task<IReadOnlyList<CartDetails>> GetAllCartDetails()
        {
            var query = "SELECT * FROM CartDetails";
            var CartDetailsList = await Task.FromResult(_dapper.GetAll<CartDetails>(query, null, commandType: CommandType.Text));
            return CartDetailsList;
        }


        public async Task UpdateCount(CartDetails cartDetails)
        {
            var query = "Update CartDetails set Count = @Count Where CartDetailsId = @CartDetailsId";
            var parameters = new DynamicParameters();
            parameters.Add("CartDetailsId", cartDetails.CartDetailsId);
            parameters.Add("Count", cartDetails.Count);
            await Task.FromResult(_dapper.Update<CartDetails>(query, parameters, commandType: CommandType.Text));
        }
    }
}
