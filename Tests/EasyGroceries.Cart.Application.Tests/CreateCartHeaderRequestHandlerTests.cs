

using System.Net;
using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.DTOs;
using EasyGroceries.Cart.Application.Features.CartHeader.Handlers.Commands;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Commands;
using EasyGroceries.Cart.Application.Profiles;
using EasyGroceries.Cart.Domain;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class CreateCartHeaderRequestHandlerTests
{
    private readonly Mock<ICartHeaderRepository> _cartHeaderRepositoryMock;
    private List<CartHeader> _cartHeaderLst;

    public CreateCartHeaderRequestHandlerTests()
    {
        _cartHeaderRepositoryMock = new Mock<ICartHeaderRepository>();
    }

    [Fact]
    public async Task Handle_Should_ReturnBadRequestAsUserIdIsZero()
    {
        // Arrange
        CartHeaderDto cartHeaderDto = new CartHeaderDto()
        {
            CartHeaderId = 1,
            CartTotal = 5,
            LoyaltyMembershipOpted = true,
            UserId = 0
        };

        var command = new CreateCartHeaderRequest() { CartHeaderDto = cartHeaderDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new CreateCartHeaderRequestHandler(_cartHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.Equal((int)HttpStatusCode.BadRequest, result.Status);
        Assert.False(result.IsSuccess);
        Assert.Equal("Validation failed", result.Message);
    }

    [Fact]
    public async Task Handle_Should_CreateCartHeaderSuccessfully()
    {
        // Arrange
        CartHeaderDto cartHeaderDto = new CartHeaderDto()
        {
            CartHeaderId = 1,
            CartTotal = 5,
            LoyaltyMembershipOpted = true,
            UserId = 1234
        };

        CartHeader cartHeader = new CartHeader()
        {
            CartHeaderId = cartHeaderDto.CartHeaderId,
            CartTotal = cartHeaderDto.CartTotal,
            LoyaltyMembershipOpted = cartHeaderDto.LoyaltyMembershipOpted,
            UserId = cartHeaderDto.UserId
        };

        var command = new CreateCartHeaderRequest() { CartHeaderDto = cartHeaderDto };

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        _cartHeaderRepositoryMock.Setup(x => x.Add(cartHeader))
                .Callback(() =>
                {
                    _cartHeaderLst.Add(cartHeader);
                });

        var handler = new CreateCartHeaderRequestHandler(_cartHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        Assert.Equal((int)HttpStatusCode.Created, result.Status);
        Assert.True(result.IsSuccess);
        Assert.Equal("CartHeader Creation Successful", result.Message);
    }

}