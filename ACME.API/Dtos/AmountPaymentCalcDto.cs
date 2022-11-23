using ACME.Crosscutting.Validators;

namespace ACME.API.Dtos
{
    public class AmountPaymentCalcDto
    {
        [ExtensionFileValidator(extensions: new string[] { ".txt" })]
        public IFormFile File { get; set; }

        public string Name { get; set; }
    }
}
