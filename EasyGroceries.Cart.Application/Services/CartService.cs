using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries;
using EasyGroceries.Cart.Domain;
using MediatR;
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

        public CartService(IMediator mediator, IProductService productService)
        {
            _mediator = mediator;
            _productService = productService;
        }

        public async Task<ResponseDto<CartDto>> CartUpsert(CartDto cartDto)
        {
            ResponseDto<CartDto> response = new ResponseDto<CartDto>();
            try
            {
                var cartHeaderFromDb = _mediator.Send(new GetCartHeaderRequest() { UserId = cartDto.CartHeader.UserId }).Result;
                if (cartHeaderFromDb == null)
                {
                    //create header and details
                    var result = _mediator.Send(
                        new CreateCartHeaderRequest() { CartHeaderDto = cartDto.CartHeader }).Result;
                    if (!result.IsSuccess)
                    {
                        response.Status = result.Status;
                        response.Message = response.Message;
                        response.IsSuccess = result.IsSuccess;
                        return response;
                    }

                    cartDto.CartDetails.First().CartHeaderId = result.Result.CartHeaderId;
                    response.IsSuccess = await _mediator.Send(new CreateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                }
                else
                {
                    //if header is not null
                    //check if details has same product
                    var cartDetailsList = await _mediator.Send(new GetCartDetailsRequest());
                    var cartDetailsOfProduct = cartDetailsList.FirstOrDefault(x => x.CartHeaderId == cartHeaderFromDb.CartHeaderId
                                                                && x.ProductId == cartDto.CartDetails.First().ProductId);
                    if (cartDetailsOfProduct == null)
                    {
                        //create cartdetails
                        cartDto.CartDetails.First().CartHeaderId = cartHeaderFromDb.CartHeaderId;
                        response.IsSuccess = await _mediator.Send(new CreateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                    }
                    else
                    {
                        //update count in cart details
                        cartDto.CartDetails.First().Count += cartDetailsOfProduct.Count;
                        cartDto.CartDetails.First().CartHeaderId = cartDetailsOfProduct.CartHeaderId;
                        cartDto.CartDetails.First().CartDetailsId = cartDetailsOfProduct.CartDetailsId;
                        response.IsSuccess = await _mediator.Send(new UpdateCartDetailsRequest() { CartDetailsDto = cartDto.CartDetails.First() });
                    }
                }

                if (response.IsSuccess)
                    response.Result = cartDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.IsSuccess = false;
            }

            return response;
        }

        public async Task<ResponseDto<CartDto>> GetCart(int userId)
        {
            ResponseDto<CartDto> response = new ResponseDto<CartDto>();

            try
            {
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

                    foreach (var cartDetail in cart.CartDetails)
                    {
                        cartDetail.Product = products.FirstOrDefault(x => x.ProductId == cartDetail.ProductId);
                        cart.CartHeader.CartTotal += (cartDetail.Count * cartDetail.Product.Price);
                    }
                }

                response.Result = cart;
                response.Status = (int)HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.Status = (int)HttpStatusCode.InternalServerError;
            }

            return response;
        }

        public async Task<ResponseDto<CartDto>> RemoveCart(int cartDetailsId)
        {
            return null;
        }

    }
}
