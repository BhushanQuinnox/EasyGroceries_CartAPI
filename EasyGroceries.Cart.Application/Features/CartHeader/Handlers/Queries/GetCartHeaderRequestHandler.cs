using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartHeader.Handlers.Queries
{
    public class GetCartHeaderRequestHandler : IRequestHandler<GetCartHeaderRequest, CartHeaderDto>
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly IMapper _mapper;

        public GetCartHeaderRequestHandler(ICartHeaderRepository cartHeaderRepository, IMapper mapper)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _mapper = mapper;
        }

        public async Task<CartHeaderDto> Handle(GetCartHeaderRequest request, CancellationToken cancellationToken)
        {
            var cartHeader = await _cartHeaderRepository.GetCartHeaderByUserId(request.UserId);
            return _mapper.Map<CartHeaderDto>(cartHeader);
        }
    }
}
