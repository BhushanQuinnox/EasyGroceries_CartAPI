using AutoMapper;
using EasyGroceries.Cart.Application.Contracts.Infrastructure;
using EasyGroceries.Cart.Application.Features.CartHeader.Handlers.Queries;
using EasyGroceries.Cart.Application.Features.CartHeader.Requests.Queries;
using EasyGroceries.Cart.Application.Profiles;
using EasyGroceries.Cart.Domain;
using Moq;

namespace EasyGroceries.Cart.Application.Tests;

public class GetCartHeaderRequestHandlerTests
{
    private readonly Mock<ICartHeaderRepository> _cartHeaderRepositoryMock;
    private List<CartHeader> _cartHeaderLst;

    public GetCartHeaderRequestHandlerTests()
    {
        _cartHeaderRepositoryMock = new Mock<ICartHeaderRepository>();
    }

    private void IntializeCartHeaderData()
    {
        _cartHeaderLst = new List<CartHeader>()
        {
            new CartHeader(){CartHeaderId = 1, CartTotal = 100, LoyaltyMembershipOpted = false, UserId = 1000},
            new CartHeader(){CartHeaderId = 2, CartTotal = 225, LoyaltyMembershipOpted = true, UserId = 2003},
            new CartHeader(){CartHeaderId = 3, CartTotal = 350, LoyaltyMembershipOpted = true, UserId = 3389},
            new CartHeader(){CartHeaderId = 4, CartTotal = 400, LoyaltyMembershipOpted = false, UserId = 4567}
        };
    }

    [Fact]
    public async Task Handle_Should_ReturnsCartHeaderByUserId()
    {
        // Arrange
        IntializeCartHeaderData();
        int userId = 2003;
        var command = new GetCartHeaderRequest() { UserId = userId };
        var cartHeader = _cartHeaderLst.FirstOrDefault(x => x.UserId == userId);

        _cartHeaderRepositoryMock.Setup(x => x.GetCartHeaderByUserId(userId))
                .ReturnsAsync(cartHeader);

        var mockMapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new MappingProfile());
        });

        var handler = new GetCartHeaderRequestHandler(_cartHeaderRepositoryMock.Object, mockMapper.CreateMapper());

        // Act
        var result = await handler.Handle(command, default);

        // Assert
        result.CartTotal.Equals(225);
        Assert.True(result.LoyaltyMembershipOpted);
    }
}