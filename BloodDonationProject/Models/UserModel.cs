using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BloodDonationProject.Models
{
    public class UserModel
    {
        public bool CheckGuestUser()
        {
            object ob = HttpContext.Current.Session["userid"];
            if (ob == null)
            {
                return true;
            }
            return false;
            
        }
    }
}