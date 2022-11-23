using ACME.Service.Domain;
using ACME.Service.Service;
using Moq;

namespace ACME.Test
{
    public class Tests
    {
        private PaymentService paymentService;

        [SetUp]
        public void Setup()
        {
            var paymentConfig = GetPaymentConfiguration();
            paymentService = new PaymentService(paymentConfig);
        }

        [Test]
        public void PaymentServiceCorrectAmount()
        {
            var expected = "The amount to pay ASTRID is: 60 USD";
            var result = paymentService.GetPaymentByLog(GetRightStream(), "ASTRID");
            Assert.IsNotEmpty(result);
            Assert.That(expected, Is.EqualTo(result));
        }

        private Stream GetRightStream()
        {
            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.WriteLine("ASTRID=MO10:00-12:00,TH12:00-14:00");
            writer.WriteLine("SEBA=MO10:00-22:00,SU20:00-23:00");
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        private PaymentConfiguration GetPaymentConfiguration()
        {
            return new PaymentConfiguration
            {
                DaysAmountPayments = new List<DaysAmountPayments>
                {
                    new DaysAmountPayments()
                    {
                        DayFrom = Service.Constant.DaysEnum.MO,
                        DayTo = Service.Constant.DaysEnum.FR,
                        Ranges = new List<TimeRangeAmount>
                        { 
                            new TimeRangeAmount()
                            {
                                From = TimeOnly.Parse("00:00"),
                                To = TimeOnly.Parse("09:00"),
                                Amount = 25
                            },
                            new TimeRangeAmount()
                            {
                                From = TimeOnly.Parse("09:01"),
                                To = TimeOnly.Parse("18:00"),
                                Amount = 15
                            },
                            new TimeRangeAmount()
                            {
                                From = TimeOnly.Parse("18:01"),
                                To = TimeOnly.Parse("23:59"),
                                Amount = 20
                            }
                        }
                    }
                }
            };
        }
    }
}