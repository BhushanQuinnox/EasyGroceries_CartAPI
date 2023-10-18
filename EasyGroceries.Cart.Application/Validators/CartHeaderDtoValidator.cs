using EasyGroceries.Cart.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Validators
{
    public class CartHeaderDtoValidator : AbstractValidator<CartHeaderDto>
    {
        public CartHeaderDtoValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0)
                .WithMessage("{PropertyName} must be greater than 0");
        }
    }
}
