using System;
using System.Collections.Generic;
using FluentValidation;
using PaymentGateway.Application.Shared;
using PaymentGateway.ViewModels;

namespace PaymentGateway.Validators
{
    public class PaymentRequestValidator: AbstractValidator<PaymentRequest>
    {
        public PaymentRequestValidator()
        {
            RuleFor(x => x.Currency.ToUpper())
                .NotNull()
                .IsEnumName(typeof(Currency))
                .WithMessage("Invalid Currency");

            RuleFor(x => x.CardNumber)
                .NotNull()
                .CreditCard()
                .WithMessage("Is not a valid card number!");
            
            /*
             * FROM: https://www.experian.com/blogs/ask-experian/what-is-a-credit-card-cvv/#:~:text=The%20CVV%20for%20Visa%2C%20Mastercard,card%20identification%20number%20(CID).
             * CVV is a 3 or 4 digit number
             */
            RuleFor(x => x.CVV)
                .NotNull()
                .MinimumLength(3)
                .MaximumLength(4)
                .Must(cvv => int.TryParse(cvv, out _))
                .WithMessage("Invalid CVV number!");

            RuleFor(x => x.ExpireDateMonth)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(12)
                .WithMessage("Invalid Month");
        }
    }
}