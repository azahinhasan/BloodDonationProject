using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;

namespace BloodDonationProject.Controllers.User
{
    public class UserInfoController : Controller
    {
        BloodDonationDBEntities7 context = new BloodDonationDBEntities7();

        // GET: UserInfo
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
        public ActionResult Login(userInfo user)
        {
            if (ModelState.IsValid)
            {
                var check = context.userInfoes.Where(x => x.userID == user.userID && x.Password == user.Password);
                var q = from p in context.userInfoes

                        where p.Email == user.Email

                        && p.Password == user.Password

                        select p;
                if (q.Any())
                {
                    Session["userid"] = (int)q.FirstOrDefault().userID;
                    Session["type"] = (q.FirstOrDefault().Type).ToLower();

                    if (Session["type"].ToString() == "user")
                    {
                        return RedirectToAction("Dashboard", "User");
                    }
                    else if (Session["type"].ToString() == "admin")
                    {
                        return RedirectToAction("Dashboard", "Admin");
                    }
                    else if (Session["type"].ToString() == "donar")
                    {
                        return RedirectToAction("Dashboard", "Donar");
                    }
                }
            }

            ViewBag.Error = "Email or Password is not valid";
            return View();

        }


        //Registration
        [HttpGet]
        public ActionResult UserRegistration()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UserRegistration(userInfo user)
        {
            if (user.Name == null)
            {
                ViewBag.ErroMsg = "Name is Empty";
                return View();
            }
            else if (user.Name.Length < 5)
            {
                ViewBag.ErroMsg = "Name is so Small";
                return View();
            }
            else if (user.Name.Length > 15)
            {
                ViewBag.ErroMsg = "Name is so large";
                return View();
            }

            if (user.Password == null)
            {
                ViewBag.ErroMsg = "Password is Empty";
                return View();
            }
            else if (user.Password.Length < 6 || user.Password.Length > 12)
            {
                ViewBag.ErroMsg = "Password Range(6-12)";
                return View();
            }
            else if (!user.Password.Contains("@") && !user.Password.Contains("#") && !user.Password.Contains("+") && !user.Password.Contains("%") && !user.Password.Contains("$"))
            {
                ViewBag.ErroMsg = "Add a Special Caracter in Password";
                return View();
            }

            if (user.Email == null)
            {
                ViewBag.ErroMsg = "Email is Empty";
                return View();
            }
            else if (!user.Email.Contains("@") || !user.Email.Contains("."))
            {
                ViewBag.ErroMsg = "Email has no @ or dot";
                return View();
            }

            var checkDupliEmail = context.userInfoes.Any(x => x.Email == user.Email);
            if (checkDupliEmail)
            {
                ViewBag.ErroMsg = "Email is used Already";
                return View();
            }
            int number;

            if (user.Phone == null)
            {
                ViewBag.ErroMsg = "Phone is Empty";
                return View();
            }
            else if (!Int32.TryParse(user.Phone, out number))
            {
                ViewBag.ErroMsg = "This not a Number";
                return View();
            }
            else if (!(user.Phone.Length == 11))
            {
                ViewBag.ErroMsg = "Phone has to be exact 11 Numbers";
                return View();
            }
            if (user.Address == null)
            {
                ViewBag.ErroMsg = "Address is Empty";
                return View();
            }
            else if (user.Address.Length == 5)
            {
                ViewBag.ErroMsg = "Address is so small";
                return View();
            }
            if (user.DOB == null)
            {
                ViewBag.ErroMsg = "DOB is Empty";
                return View();
            }
            else if ((DateTime.Now.Year - user.DOB.Value.Year) < 18)
            {
                ViewBag.ErroMsg = "Your Age should be 18";
                return View();
            }

            if (user.BloodGroup == null)
            {
                ViewBag.ErroMsg = "DOB is Empty";
                return View();
            }

            user.Type = "user";
            context.userInfoes.Add(user);
            context.SaveChanges();
            return RedirectToAction("Login");
        }

        

    }
}