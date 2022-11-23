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
            var totalAmount = 0;
            foreach (var workingDay in workingDays)
            {
                if (!Enum.TryParse(typeof(DaysEnum), workingDay[..2], out var day))
                    throw new Exception("Incorrect format");

                var times = workingDay.Split("-");

                timeFrom = TimeOnly.Parse(times[0][2..]);
                timeTo = TimeOnly.Parse(times[1]);
                var config = GetDaysAmountPayments((DaysEnum)day);
                totalAmount += CalculateAmountByDay(config, timeFrom, timeTo);
            }

            return totalAmount;
        }

        private DaysAmountPayments GetDaysAmountPayments(DaysEnum day)
        {
            return _paymentConfiguration
                .DaysAmountPayments
                .First(x => (DaysEnum)day >= x.DayFrom && (DaysEnum)day <= x.DayTo);
        }

        private int CalculateAmountByDay(DaysAmountPayments dayAmountPayment, TimeOnly timeFrom, TimeOnly timeTo)
        {
            var amount = 0;
            
            foreach(var range in dayAmountPayment.Ranges) 
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
                    if(timeTo > range.To)
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
