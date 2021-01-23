using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using PaymentGateway.ViewModels;

namespace PaymentGateway.Client
{
    public class PaymentGatewayClient
    {
        private readonly string _baseUrl;
        private readonly HttpClient _httpClient;
        private static JsonSerializerOptions _options = new JsonSerializerOptions {PropertyNameCaseInsensitive = true};

        public PaymentGatewayClient(string baseUrl)
        {
            _baseUrl = baseUrl;

            _httpClient = new HttpClient {BaseAddress = new Uri(_baseUrl)};
        }
        
        public PaymentGatewayClient(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<PaymentResponse> ProcessPayment(PaymentRequest request)
        {
            var apiResponse = await _httpClient.PostAsync("PaymentGateway",
                new StringContent(JsonSerializer.Serialize(request), Encoding.Default, "application/json"));

            var response = await apiResponse.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<PaymentResponse>(response, _options);
        }
        
        public async Task<PaymentInformation> GetPayment(Guid paymentId)
        {
            var apiResponse = await _httpClient.GetAsync($"PaymentGateway/{paymentId}");
            
            var response = await apiResponse.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<PaymentInformation>(response, _options);
        }
    }
}