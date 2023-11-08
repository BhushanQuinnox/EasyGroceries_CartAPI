using System.Net;
using AutoFixture;
using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Services.CartAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace EasyGroceries.Services.CartAPI.Tests;

public class CartControllerTests
{
    private readonly Mock<ICartService> _cartServiceMock;

    public CartControllerTests()
    {
        _cartServiceMock = new Mock<ICartService>();
    }

    [Fact]
    public async Task GetCart_Should_ReturnCartForSpecifiedUserId()
    {
        // Arrange
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

        ResponseDto<CartDto> expectedResponse = new ResponseDto<CartDto>()
        {
            Result = cartDto,
            Status = (int)HttpStatusCode.OK
        };

        _cartServiceMock.Setup(x => x.GetCart(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        // Act
        CartController cartController = new CartController(_cartServiceMock.Object);
        var actualResponse = await cartController.GetCart(1234);
        var responseContent = actualResponse.Result as OkObjectResult;
        // Assert
        Assert.NotNull(responseContent);
    }

    [Fact]
    public async Task GetCart_Should_ReturnNotFoundStatusCodeAsNoCartData()
    {
        // Arrange
        ResponseDto<CartDto> expectedResponse = new ResponseDto<CartDto>()
        {
            Result = null,
            Status = (int)HttpStatusCode.NotFound
        };

        _cartServiceMock.Setup(x => x.GetCart(It.IsAny<int>())).ReturnsAsync(expectedResponse);

        // Act
        CartController cartController = new CartController(_cartServiceMock.Object);
        var actualResponse = await cartController.GetCart(1234);
        var responseContent = actualResponse.Result as NotFoundObjectResult;

        // Assert
        Assert.Null(responseContent);
    }

    [Fact]
    public async Task CartUpsert_Should_CreateCartSuccessfully()
    {
        // Arrange
        var fixture = new Fixture();
        var cartDto = fixture.Create<CartDto>();

        ResponseDto<CartDto> expectedResponse = new ResponseDto<CartDto>()
        {
            Result = cartDto
        };

        _cartServiceMock.Setup(x => x.CartUpsert(It.IsAny<CartDto>())).ReturnsAsync(expectedResponse);

        // Act
        CartController cartController = new CartController(_cartServiceMock.Object);
        var actualResponse = await cartController.CartUpsert(cartDto);

        // Assert
        Assert.NotNull(actualResponse);
    }
}