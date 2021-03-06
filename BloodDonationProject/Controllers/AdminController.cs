using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Web.Security;

namespace BloodDonationProject.Controllers
{
    public class AdminController : Controller
    {
        Models.BloodDonationDBEntities3 context = new Models.BloodDonationDBEntities3();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(userInfo info)
        {

            context.userInfoes.Add(info);
            context.SaveChanges();
            return RedirectToAction("Create");
        }
    }
}