namespace ACME.Service.Service
{
    public interface IPaymentService
    {
        string GetPayment(string line, string name);
    }
}