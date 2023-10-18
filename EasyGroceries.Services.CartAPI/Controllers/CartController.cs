using EasyGroceries.Cart.Application.Contracts.Services;
using EasyGroceries.Cart.Application.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EasyGroceries.Services.CartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<ActionResult<ResponseDto<CartDto>>> GetCart(int userId)
        {
            var response = await _cartService.GetCart(userId);
            if (response.Result == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpPost("CartUpsert")]
        public async Task<ActionResult<ResponseDto<CartDto>>> CartUpsert([FromBody] CartDto cartDto)
        {
            var response = await _cartService.CartUpsert(cartDto);
            return Ok(response);
        }
    }
}
