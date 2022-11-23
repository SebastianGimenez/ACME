namespace ACME.Service.Service
{
    public interface IPaymentService
    {
        string GetPaymentByLog(Stream stream, string name);
    }
}