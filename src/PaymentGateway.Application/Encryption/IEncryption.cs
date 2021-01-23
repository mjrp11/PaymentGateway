namespace PaymentGateway.Application.Encryption
{
    public interface ISymmetricEncryption
    {
        string Encrypt(string value);

        string Decrypt(string value);
    }
}