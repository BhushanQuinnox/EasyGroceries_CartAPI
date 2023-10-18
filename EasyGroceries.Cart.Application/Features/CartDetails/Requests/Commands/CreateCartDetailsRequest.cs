using EasyGroceries.Cart.Application.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands
{
    public class CreateCartDetailsRequest : IRequest<bool>
    {
        public CartDetailsDto CartDetailsDto { get; set; }
    }
}
