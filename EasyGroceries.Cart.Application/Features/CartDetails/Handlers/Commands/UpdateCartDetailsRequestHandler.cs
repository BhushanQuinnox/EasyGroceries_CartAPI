using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Validators;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Features.CartDetails.Handlers.Commands
{
    public class UpdateCartDetailsRequestHandler : IRequestHandler<UpdateCartDetailsRequest, bool>
    {
        private readonly ICartDetailsRepository _cartDetailsRepository;
        private readonly IMapper _mapper;

        public UpdateCartDetailsRequestHandler(ICartDetailsRepository cartDetailsRepository, IMapper mapper)
        {
            _cartDetailsRepository = cartDetailsRepository;
            _mapper = mapper;
        }

        public async Task<bool> Handle(UpdateCartDetailsRequest request, CancellationToken cancellationToken)
        {
            var validator = new CartDetailsDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CartDetailsDto);
            if (!validationResult.IsValid)
            {
                return false;
            }

            var cartDetails = _mapper.Map<Domain.CartDetails>(request.CartDetailsDto);
            await _cartDetailsRepository.UpdateCount(cartDetails);
            return true;
        }
    }
}
