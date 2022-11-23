using ACME.API.Dtos;
using ACME.Service.Service;
using Microsoft.AspNetCore.Mvc;

namespace ACME.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpPost()]
        public ActionResult<string> CalculateAmount([FromForm] AmountPaymentCalcDto dto)
        {
            return Ok(_paymentService.GetPaymentByLog(dto.File.OpenReadStream(), dto.Name));
        }
    }
}
