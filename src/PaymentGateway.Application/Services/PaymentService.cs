using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PaymentGateway.Application.BankGateway;
using PaymentGateway.Application.Encryption;
using PaymentGateway.Application.Extensions;
using PaymentGateway.Application.Requests;
using PaymentGateway.Application.Responses;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Repositories;

namespace PaymentGateway.Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentsRepository _paymentsRepository;
        private readonly ISymmetricEncryption _symmetricEncryption;
        private readonly ICardsRepository _cardsRepository;
        private readonly IBankGateway _bankGateway;

        public PaymentService(
            IPaymentsRepository paymentsRepository, 
            ISymmetricEncryption symmetricEncryption,
            ICardsRepository cardsRepository,
            IBankGateway bankGateway)
        {
            _paymentsRepository = paymentsRepository;
            _symmetricEncryption = symmetricEncryption;
            _cardsRepository = cardsRepository;
            _bankGateway = bankGateway;
        }

        public async Task<ProcessPaymentResponse> ProcessPayment(ProcessPaymentRequest request)
        {
            var card = GetCardFromRequest(request);
            var payment = CreatePayment(card, request.Amount, request.Currency.ToString());

            var bankResponse = await _bankGateway.ValidatePayment(payment);

            UpdatePaymentInformationFromBankResponse(payment, bankResponse);
            
            return new ProcessPaymentResponse
            {
                PaymentStatus = payment.Status.ToApplicationPaymentStatus(),
                PaymentId = payment.UniqueId
            };
        }
        
        public async Task<PaymentResponse> GetPaymentInformation(Guid paymentId)
        {
            var payment = _paymentsRepository.GetPayment(paymentId);
            
            return payment == null ? null : new PaymentResponse
            {
                PaymentId = payment.UniqueId,
                PaymentDate = payment.DateCreated,
                Amount = payment.Amount,
                Currency = payment.Currency,
                CardMask = payment.Card.CardNumberMask,
                CardExpirationDate = $"{payment.Card.ExpireDateMonth}/{payment.Card.ExpireDateYear}",
                Status = payment.Status.ToApplicationPaymentStatus()
            };
        }

        private void UpdatePaymentInformationFromBankResponse(Payment payment, BankResponse bankResponse)
        {
            payment.BankPaymentIdentifier = bankResponse.PaymentIdentifier;
            payment.Status = bankResponse.Status == BankResponseStatus.Success
                ? PaymentStatus.Processed
                : PaymentStatus.FailedOnBankRequest;
            
            _paymentsRepository.Save(payment);
        }

        private Payment CreatePayment(Card card, decimal amount, string currency)
        {
            var payment = new Payment
            {
                Amount = amount,
                Currency = currency,
                Card = card,
                Status = PaymentStatus.PendingBankRequest,
                UniqueId = Guid.NewGuid(),
                DateCreated = DateTime.UtcNow,
            };

            _paymentsRepository.Save(payment);
            return payment;
        }

        private Card GetCardFromRequest(ProcessPaymentRequest request)
        {
            var cardNumberEncrypted = _symmetricEncryption.Encrypt(request.CardNumber);
            var cvvEncrypted = _symmetricEncryption.Encrypt(request.CVV);

            var card = _cardsRepository.GetCard(cardNumberEncrypted, cvvEncrypted, request.ExpireDateMonth,
                request.ExpireDateYear);

            if (card == null)
            {
                card = new Card
                {
                    CardNumber = cardNumberEncrypted,
                    CardNumberMask = GetMask(request.CardNumber),
                    CVV = cvvEncrypted,
                    ExpireDateMonth = request.ExpireDateMonth,
                    ExpireDateYear = request.ExpireDateYear
                };
            }

            return card;
        }

        private string GetMask(string value)
        {
            return new string('*', value.Length - 4) + value.Substring(value.Length - 4);
        }
    }
}