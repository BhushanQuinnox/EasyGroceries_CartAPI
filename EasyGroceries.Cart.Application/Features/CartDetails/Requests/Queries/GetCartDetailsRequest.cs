using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries
{
    public class GetCartDetailsRequest : IRequest<List<CartDetailsDto>>
    {
    }
}
