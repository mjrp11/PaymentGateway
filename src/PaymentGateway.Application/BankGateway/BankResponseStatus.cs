namespace PaymentGateway.Application.BankGateway
{
    public enum BankResponseStatus
    {
        Success,
        LackOfFunds,
        InvalidCard,
        UnexpectedError
    }
}