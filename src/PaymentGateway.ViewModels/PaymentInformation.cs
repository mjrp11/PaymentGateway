using System;
using System.Text.Json.Serialization;

namespace PaymentGateway.ViewModels
{
    public class PaymentInformation
    {
        public Guid PaymentId { get; set; }
        
        public string CardNumber { get; set; }
        
        public string CardExpireDate { get; set; }
        
        public decimal Amount { get; set; }
        
        public string Currency { get; set; }
        
        public DateTime PaymentDate { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatus PaymentStatus { get; set; }
    }
}