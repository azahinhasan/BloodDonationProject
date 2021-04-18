using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonationProject.Models.MetaData
{
    public class BloodBookMetaData
    {
        public int BookId { get; set; }
        [Required, Display(Name ="Name")]
        public string BookName { get; set; }
        [Required, Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }
        [Required, Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        [Required, Display(Name = "Relation with you")]
        public string Relation { get; set; }
        [Required]
        public int UserId { get; set; }

        public virtual userInfo userInfo { get; set; }
    }
}