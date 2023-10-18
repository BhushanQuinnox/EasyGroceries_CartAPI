using EasyGroceries.Cart.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Validators
{
    public class CartDtoValidator : AbstractValidator<CartDto>
    {
        public CartDtoValidator()
        {
            RuleFor(x => x.CartHeader.UserId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0");

            RuleForEach(x => x.CartDetails)
                .SetValidator(new CartDetailsDtoValidator());
        }
    }
}
