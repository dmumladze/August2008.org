using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace August2008.Model
{
    public class DonationSearchCriteria
    {
        public DonationSearchCriteria()
            : this(DateTime.Now.AddYears(-1), DateTime.Now, null)
        {
        }
        public DonationSearchCriteria(DateTime fromDate, DateTime toDate)
            : this(fromDate, toDate, null)
        {
        }
        public DonationSearchCriteria(DateTime fromDate, DateTime toDate, string name)
        {
            FromDate = fromDate;
            ToDate = toDate;
            Name = name;
        }
        public int? UserId { get; set; }
        public string Name { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }

        public IEnumerable<Donation> Result { get; set; }
    }
}
