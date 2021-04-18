using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Web.Security;
using Rotativa;
using System.IO;
using System.Text.RegularExpressions;
namespace BloodDonationProject.Controllers
{
    public class AdminController : Controller
    {
        
        Models.BloodDonationDBEntities context = new Models.BloodDonationDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            if (Session["ValidType"] != "AdMo")
            {
                return RedirectToAction("Login", "Home");
            }

            var email = Session["Email"].ToString();
            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            Session["DarkMood"] = data.darkMood;
           // Session["DarkMood"] = "no";
            return View();
        }

    
        public ActionResult AfterReg(string email)
        {

            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();

            return View(context.userInfoes.Find(data.userID));
        }

        public ActionResult showReports()
        {

            if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");
                
            }

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
            if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");
                
            }
            return View(context.bannedUsers.ToList());
        }

        public ActionResult RepoterHistory(int id)
        {
            if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");
                
            }
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
            if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");

            }


            //var data = context.userInfoes.Where(r => r.Type == "Admin").FirstOrDefault<userInfo>();
            //Moderator

            return View(context.userInfoes.Where(r => r.Type == "Admin" | r.Type == "Moderator"));
        }
        public ActionResult DisableAdnMod(int id,string email)
        {
           

            var check = context.DisabledAccounts.Where(r => r.Email == email).ToList();

            if(check.Count != 0)
            {
                TempData["DisableEnableError"] = "Account Already Disabled";
                return RedirectToAction("AdnModList");
            }

            DateTime utcDate = DateTime.Today;
            DisabledAccount bn = new DisabledAccount();
            var data = context.userInfoes.Where(r => r.userID == id).FirstOrDefault<userInfo>();

            bn.Email = data.Email;
            bn.Name = data.Name;
            bn.DisabledDate = utcDate;


            context.DisabledAccounts.Add(bn);
            context.SaveChanges();
            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            TempData["DisableEnableError"] = "Account  Disabled";
            context.SaveChanges();
            return RedirectToAction("AdnModList");
        }
        public ActionResult EnableAdnMod(string email)
        {
            var check = context.DisabledAccounts.Where(r => r.Email == email).ToList();

            if (check.Count == 0)
            {
                TempData["DisableEnableError"] = "Account Already Enabled";
                return RedirectToAction("AdnModList");
            }

            var data = context.DisabledAccounts.Where(r => r.Email == email).FirstOrDefault<DisabledAccount>();
            context.DisabledAccounts.Remove(context.DisabledAccounts.Find(data.id));
            context.SaveChanges();
            TempData["DisableEnableError"] = "Account  Enabled";
            return RedirectToAction("AdnModList");
        }
        public ActionResult DonnerList()
        {
            /*if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");

            }*/


            //var data = context.userInfoes.Where(r => r.Type == "Admin").FirstOrDefault<userInfo>();
            //Moderator

            return View(context.userInfoes.Where(r => r.Type == "Donner"));
        }

        public ActionResult DonnerInfo(int id)
        {

            return View(context.userInfoes.Where(r => r.userID == id));
        }

        public ActionResult DonnerVarify(int id)
        {
            var data = context.userInfoes.Where(r => r.userID == id).FirstOrDefault<userInfo>();
            //Moderator

            if (data.isVerified == "yes")
            {
                data.isVerified = "no";
            }
            else
            {
                data.isVerified = "yes";
            }

            context.Entry(data).State = System.Data.Entity.EntityState.Modified;
            context.SaveChanges();

            return RedirectToAction("DonnerInfo",new {id = id });

           //return View(context.userInfoes.Where(r => r.userID == id));
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
            if (Session["Type"] != "Admin")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");

            }
            return View();
        }

        [HttpPost]
        public ActionResult Create(userInfo info, HttpPostedFileBase file)
        {
            var rand = new Random();
            bool EmailAlreadyAdded = context.userInfoes.Any(x => x.Email == info.Email);
            Salary infoSalary = new Salary();
            //bool PassAlreadyAdded = context.userInfoes.Any(x => x.Password == info.Password);

            if (EmailAlreadyAdded)
            {
                TempData["EmailExist"] = "Email Already SignUp";
            }
            /*      if (PassAlreadyAdded)
                  {
                      TempData["PasswordExist"] = "Password Already SignUp";
                  }*/
            if (info.Email == null || info.Name==null || info.Address == null || info.Salary == null || info.Phone == null)
            {
                TempData["ValidationError"] = "Fill UP All Text Box";
            }
            if (info.Salary <=0)
            {
                TempData["SalaryError"] = "Range should be more then 0";
            }
            /*if (info.Phone)
             {
                 TempData["SalaryError"] = "Range should be more then 0";
             }*/
           /* if (Regex.Match(info.Phone, (@"^(\+[0-9]{9})$").Success)
            {
                TempData["PhoneError"] = "InValid Phone Number";
            }*/
            string path = null;
            if (file == null) { TempData["TempPhotoError"] = "Have To Upload Pic"; }
      
            else
            {
                    if (file.ContentType == "image/png" || file.ContentType == "image/jpeg" || file.ContentType == "image/jpg")
                    {
                        path = Path.Combine(Server.MapPath("~/Content/Images/"), Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        if (!EmailAlreadyAdded && path != null)
                        {

                            TempData["DoneReg"] = "New User Added";
                            info.Password = rand.Next(300, 901).ToString() + "azhe";
                            info.Docoment = "none";
                            info.ProPic = file.FileName;
                            info.darkMood = "no";
                            context.userInfoes.Add(info);
                            context.SaveChanges();
                            infoSalary.UserId = info.userID;
                            context.Salaries.Add(infoSalary);
                            context.SaveChanges();
                            return RedirectToAction("AfterReg", new { info.Email });
                            //return RedirectToAction("AfterReg");

                        }
                    }
                    else
                    {
                        TempData["TempPhotoError"] = "Pic type should me png/jpeg/jpg";
                    }
                
            }
           


            //TempData["TempPhoto"] = "Add Photo";
       
            return View();
        }

        public ActionResult CreateContactUsList()
        {
            if (Session["ValidType"] != "AdMo")
            {
                TempData["errorLogin"] = "Have To Login First";
                return RedirectToAction("Login", "Home");

            }
            return View(context.contactUs.ToList());
        }

        public ActionResult SalaryList()
        {
            if (Session["Email"].ToString() != "azahinhasan@gmail.com")
            {
                TempData["AcessDenay"] = "You dont have access!";
                return RedirectToAction("Index");

            }
            return View(context.Salaries.ToList());
        }

        [HttpGet]
        public ActionResult SalaryListUpdate(int id)
        {
            var SalaryListUpdate = context.Salaries.Find(id);

            return View(SalaryListUpdate);
        }

        [HttpPost]
        public ActionResult SalaryListUpdate(int id, Salary info)
        {
            info.id = id;
            var SalaryListUpdate = context.Salaries.Find(id);

            SalaryListUpdate.January = info.January;
            SalaryListUpdate.February = info.February;
            SalaryListUpdate.March = info.March;
            SalaryListUpdate.April = info.April;
            SalaryListUpdate.May = info.May;
            SalaryListUpdate.June = info.June;
            SalaryListUpdate.July = info.July;
            SalaryListUpdate.August = info.August;
            SalaryListUpdate.September = info.September;
            SalaryListUpdate.October = info.October;
            SalaryListUpdate.November = info.November;
            SalaryListUpdate.December = info.December;
            //context.Entry(category).State = EntityState.Modified;
            context.SaveChanges();
            TempData["SalaryUpadated"] = "Salary Info Updated";
            return RedirectToAction("SalaryListUpdate",new { id = id});
        }


        [HttpGet]

        public ActionResult CreateContactUs()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateContactUs(contactU info)
        {
            if (info.Name == null || info.Email == null || info.Massage == null)
            {
                TempData["contactUsDone"] = "Fill up All Text Box";
                return RedirectToAction("CreateContactUs");
            }
            context.contactUs.Add(info);
            context.SaveChanges();
            TempData["contactUsDone"] = "Thanks for your Info";
            return RedirectToAction("CreateContactUs");
        }
        public ActionResult CreateContactUsDelete(int id)
        {

            context.contactUs.Remove(context.contactUs.Find(id));
            context.SaveChanges();
            return RedirectToAction("CreateContactUsList");

        }
        public ActionResult CreateContactUsAbout(string email)
            {

            var data = context.userInfoes.Where(r => r.Email == email).FirstOrDefault<userInfo>();
            if (data != null)
            {
                return RedirectToAction("RepoterInfo",new { id = data.userID , email = data.Email });
            }

            return RedirectToAction("DataNotFound");
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

        [HttpGet]
        public ActionResult DataNotFound()
        {
            return View();
        }

        public ActionResult UnBanAllView()
        {
            return View();
        }

   
        public ActionResult UnBanAll()
        {
            var data = context.bannedUsers.ToList();
            int counter = 0;
            foreach (var i in data)
            {
                DateTime dt = (DateTime)i.BannedDate;
                int comp = int.Parse(DateTime.Now.ToString("yyyyMMdd")) - int.Parse(dt.ToString("yyyyMMdd"));

                if (comp / 10000 >= 1)
                {
                    counter++;
                    context.bannedUsers.Remove(context.bannedUsers.Find(i.id));
                    context.SaveChanges();
                }
            }

            TempData["TotalUnbanded"] = counter;

            return RedirectToAction("UnBanAllView");
            //return RedirectToAction("DataNotFound");
        }

        public ActionResult Pie()
        {
            return View();
        }


    }

       
       
 }
