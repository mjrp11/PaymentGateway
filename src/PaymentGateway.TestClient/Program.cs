using System;
using System.Threading.Tasks;
using PaymentGateway.Client;
using PaymentGateway.ViewModels;

namespace PaymentGateway.TestClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var paymentGatewayClient = new PaymentGatewayClient("http://localhost:2345/");

            var request = new PaymentRequest
            {
                CardNumber = "4111111111111111",
                ExpireDateYear = 2021,
                ExpireDateMonth = 10,
                CVV = "123",
                Amount = (decimal) 10.10,
                Currency = "EUR"
            }; 
            
            var payment = await paymentGatewayClient.ProcessPayment(request);

            var paymentInfo = await paymentGatewayClient.GetPayment(payment.PaymentId);

            if (payment.Status != paymentInfo.PaymentStatus)
            {
                throw new Exception("not same values!");
            }

            if (!paymentInfo.CardExpireDate.Equals("10/2021"))
            {
                throw new Exception("Invalid expiration date!");
            }
        }
    }
}