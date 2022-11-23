using ACME.Service.Constant;

namespace ACME.Service.Domain
{
    public class DaysAmountPayments
    {
        public DaysEnum DayFrom { get; set; }
        
        public DaysEnum DayTo { get; set; }

        public ICollection<TimeRangeAmount> Ranges { get; set; }
    }
}
