using System;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Shared;
using PaymentGateway.ViewModels;

namespace PaymentGateway.Extensions
{
    public static class PaymentRequestExtensions
    {
        public static ProcessPaymentRequest ToServiceRequest(this PaymentRequest paymentRequest)
        {
            return new ProcessPaymentRequest
            {
                Amount = paymentRequest.Amount,
                Currency = (Currency) Enum.Parse(typeof(Currency), paymentRequest.Currency.ToUpper()),
                CardNumber = paymentRequest.CardNumber,
                CVV = paymentRequest.CVV,
                ExpireDateMonth = paymentRequest.ExpireDateMonth,
                ExpireDateYear = paymentRequest.ExpireDateYear
            };
        }
    }
}