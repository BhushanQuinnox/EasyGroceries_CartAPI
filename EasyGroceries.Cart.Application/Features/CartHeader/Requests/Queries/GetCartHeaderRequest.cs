using EasyGroceries.Cart.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries
{
    public class GetCartHeaderRequest : IRequest<CartHeaderDto>
    {
        public int UserId { get; set; }
    }
}
