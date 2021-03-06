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
        Models.BloodDonationDBEntities6 context = new Models.BloodDonationDBEntities6();
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
            bool AdminisValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type  == "Admin");
            bool ModeratorValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "Moderator");

            bool DonorisValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "Donor");

            bool BanCheck = context.bannedUsers.Any(x => x.Email == Info.Email);



            if (AdminisValid || DonorisValid)
            {
                Session["Email"] = Info.Email;
            }

            if (AdminisValid && !BanCheck)
            {
                //FormsAuthentication.SetAuthCookie(Info.Email, false);
                Session["Type"] = "Admin";
                return RedirectToAction("Index" , "Admin" , new { email = Info.Email });
            }
            if (ModeratorValid && !BanCheck)
            {
                //FormsAuthentication.SetAuthCookie(Info.Email, false);
                Session["Type"] = "Moderator";
                //return RedirectToAction("Index", "Admin", new { email = Info.Email });
            }

            if (DonorisValid && !BanCheck)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
                //return RedirectToAction("Index");
            }

            if (BanCheck && (AdminisValid || ModeratorValid || DonorisValid))
            {
                TempData["errorLogin"] = "Your Account is Banned";
            }

            TempData["errorLogin"] = "Wrong Email or Password";
            Session["Type"] = "Admin";
            // return RedirectToAction("Login");
            return RedirectToAction("Index", "Admin", new { email = Info.Email });
        }
    }
}