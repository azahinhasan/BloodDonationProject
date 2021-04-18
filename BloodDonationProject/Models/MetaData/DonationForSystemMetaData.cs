using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonationProject.Models.MetaData
{
    public class DonationForSystemMetaData
    {
        public int DonationId { get; set; }
        [Required, Display(Name = "Amount(Taka)")]
        public double Amount { get; set; }
        [Required, Display(Name = "Money Source")]
        public string MoneySource { get; set; }
        [Required]
        public string Name { get; set; }
        [Required, Display(Name = "Your Email")]
        public string UserEmail { get; set; }
        [Display(Name = "Your Message")]
        public string YourMessage { get; set; }
    }
}