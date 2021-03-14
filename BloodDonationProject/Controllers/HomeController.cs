using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Web.Security;


namespace BloodDonationProject.Controllers
{
    public class HomeController : Controller
    {
        Models.BloodDonationDBEntities10 context = new Models.BloodDonationDBEntities10();
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult Login()
        {
   
            return View();
        }

        [HttpPost]
        public ActionResult Login(userInfo Info)
        {
            bool AdminisValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password.ToString() && x.Type  == "Admin");
            bool ModeratorValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "Moderator");

            bool DonorisValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "Donor");

            bool BanCheck = context.bannedUsers.Any(x => x.Email == Info.Email);
            Session["Email"] = "";
            Session["Type"] = "";
            //Session["Email"] = Info.Email;
            if (AdminisValid || DonorisValid || ModeratorValid)
            {
                Session["Email"] = Info.Email;
              
            }

            if (BanCheck)
            {
                TempData["errorLogin"] = "Your Account is Banned";
                return RedirectToAction("Login");
            }

            if (AdminisValid && !BanCheck)
            {
                //FormsAuthentication.SetAuthCookie(Info.Email, false);
                Session["Type"] = "Admin";
                Session["ValidType"] = "AdMo";
                return RedirectToAction("Index" , "Admin");
            }
            if (ModeratorValid && !BanCheck)
            {
                //FormsAuthentication.SetAuthCookie(Info.Email, false);
                Session["Type"] = "Moderator";
                Session["ValidType"] = "AdMo";
                return RedirectToAction("Index", "Admin");
            }

            if (DonorisValid && !BanCheck)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
                //return RedirectToAction("Index");
            }


            TempData["errorLogin"] = "Wrong Password or Email";
            //Session["Type"] = "Admin";
            return RedirectToAction("Login");
           // return RedirectToAction("Index", "Admin", new { email = Info.Email });
        }
    }
}