using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Web.Security;
using Rotativa;
using System.IO;


namespace BloodDonationProject.Controllers
{
    public class AdminController : Controller
    {
        Models.BloodDonationDBEntities9 context = new Models.BloodDonationDBEntities9();
        // GET: Admin
        public ActionResult Index()
        {
            if(Session["Email"] == null || Session["Type"].ToString() != "Admin")
            {
                return RedirectToAction("Login", "Home");
            }

            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            Session["DarkMood"] = data.darkMood;
            return View();
        }

    
        public ActionResult AfterReg(string email)
        {

            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            return View(context.userInfoes.Find(data.userID));
        }

        public ActionResult showReports()
        {
            return View(context.reports.ToList());
        }

        public ActionResult RepoterInfo(int id,string email )
        {
            //var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            bool BanCheck = context.bannedUsers.Any(x => x.Email == email);

            if (BanCheck)
            {
                Session["userban"] = true;
            }
            else
            {
                Session["userban"] = false;
            }

            ViewData["Reports"] = context.reports.Where(r => r.DonorId == id).ToList();

            return View(context.userInfoes.Where(r => r.userID == id));
        }
        public ActionResult RepoterInfoFilter(string email)
        {

            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            return RedirectToAction("RepoterInfo",new {id=data.userID, email = email });
        }
        public ActionResult banUsersList()
        {
            return View(context.bannedUsers.ToList());
        }

        public ActionResult RepoterHistory(int id)
        {
            return View(context.reports.Where(r => r.DonorId == id));
        }

        [HttpPost]
        public ActionResult searchBanUser(string email)
        {

            @Session["searchBanUser"] = email;
            // return View(context.reports.Where(r => r.DonorId == id));
            var data = context.bannedUsers.Where(r => r.Email == email).ToList();
            if(!data.Any())
            {
                TempData["searchBanUserError"] = "Not Found";
                return RedirectToAction("banUsersList");
            }

            return View(data);
        }

        [HttpPost]
        public ActionResult searchReports(string email)
        {

            // return View(context.reports.Where(r => r.DonorId == id));
            var data = context.reports.Where(r => r.userInfo.Email == email).ToList();
            if (!data.Any())
            {
                TempData["searchReportsError"] = "Not Found";
                return RedirectToAction("banUsersList");
            }

            return View(data);
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

        public ActionResult darkMood()
        {
            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            if(data.darkMood == "yes")
            {
                data.darkMood = "no";  
            }
            else
            {
                data.darkMood = "yes";
            }
            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();
           //Session["DarkMood"] = data.darkMood;
            return RedirectToAction("Index");
        }

        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index");
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


                context.bannedUsers.Add(bn);
                context.SaveChanges();
                context.Entry(data).State = System.Data.Entity.EntityState.Modified;

            context.SaveChanges();
            return RedirectToAction("banUsersList");
        }
        [HttpGet]
        public ActionResult UnBanUser(string email)
        {
            /* bannedUser bu = new bannedUser();
             var userDum = context.bannedUsers.Where(b => b.Email == email).ToList();*/

            var data = context.bannedUsers.Where(r => r.Email == email).FirstOrDefault<bannedUser>();
            context.bannedUsers.Remove(context.bannedUsers.Find(data.id));
            context.SaveChanges();
            return RedirectToAction("banUsersList");
        }

        [HttpGet]

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(userInfo info, HttpPostedFileBase file)
        {
            var rand = new Random();
            bool EmailAlreadyAdded = context.userInfoes.Any(x => x.Email == info.Email);
            //bool PassAlreadyAdded = context.userInfoes.Any(x => x.Password == info.Password);

            if (EmailAlreadyAdded)
            {
                TempData["EmailExist"] = "Email Already SignUp";
            }
            /*      if (PassAlreadyAdded)
                  {
                      TempData["PasswordExist"] = "Password Already SignUp";
                  }*/
            string path = null;

                path = Path.Combine(Server.MapPath("~/Content/Images/"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                if (!EmailAlreadyAdded && path != null)
                {

                    TempData["DoneReg"] = "New User Added";
                    info.Password = rand.Next(300, 901).ToString() + "azhe";
                    info.Docoment = file.FileName;
                    info.ProPic = file.FileName;
                    context.userInfoes.Add(info);
                    context.SaveChanges();
                    return RedirectToAction("AfterReg", new { info.Email });
                    //return RedirectToAction("AfterReg");

                }
                //TempData["TempPhoto"] = "Add Photo";

            return View();
        }

        [HttpGet]

        public ActionResult CreateContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateContactUs(contactU info)
        {
            context.contactUs.Add(info);
            context.SaveChanges();
            TempData["contactUsDone"] = "Thanks for your Info";
            return RedirectToAction("CreateContactUs");
        }

        [HttpGet]
        public ActionResult Print(string email)
        {
            return new ActionAsPdf("AfterReg",new {email = email })
            {
                FileName = Server.MapPath("~/App_Data/ListProduct.pdf")
            };
        }



        /* public ActionResult banUserDetiels(string email)
         { }*/





        public ActionResult Pie()
        {
            return View();
        }


    }

       
       
 }
