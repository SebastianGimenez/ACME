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

        public string GetPaymentByLog(Stream stream, string name)
        {
            int amount = 0;
            using (StreamReader sr = new StreamReader(stream))
            {
                while (sr.Peek() >= 0)
                {
                    var line = sr.ReadLine();
                    if (!string.IsNullOrEmpty(line) && line.StartsWith(name))
                        amount += GetAmount(line.Split("=")[1]);
                }
            }

            return $"The amount to pay {name} is: {amount} USD";
        }

        private int GetAmount(string line)
        {
            var workingDays = line.Split(",");
            var totalAmount = 0;
            TimeOnly timeFrom;
            TimeOnly timeTo;
            foreach (var workingDay in workingDays)
            {
                if (!Enum.TryParse(typeof(DaysEnum), workingDay[..2], out var day))
                    throw new Exception("Incorrect format");

                var times = workingDay.Split("-");

                timeFrom = TimeOnly.Parse(times[0][2..]);
                timeTo = TimeOnly.Parse(times[1]);
                var config = GetDaysAmountPayments((DaysEnum)day);
                totalAmount += CalculateAmountByRange(config, timeFrom, timeTo);
            }

            return totalAmount;
        }

        private DaysAmountPayments GetDaysAmountPayments(DaysEnum day)
        {
            return _paymentConfiguration
                .DaysAmountPayments
                .First(x => (DaysEnum)day >= x.DayFrom && (DaysEnum)day <= x.DayTo);
        }

        private int CalculateAmountByRange(DaysAmountPayments dayAmountPayment, TimeOnly timeFrom, TimeOnly timeTo)
        {
            var amount = 0;

            foreach (var range in dayAmountPayment.Ranges)
            {
                if (timeFrom >= range.From && timeFrom <= range.To)
                {
                    if (timeTo > range.To)
                        amount += (int)(range.To - timeFrom).TotalHours * range.Amount;

                    if (timeTo <= range.To)
                    {
                        amount += (int)(timeTo - timeFrom).TotalHours * range.Amount;
                        break;
                    }
                }

                if (timeFrom < range.From && timeTo > range.From)
                {
                    if (timeTo > range.To)
                        amount += (int)(range.To - range.From).TotalHours * range.Amount;

                    if (timeTo >= range.To)
                    {
                        amount += (int)(range.To - range.From).TotalHours * range.Amount;
                        break;
                    }
                }
            }

            return amount;
        }
    }
}
