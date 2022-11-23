using ACME.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace ACME.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        public string Data = "ASTRID=MO10:00-12:00,TH12:00-14:00,SU20:00-21:00\r\n\r\n";
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet]
        public void Get()
        {
            _paymentService.GetPayment(Data, "RENE");
        }
    }
}
