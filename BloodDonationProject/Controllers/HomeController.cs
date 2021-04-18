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
        Models.BloodDonationDBEntities context = new Models.BloodDonationDBEntities();
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

            bool UserValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "User");

            bool BanCheck = context.bannedUsers.Any(x => x.Email == Info.Email);
            bool DisableCheck = context.DisabledAccounts.Any(x => x.Email == Info.Email);
            Session["Email"] = "";
            Session["Type"] = "";
            //Session["Email"] = Info.Email;
            if (AdminisValid || DonorisValid || ModeratorValid)
            {
                Session["Email"] = Info.Email;
              
            }

            if (BanCheck)
            {
                TempData["errorLogin"] = "Your Account is Banned.Please go to Contact Us page....";
                return RedirectToAction("Login");
            }
            if (DisableCheck)
            {
                TempData["errorLogin"] = "Your Account is Disabled";
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

            if (UserValid && !BanCheck)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
                var userid = context.userInfoes.Where(x => x.Email == Info.Email).FirstOrDefault().userID;
                Session["userid"] = userid;
                return RedirectToAction("Dashboard", "User");
            }

            if (DonorisValid && !BanCheck)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
               // return RedirectToAction("Dashboard", "User");   //oi tonmoy eikhane .....................
            }


            TempData["errorLogin"] = "Wrong Password or Email";
            //Session["Type"] = "Admin";
            return RedirectToAction("Login");
           // return RedirectToAction("Index", "Admin", new { email = Info.Email });
        }
    }
}