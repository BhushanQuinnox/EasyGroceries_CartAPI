using EasyGroceries.Cart.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Contracts.Infrastructure
{
    public interface ICartDetailsRepository
    {
        Task AddCartDetails(CartDetails cartDetails);
        Task<IReadOnlyList<CartDetails>> GetAllCartDetails();
        Task<CartDetails> GetCartDetailsByCartDetailsId(int cartDetailsId);
        Task<CartDetails> GetCartDetailsByHeaderId(int headerId);
        Task AddCartDetailsList(List<CartDetails> cartDetails);
        Task Update(CartDetails cartDetails);
        Task Delete(CartDetails cartDetails);
    }
}
