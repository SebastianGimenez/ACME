using ACME.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace ACME.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        public string Data = "RENE=MO10:00-12:00,TU10:00-12:00,TH01:00-03:00,SA14:00-18:00,SU20:00-21:00";
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
