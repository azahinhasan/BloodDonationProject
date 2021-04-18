using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Repository;
using BloodDonationProject.Models;


namespace BloodDonationProject.Controllers.User
{
    public class DonationForSystemController : Controller
    {
        DonationForSystemRepostiry donationRepository = new DonationForSystemRepostiry();

        // GET: DonationForSystem
        public ActionResult Index()
        {
            if (Session["donationInfo"] != null)
            {
                Session["donationInfo"] = null;
                Session.Remove("donationInfo");
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult AddFund()
        {
            //UserEmail YourMessage
            if (Session["userid"] != null)
            {
                BloodDonationDBEntities7 context = new BloodDonationDBEntities7();
                ViewData["loggedUserInfo"] = context.userInfoes.Find((int)Session["userid"]);
            }
           
            return View();
        }

        [HttpPost]
        public ActionResult AddFund(DonationForSystem donation)
        {
            if (ModelState.IsValid)
            {
                Session["donationInfo"] = donation;
                return RedirectToAction("FundMedium");
            }
            return View();
        }


        [HttpGet]
        public ActionResult FundMedium()
        {
            if (true)
            {

            }
            return View(Session["donationInfo"]);
        }

        [HttpPost, ActionName("FundMedium")]
        public ActionResult ConfirmFundMedium(FormCollection form)
        {
            if (form["MobileNumber"] == "")
            {
                ViewData["ErrorMsg"] = "Please Enter Your Mobile Number";
                return View(Session["donationInfo"]);
            }
            else if (form["MobileNumber"].Length < 11)
            {
                ViewData["ErrorMsg"] = "Mobile Number is not valid";
                return View(Session["donationInfo"]);
            }
            else if(form["pin"] == "")
            {
                ViewData["ErrorMsg"] = "Please Enter PIN Number";
                return View(Session["donationInfo"]);
            }
            else if (form["pin"].Length < 4)
            {
                ViewData["ErrorMsg"] = "Invalid PIN Number";
                return View(Session["donationInfo"]);
            }

            DonationForSystem donation = (DonationForSystem) Session["donationInfo"];
            donationRepository.Insert(donation);
            TempData["thanks"] = "Thanks for Support!! Best Wishes :)";
            return RedirectToAction("Index", "DonationForSystem");
        }
    }
}