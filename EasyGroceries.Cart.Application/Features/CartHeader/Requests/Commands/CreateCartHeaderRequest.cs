using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands
{
    public class CreateCartHeaderRequest : IRequest<ResponseDto<CartHeaderDto>>
    {
        public CartHeaderDto CartHeaderDto { get; set; }
    }
}
