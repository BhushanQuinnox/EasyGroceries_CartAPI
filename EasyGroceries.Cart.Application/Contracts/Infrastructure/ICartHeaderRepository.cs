using EasyGroceries.Cart.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Contracts.Infrastructure
{
    public interface ICartHeaderRepository
    {
        Task<CartHeader> GetCartHeaderByUserId(int userId);
        Task Add(CartHeader cartHeader);
        Task Delete(int cartHeaderId);
    }
}
