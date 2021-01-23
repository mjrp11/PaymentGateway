using Amazon.SimpleSystemsManagement;
using Amazon.SimpleSystemsManagement.Model;
using Microsoft.Extensions.Configuration;

namespace PaymentGateway.Application.Encryption.EncryptionKeys
{
    public class SSMEncryptionKey: IEncryptionKeys
    {
        private readonly AmazonSimpleSystemsManagementClient _amazonSimpleSystemsManagementClient;
        private readonly IConfiguration _configuration;

        public SSMEncryptionKey(
            AmazonSimpleSystemsManagementClient amazonSimpleSystemsManagementClient,
            IConfiguration configuration)
        {
            _amazonSimpleSystemsManagementClient = amazonSimpleSystemsManagementClient;
            _configuration = configuration;
        }

        public string GetKey()
        {
            return _amazonSimpleSystemsManagementClient.GetParameterAsync(new GetParameterRequest
            {
                Name = _configuration["Encryption:SSM_Path"]
            }).GetAwaiter().GetResult().Parameter.Value;
        }
    }
}