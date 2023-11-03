using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Handlers.Commands;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Profiles;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class UpdateCartDetailsRequestHandlerTests
{
    private readonly Mock<ICartDetailsRepository> _cartDetailsRepositoryMock;

    public UpdateCartDetailsRequestHandlerTests()
    {
        _cartDetailsRepositoryMock = new Mock<ICartDetailsRepository>();
    }

    [Fact]
    public async Task Handle_Should_ReturnFalseAsCountIsLessThanZero()
    {
        // Arrange
        CartDetailsDto cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 100,
            Count = -1,
            ProductId = 100,
            Product = new ProductDto()
            {
                Name = "Amul Milk",
                Category = "Dairy Product",
                Price = 70,
                Description = "Creamy milk",
                ProductId = 100
            }
        };

        var command = new UpdateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new UpdateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrueAsCartDetailsUpdatedSuccessfully()
    {
        // Arrange
        CartDetailsDto cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 100,
            Count = 10,
            ProductId = 100,
            Product = new ProductDto()
            {
                Name = "Amul Milk",
                Category = "Dairy Product",
                Price = 70,
                Description = "Creamy milk",
                ProductId = 100
            }
        };

        var command = new UpdateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new UpdateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result);
    }
}