using EasyGroceries.Cart.Application.DTOs;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyGroceries.Cart.Application.Validators
{
    public class CartDetailsDtoValidator : AbstractValidator<CartDetailsDto>
    {
        public CartDetailsDtoValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThanOrEqualTo(0)
                .WithMessage("{PropertyName} must be geater than equal to 0");

            RuleFor(x => x.Product)
                .NotNull()
                .WithMessage("{PropertyName} should not be null");

            RuleFor(x => x.Product)
                .NotNull()
                .SetValidator(new ProductDtoValidator());
        }
    }
}
