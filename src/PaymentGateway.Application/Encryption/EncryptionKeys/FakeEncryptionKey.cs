using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Application.Encryption.EncryptionKeys
{
    public class FakeEncryptionKey: IEncryptionKeys
    {
        private readonly IConfiguration _configuration;

        public FakeEncryptionKey(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetKey()
        {
            return _configuration["Encryption:Key"];
        }
    }
}