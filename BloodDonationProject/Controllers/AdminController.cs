using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Web.Security;
using Rotativa;



namespace BloodDonationProject.Controllers
{
    public class AdminController : Controller
    {
        Models.BloodDonationDBEntities6 context = new Models.BloodDonationDBEntities6();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

    
        public ActionResult AfterReg(string email)
        {
            return View(context.userInfoes.Where(r => r.Email == email));
        }

        public ActionResult showReports()
        {
            return View(context.reports.ToList());
        }

        public ActionResult RepoterInfo(int id )
        {
            return View(context.userInfoes.Where(r => r.userID == id));
        }

        public ActionResult RepoterHistory(int id)
        {
            return View(context.reports.Where(r => r.DonorId == id));
        }

     
        public ActionResult AdnModList()
        {


            //var data = context.userInfoes.Where(r => r.Type == "Admin").FirstOrDefault<userInfo>();
            //Moderator

            return View(context.userInfoes.Where(r => r.Type == "Admin" | r.Type == "Moderator"));
        }

        public ActionResult typeChage(int id)
        {


            var data = context.userInfoes.Where(r => r.userID == id).FirstOrDefault<userInfo>();
            //Moderator

            if (data.Type == "Moderator")
            {
                data.Type = "Admin";
            }
            else
            {
                data.Type = "Moderator";
            }
            
            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
            return RedirectToAction("AdnModList");
        }

        [HttpGet]
        public ActionResult BanUser(int id)
        {
            DateTime utcDate = DateTime.Today;
            bannedUser bn = new bannedUser();

            var data = context.userInfoes.Where(r => r.userID == id).FirstOrDefault<userInfo>();

            bn.Email = data.Email;
            bn.Name = data.Name;
            bn.duration = 0;
            bn.BannedDate = utcDate;

            if (data.BanStatus!="true")
            {
                context.bannedUsers.Add(bn);
                context.SaveChanges();
                data.BanStatus = "true";
                context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            }


            context.SaveChanges();
            return RedirectToAction("showReports");
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(userInfo info)
        {
            var rand = new Random();
            bool EmailAlreadyAdded = context.userInfoes.Any(x => x.Email == info.Email);
            bool PassAlreadyAdded = context.userInfoes.Any(x => x.Password == info.Password);

            if (EmailAlreadyAdded)
            {
                TempData["EmailExist"] = "Email Already SignUp";
            }
            if (PassAlreadyAdded)
            {
                TempData["PasswordExist"] = "Password Already SignUp";
            }
            if (!EmailAlreadyAdded && !PassAlreadyAdded)
            {
                TempData["DoneReg"] = "New User Added";
                info.Password = rand.Next(50, 101).ToString();
                context.userInfoes.Add(info);
                context.SaveChanges();
                return RedirectToAction("AfterReg",new { info.Email });

            }
            return View();
        }

 

        [HttpGet]

        public ActionResult Print()
        {
            return new ActionAsPdf("AfterReg")
            {
                FileName = Server.MapPath("~/App_Data/ListProduct.pdf")
            };
        }

       

     
    }

       
       
    }
