using ACME.Service.Constant;
using ACME.Service.Domain;

namespace ACME.Service.Service
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentConfiguration _paymentConfiguration;

        public PaymentService(PaymentConfiguration paymentConfiguration)
        {
            _paymentConfiguration = paymentConfiguration;
        }

        public string GetPayment(string line, string name)
        {
            //filter file by name
            //starts with

            line = line.Split("=")[1];
            var valor = GetAmount(line);
            return $"The amount to pay {name} is: {valor}";
        }

        private int GetAmount(string line)
        {
            var workingDays = line.Split(",");
            TimeOnly timeFrom;
            TimeOnly timeTo;
            foreach (var workingDay in workingDays)
            {
                if (!Enum.TryParse(typeof(DaysEnum), workingDay[..2], out var day))
                    throw new Exception("Incorrect format");

                var times = workingDay.Split("-");

                timeFrom = TimeOnly.Parse(times[0][2..]);
                timeTo = TimeOnly.Parse(times[0][2..]);
                var config = GetDaysAmountPayments((DaysEnum)day);


            }

            return 0;
        }

        private DaysAmountPayments GetDaysAmountPayments(DaysEnum day)
        {
            return _paymentConfiguration
                .DaysAmountPayments
                .First(x => (DaysEnum)day >= x.DayFrom && (DaysEnum)day <= x.DayTo);
        }

        private int GetAmount(DaysAmountPayments dayAmountPayment, TimeOnly timeFrom, TimeOnly timeTo)
        {

            return 0;
        }
    }
}
