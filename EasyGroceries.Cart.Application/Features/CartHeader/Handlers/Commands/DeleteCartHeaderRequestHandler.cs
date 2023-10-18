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
    public class DeleteCartHeaderRequestHandler : IRequestHandler<DeleteCartHeaderRequest, ResponseDto<CartHeaderDto>>
    {
        private readonly ICartHeaderRepository _cartHeaderRepository;
        private readonly IMapper _mapper;

        public DeleteCartHeaderRequestHandler(ICartHeaderRepository cartHeaderRepository, IMapper mapper)
        {
            _cartHeaderRepository = cartHeaderRepository;
            _mapper = mapper;
        }

        public async Task<ResponseDto<CartHeaderDto>> Handle(DeleteCartHeaderRequest request, CancellationToken cancellationToken)
        {
            var response = new ResponseDto<CartHeaderDto>();
            await _cartHeaderRepository.Delete(request.CartHeaderId);
            response.Message = "CartHeader Deletion Successful";
            return response;
        }
    }
}
