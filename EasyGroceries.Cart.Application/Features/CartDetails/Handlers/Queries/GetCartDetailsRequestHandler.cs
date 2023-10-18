using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartDetails.Handlers.Queries
{
    public class GetCartDetailsRequestHandler : IRequestHandler<GetCartDetailsRequest, List<CartDetailsDto>>
    {
        private readonly ICartDetailsRepository _cartDetailsRepository;
        private readonly IMapper _mapper;

        public GetCartDetailsRequestHandler(ICartDetailsRepository cartDetailsRepository, IMapper mapper)
        {
            _cartDetailsRepository = cartDetailsRepository;
            _mapper = mapper;
        }

        public async Task<List<CartDetailsDto>> Handle(GetCartDetailsRequest request, CancellationToken cancellationToken)
        {
            var cartDetailsList = await _cartDetailsRepository.GetAllCartDetails();
            return _mapper.Map<List<CartDetailsDto>>(cartDetailsList);
        }
    }
}
