

using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartDetails.Handlers.Commands;
using EasyGroceries.Cart.Application.Features.CartDetails.Requests.Commands;
using EasyGroceries.Cart.Application.Profiles;
using EasyGroceries.Cart.Domain;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class CreateCartDetailsRequestHandlerTests
{
    private readonly Mock<ICartDetailsRepository> _cartDetailsRepositoryMock;

    public CreateCartDetailsRequestHandlerTests()
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

        var command = new CreateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new CreateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_Should_ReturnFalseAsProductIsNull()
    {
        // Arrange
        CartDetailsDto cartDetailsDto = new CartDetailsDto()
        {
            CartDetailsId = 1,
            CartHeaderId = 100,
            Count = 10,
            ProductId = 100,
            Product = null
        };

        var command = new CreateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new CreateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_Should_ReturnFalseAsProductPriceIsLessThanZero()
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
                Price = -1,
                Description = "Creamy milk",
                ProductId = 100
            }
        };

        var command = new CreateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new CreateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task Handle_Should_ReturnTrueAsProductDetailsAddedSuccessfully()
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

        CartDetails cartDetails = new CartDetails()
        {
            CartDetailsId = cartDetailsDto.CartDetailsId,
            CartHeaderId = cartDetailsDto.CartHeaderId,
            Count = cartDetailsDto.Count,
            ProductId = cartDetailsDto.ProductId
        };

        var command = new CreateCartDetailsRequest() { CartDetailsDto = cartDetailsDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        List<CartDetails> cartDetailsLst = new List<CartDetails>();
        _cartDetailsRepositoryMock.Setup(x => x.AddCartDetails(cartDetails))
                .Callback(() =>
                {
                    cartDetailsLst.Add(cartDetails);
                });

        var handler = new CreateCartDetailsRequestHandler(_cartDetailsRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.True(result);
    }

}