using EasyGroceries.Cart.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Contracts.Services
{
    public interface ICartService
    {
        Task<ResponseDto<CartDto>> CartUpsert(CartDto cartDto);
        Task<ResponseDto<CartDto>> GetCart(int userId);
        Task<ResponseDto<CartDto>> RemoveCart(int cartDetailsId);
    }
}
