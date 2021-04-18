using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BloodDonationProject.Models.MetaData
{
    public class PostMetaData
    {
        public int PostID { get; set; }
        public int UserId { get; set; }
        [Required, MinLength(5), MaxLength(30)]
        public string Address { get; set; }
        [Required, Display(Name ="Wanted Blood Group")]
        public string WantedBlood { get; set; }
        [Required, MinLength(5, ErrorMessage ="Text is not sufficient")]
        public string Text { get; set; }
       

        public virtual Post Posts1 { get; set; }
        public virtual Post Post1 { get; set; }
        public virtual userInfo userInfo { get; set; }
    }
}