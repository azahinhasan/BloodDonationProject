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
        Models.BloodDonationDBEntities3 context = new Models.BloodDonationDBEntities3();
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
            bool DonorisValid = context.userInfoes.Any(x => x.Email == Info.Email && x.Password == Info.Password && x.Type == "Donor");


            if (AdminisValid || DonorisValid)
            {
                Session["Email"] = Info.Email;
            }

            if (AdminisValid)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
                return RedirectToAction("Index" , "Admin" , new { email = Info.Email });
            }

            if (DonorisValid)
            {
                FormsAuthentication.SetAuthCookie(Info.Email, false);
                TempData["errorLogin"] = "solved";
                //return RedirectToAction("Index");
            }

            TempData["errorLogin"] = "Wrong Email or Password";
            return RedirectToAction("Login");
        }
    }
}