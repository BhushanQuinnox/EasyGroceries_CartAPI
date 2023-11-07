

using System.Net;
using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Queries;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries;
using EasyGroceries.Cart.Application.Services;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class CartServiceTests
{
    private readonly Mock<IMediator> _mediatorMock;

    private readonly Mock<IProductService> _productServiceMock;

    public CartServiceTests()
    {
        _mediatorMock = new Mock<IMediator>();
        _productServiceMock = new Mock<IProductService>();
    }

    [Fact]
    public async Task CartUpsert_Should_CreateNewCartHeaderAndCartDetails()
    {
        // Arrange
        CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto()
            {
                CartHeaderId = 100,
                CartTotal = 10,
                ApartmentName = "Atria Grande",
                City = "Pune",
                UserId = 1234,
                Name = "B N"
            },
            CartDetails = new List<CartDetailsDto>()
            {
                new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 100, Count = 5, ProductId = 1001},
                new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
                new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 300, Count = 9, ProductId = 3001},
                new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 400, Count = 3, ProductId = 4001},
                new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
            },
        };

        ResponseDto<CartHeaderDto> dummyResponse = new ResponseDto<CartHeaderDto>()
        {
            Result = cartDto.CartHeader,
            IsSuccess = true,
            Message = "Created Successfully",
            Status = (int)HttpStatusCode.Created
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(() => null);

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateCartHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(dummyResponse);

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        // Act
        var response = cartService.CartUpsert(cartDto).Result;

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(cartDto.CartHeader.Name, response.Result.CartHeader.Name);
        Assert.Equal(cartDto.CartHeader.CartTotal, response.Result.CartHeader.CartTotal);
        Assert.Equal(cartDto.CartDetails.First().Count, response.Result.CartDetails.First().Count);
    }

    [Fact]
    public async Task CartUpsert_Should_CreateNewCartDetailsWithExistingCartHeader()
    {
        // Arrange
        CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto()
            {
                CartHeaderId = 100,
                CartTotal = 10,
                ApartmentName = "Atria Grande",
                City = "Pune",
                UserId = 1234,
                Name = "B N"
            },
            CartDetails = new List<CartDetailsDto>()
            {
                new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 600, Count = 5, ProductId = 1001},
                new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
                new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 300, Count = 9, ProductId = 3001},
                new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 400, Count = 3, ProductId = 4001},
                new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
            },
        };

        ResponseDto<CartHeaderDto> dummyResponse = new ResponseDto<CartHeaderDto>()
        {
            Result = cartDto.CartHeader,
            IsSuccess = true,
            Message = "Created Successfully",
            Status = (int)HttpStatusCode.Created
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartHeader);

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartDetails.ToList());

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        // Act
        var response = cartService.CartUpsert(cartDto).Result;

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(cartDto.CartDetails.First().Count, response.Result.CartDetails.First().Count);
    }

    [Fact]
    public async Task CartUpsert_Should_UpdateCountAsCartDetailsWithCartHeaderAlreadyExists()
    {
        // Arrange
        CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto()
            {
                CartHeaderId = 100,
                CartTotal = 10,
                ApartmentName = "Atria Grande",
                City = "Pune",
                UserId = 1234,
                Name = "B N"
            },
            CartDetails = new List<CartDetailsDto>()
            {
                new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 100, Count = 7, ProductId = 1001},
                new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
                new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 300, Count = 9, ProductId = 3001},
                new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 400, Count = 3, ProductId = 4001},
                new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
            },
        };

        ResponseDto<CartHeaderDto> dummyResponse = new ResponseDto<CartHeaderDto>()
        {
            Result = cartDto.CartHeader,
            IsSuccess = true,
            Message = "Created Successfully",
            Status = (int)HttpStatusCode.Created
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartHeader);

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartDetails.ToList());

        _mediatorMock.Setup(x => x.Send(It.IsAny<CreateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(true);

        // Act
        var response = cartService.CartUpsert(cartDto).Result;

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(14, response.Result.CartDetails.First().Count);
    }

    // [Fact]
    // public async Task CartUpsert_Should_FailedDueToException()
    // {
    //     // Arrange
    //     CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

    //     CartDto cartDto = new CartDto()
    //     {
    //         CartHeader = new CartHeaderDto()
    //         {
    //             CartHeaderId = 100,
    //             CartTotal = 10,
    //             ApartmentName = "Atria Grande",
    //             City = "Pune",
    //             UserId = 1234,
    //             Name = "B N"
    //         },
    //         CartDetails = new List<CartDetailsDto>()
    //         {
    //             new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 100, Count = 7, ProductId = 1001},
    //             new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
    //             new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 300, Count = 9, ProductId = 3001},
    //             new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 400, Count = 3, ProductId = 4001},
    //             new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
    //         },
    //     };

    //     ResponseDto<CartHeaderDto> dummyResponse = new ResponseDto<CartHeaderDto>()
    //     {
    //         Result = cartDto.CartHeader,
    //         IsSuccess = true,
    //         Message = "Created Successfully",
    //         Status = (int)HttpStatusCode.Created
    //     };

    //     _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
    //                 .ThrowsAsync(new Exception("Exception occur while retrieving Cart Header information"));

    //     _mediatorMock.Setup(x => x.Send(It.IsAny<CreateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
    //                 .ReturnsAsync(true);

    //     _mediatorMock.Setup(x => x.Send(It.IsAny<UpdateCartDetailsRequest>(), It.IsAny<CancellationToken>()))
    //                 .ReturnsAsync(true);

    //     // Act
    //     var response = cartService.CartUpsert(cartDto).Result;

    //     // Assert
    //     _mediatorMock.Verify(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()),
    //                         Times.Once);

    //     _mediatorMock.Verify(x => x.Send(It.IsAny<CreateCartDetailsRequest>(), It.IsAny<CancellationToken>()),
    //                         Times.Never);

    //     _mediatorMock.Verify(x => x.Send(It.IsAny<UpdateCartDetailsRequest>(), It.IsAny<CancellationToken>()),
    //                         Times.Never);
    //     // Assert.False(response.IsSuccess);
    //     // Assert.Null(response.Result);
    //     // Assert.Contains("Exception occur while retrieving Cart Header information", response.Message);
    // }

    [Fact]
    public async Task GetCart_Should_ReturnsCartInfoOfUserId()
    {
        // Arrange
        CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

        CartDto cartDto = new CartDto()
        {
            CartHeader = new CartHeaderDto()
            {
                CartHeaderId = 100,
                CartTotal = 10,
                ApartmentName = "Atria Grande",
                City = "Pune",
                UserId = 1234,
                Name = "B N"
            },
            CartDetails = new List<CartDetailsDto>()
            {
                new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 100, Count = 7, ProductId = 1001},
                new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
                new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 100, Count = 9, ProductId = 3001},
                new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 100, Count = 3, ProductId = 4001},
                new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
            },
        };

        IEnumerable<ProductDto> productDtos = new List<ProductDto>()
        {
            new ProductDto(){ProductId = 1001, Name = "Dove", Price = 45, Category = "Cosmetics", Description = "Soft soap"},
            new ProductDto(){ProductId = 2001, Name = "Parle G", Price = 10, Category = "Bakery", Description = "G Genius"},
            new ProductDto(){ProductId = 3001, Name = "Potato", Price = 20, Category = "Vegitable", Description = "Fresh Veg"},
            new ProductDto(){ProductId = 4001, Name = "Cheese", Price = 110, Category = "Dairy", Description = "Cheezy"},
            new ProductDto(){ProductId = 5001, Name = "Milk", Price = 70, Category = "Dairy", Description = "Full cream milk"},
        };

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartHeader);

        _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartDetailsRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(cartDto.CartDetails.ToList());

        _productServiceMock.Setup(x => x.GetProducts()).ReturnsAsync(productDtos);
        var expectedCartTotal = 835;

        // Act
        var response = cartService.GetCart(1234).Result;

        // Assert
        Assert.True(response.IsSuccess);
        Assert.Equal(expectedCartTotal, response.Result.CartHeader.CartTotal);
        Assert.Equal((int)HttpStatusCode.OK, response.Status);
    }

    // [Fact]
    // public async Task GetCart_Should_FailedDueToException()
    // {
    //     // Arrange
    //     CartService cartService = new CartService(_mediatorMock.Object, _productServiceMock.Object);

    //     CartDto cartDto = new CartDto()
    //     {
    //         CartHeader = new CartHeaderDto()
    //         {
    //             CartHeaderId = 100,
    //             CartTotal = 10,
    //             ApartmentName = "Atria Grande",
    //             City = "Pune",
    //             UserId = 1234,
    //             Name = "B N"
    //         },
    //         CartDetails = new List<CartDetailsDto>()
    //         {
    //             new CartDetailsDto(){CartDetailsId = 1, CartHeaderId = 100, Count = 7, ProductId = 1001},
    //             new CartDetailsDto(){CartDetailsId = 2, CartHeaderId = 200, Count = 8, ProductId = 2001},
    //             new CartDetailsDto(){CartDetailsId = 3, CartHeaderId = 100, Count = 9, ProductId = 3001},
    //             new CartDetailsDto(){CartDetailsId = 4, CartHeaderId = 100, Count = 3, ProductId = 4001},
    //             new CartDetailsDto(){CartDetailsId = 5, CartHeaderId = 500, Count = 10, ProductId = 5001}
    //         },
    //     };

    //     IEnumerable<ProductDto> productDtos = new List<ProductDto>()
    //     {
    //         new ProductDto(){ProductId = 1001, Name = "Dove", Price = 45, Category = "Cosmetics", Description = "Soft soap"},
    //         new ProductDto(){ProductId = 2001, Name = "Parle G", Price = 10, Category = "Bakery", Description = "G Genius"},
    //         new ProductDto(){ProductId = 3001, Name = "Potato", Price = 20, Category = "Vegitable", Description = "Fresh Veg"},
    //         new ProductDto(){ProductId = 4001, Name = "Cheese", Price = 110, Category = "Dairy", Description = "Cheezy"},
    //         new ProductDto(){ProductId = 5001, Name = "Milk", Price = 70, Category = "Dairy", Description = "Full cream milk"},
    //     };

    //     _mediatorMock.Setup(x => x.Send(It.IsAny<GetCartHeaderRequest>(), It.IsAny<CancellationToken>()))
    //                 .ThrowsAsync(new Exception("Exception occur while retrieving Cart Header information"));

    //     // Act
    //     var response = cartService.GetCart(1234).Result;

    //     // Assert
    //     Assert.False(response.IsSuccess);
    //     Assert.Null(response.Result);
    //     Assert.Equal((int)HttpStatusCode.InternalServerError, response.Status);
    //     Assert.Contains("Exception occur while retrieving Cart Header information", response.Message);
    // }

}