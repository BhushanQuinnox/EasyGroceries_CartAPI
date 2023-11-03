using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands;
using EasyGroceries.Cart.Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyGroceries.Cart.Domain;
using EasyGroceries.Cart.Application.Validators;
using System.Net;

namespace EasyGroceries.Cart.Application.Features.CartHeader.Handlers.Commands
{
    public class CreateCartHeaderRequestHandler : IRequestHandler<CreateCartHeaderRequest, ResponseDto<CartHeaderDto>>
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly IMapper _mapper;

        public CreateCartHeaderRequestHandler(ICartHeaderRepository cartHeaderRepository, IMapper mapper)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<CartHeaderDto>> Handle(CreateCartHeaderRequest request, CancellationToken cancellationToken)
        {
            var validator = new CartHeaderDtoValidator();
            var validationResult = await validator.ValidateAsync(request.CartHeaderDto);
            var response = new ResponseDto<CartHeaderDto>();

            if (!validationResult.IsValid)
            {
                response.IsSuccess = false;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Message = "Validation failed";
                return response;
            }

            var cartHeader = _mapper.Map<EasyGroceries.Cart.Domain.CartHeader>(request.CartHeaderDto);
            await _cartHeaderRepository.Add(cartHeader);
            response.Status = (int)HttpStatusCode.Created;
            response.Message = "CartHeader Creation Successful";
            response.Result = request.CartHeaderDto;
            return response;
        }
    }
}
