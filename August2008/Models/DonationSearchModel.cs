using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Resources.Donations;

namespace August2008.Models
{
    public class DonationSearchModel
    {
        public DonationSearchModel()
        {
            Result = new List<DonationModel>();
        }
        public int? UserId { get; set; }
        [Display(Name = "Name", ResourceType = typeof(Labels))]
        public string Name { get; set; }
        [Display(Name = "FromDate", ResourceType = typeof(Labels))]
        public DateTime? FromDate { get; set; }
        [Display(Name = "ToDate", ResourceType = typeof(Labels))]
        public DateTime? ToDate { get; set; }

        public IEnumerable<DonationModel> Result { get; set; }

        public bool ConfirmDonation { get; set; } 
    }
}