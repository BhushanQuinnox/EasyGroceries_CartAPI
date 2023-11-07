using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries;
using EasyGroceries.Cart.Domain;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Services
{
    public class CartService : ICartService
    {
        private readonly IMediator _mediator;
        private readonly IProductService _productService;
        private readonly ILogger<CartService> _logger;

        public CartService(IMediator mediator, IProductService productService, ILogger<CartService> logger)
        {
            _mediator = mediator;
            _productService = productService;
            _logger = logger;
        }

        public async Task<ResponseDto<CartDto>> CartUpsert(CartDto cartDto)
        {
            ResponseDto<CartDto> response = new ResponseDto<CartDto>();
            bool validProduct = await isProductExists(cartDto.CartDetails.First().ProductId);
            if (validProduct)
            {
                var cartHeaderFromDb = _mediator.Send(new GetCartHeaderRequest() { UserId = cartDto.CartHeader.UserId }).Result;
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    _logger.LogInformation("Cart header is not present");
                    _logger.LogInformation("Creating Cart Header");
                    var result = _mediator.Send(
                        new CreateCartHeaderRequest() { CartHeaderDto = cartDto.CartHeader }).Result;
                    if (!result.IsSuccess)
                    {
                        response.Status = result.Status;
                        response.Message = response.Message;
                        response.IsSuccess = result.IsSuccess;
                        return response;
                    }

                    _logger.LogInformation("Creating Cart Details");
                    cartDto.CartDetails.First().CartHeaderId = result.Result.CartHeaderId;
                    response.IsSuccess = await _mediator.Send(new CreateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    _logger.LogInformation("Cart header is already exists");
                    var cartDetailsList = await _mediator.Send(new GetCartDetailsRequest());
                    var cartDetailsOfProduct = cartDetailsList.FirstOrDefault(x => x.CartHeaderId == cartHeaderFromDb.CartHeaderId
                                                                && x.ProductId == cartDto.CartDetails.First().ProductId);
                    if (cartDetailsOfProduct == null)
                    {
                        //create cartdetails
                        _logger.LogInformation("Creating Cart details");
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        response.IsSuccess = await _mediator.Send(new CreateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                    }
                    else
                    {
                        //update count in cart details
                        _logger.LogInformation("Updating count in cart details");
                        cartDto.CartDetails.First().Count += cartDetailsOfProduct.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsOfProduct.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsOfProduct.CartDetailsId;
                        response.IsSuccess = await _mediator.Send(new UpdateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                    }
                }
            }
            else
            {
                _logger.LogInformation("Failed to add header and details as product is not valid");
                response.IsSuccess = false;
                response.Message = $"Product of id {cartDto.CartDetails.First().ProductId} is not exists";
                response.Status = (int)HttpStatusCode.BadRequest;
            }

            if (response.IsSuccess)
                response.Result = cartDto;

            return response;
        }

        public async Task<ResponseDto<CartDto>> GetCart(int userId)
        {
            ResponseDto<CartDto> response = new ResponseDto<CartDto>();

            CartDto cart = new CartDto
            {
                CartHeader = await _mediator.Send(new GetCartHeaderRequest() { UserId = userId })
            };

            List<CartDetailsDto> allCartDetails = await _mediator.Send(new GetCartDetailsRequest());
            cart.CartDetails = allCartDetails.Where(x => x.CartHeaderId == cart.CartHeader.CartHeaderId);
            if (cart.CartDetails != null && cart.CartDetails.Count() > 0)
            {
                // Retrive all the products from Product Service
                IEnumerable<ProductDto> products = await _productService.GetProducts();

                if (products != null && products.Any())
                {
                    foreach (var cartDetail in cart.CartDetails)
                    {
                        cartDetail.Product = products.FirstOrDefault(x => x.ProductId == cartDetail.ProductId);

                        if (cartDetail.Product != null)
                            cart.CartHeader.CartTotal += (cartDetail.Count * cartDetail.Product.Price);
                    }
                }
            }

            response.Result = cart;
            response.Status = (int)HttpStatusCode.OK;

            return response;
        }

        public async Task<ResponseDto<CartDto>> RemoveCart(int cartDetailsId)
        {
            // TBD: Future implementation
            return null;
        }

        private async Task<bool> isProductExists(int productId)
        {
            bool productExists = false;

            // Retrive all the products from Product Service
            IEnumerable<ProductDto> products = await _productService.GetProducts();
            var product = products.FirstOrDefault(x => x.ProductId == productId);
            if (product != null)
            {
                productExists = true;
            }

            return productExists;
        }

    }
}
