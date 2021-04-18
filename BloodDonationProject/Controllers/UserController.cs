using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BloodDonationProject.Models;
using System.Data.Entity;
using System.IO;

namespace BloodDonationProject.Controllers
{
    public class UserController : Controller
    {
        BloodDonationDBEntities context = new BloodDonationDBEntities();
        UserModel um = new UserModel();


        public ActionResult Test()
        {
            return View();
        }


        public ActionResult Index()
        {
            //Session["userid"] = 3036;
            //Session["type"] = "user";
            return View();
        }

        public ActionResult Dashboard()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            var userId = (int)Session["userid"];

            //ViewBag.MyPosts = context.userInfoes.Find(Session["userid"]).Posts;
            ViewBag.MyPosts = context.Posts.Where(x => x.UserId == userId).OrderByDescending(x=>x.Time).ToList();
            ViewBag.MyRequest = context.RequestBloods.Where(x => x.UserId == userId).ToList();
            //ViewBag.MyRequest = context.RequestBloods.ToList().Find(x => x.UserId == userId);
            return View(context.userInfoes.Find(Session["userid"]));
        }


        [HttpGet]
        public ActionResult Donarslist()
        {
            var donarlist = context.userInfoes.Where(x => x.Type == "donar").ToList();
            return View(donarlist);
        }

        [HttpPost]
        public ActionResult Donarslist(string SearchDonar)
        {
            var donarlist = context.userInfoes.Where(x => x.Type == "donar").ToList();
            donarlist = donarlist.Where(x => x.BloodGroup == SearchDonar).ToList();
            //TempData["search_donar"] = donarlist;
            return View(donarlist); ;
        }

        public ActionResult MyProfile()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            
            return View(context.userInfoes.Find(Session["userid"]));
        }


        [HttpGet]
        public ActionResult EditProfile()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            return View(context.userInfoes.Find(Session["userid"]));
        }

        [HttpPost]
        public ActionResult EditProfile(userInfo user)
        {
            var userToEdit = context.userInfoes.Find(Session["userid"]);
            userToEdit.Name = user.Name;
            userToEdit.Phone = user.Phone;
            userToEdit.Email = user.Email;
            //userToEdit.DOB = user.DOB;
            userToEdit.BloodGroup = user.BloodGroup;
            userToEdit.Address = user.Address;
            context.SaveChanges();
            return RedirectToAction("MyProfile", "User");
        }

        public ActionResult UserDetails(int id)
        {
            if (id == (int) Session["userid"])
            {
                return RedirectToAction("MyProfile", "User");
            }
            return View(context.userInfoes.Find(id));
        }


        [HttpGet]
        public ActionResult RequestBlood(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index");
            }

            return View();
        }

        [HttpPost]
        public ActionResult RequestBlood(int id, RequestBlood rb)
        {
            if (rb.Request_Message == null)
            {
                ViewBag.ErrorMsg = "Request Message is Empty";
                return View();
            }

            rb.DonarId = id;
            rb.UserId = (int)Session["userid"];
            context.RequestBloods.Add(rb);
            context.SaveChanges();
            return RedirectToAction("Donarslist");
        }



        public ActionResult DataVisualizePieChart()
        {
            var userId = (int)Session["userid"];
            ViewBag.MyPosts = context.Posts.Where(x => x.UserId == userId).ToList();
            return View(context.userInfoes.Find(Session["userid"]));
        }


        [HttpGet]
        public ActionResult ChangeProPic()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult ChangeProPic(HttpPostedFileBase file)
        {
            if (file == null)
            {
                ViewBag.ErrMsg = "Please Select a Valid Image";
                return View();
            }

            if (file.ContentLength > 0)
            {
                var userid = Session["userid"];
                //u.ProPic = 
                string extension = Path.GetExtension(file.FileName);
                string _FileName = userid.ToString() + extension;
                //string _FileName = Path.GetFileName(file.FileName);
                string _path = Path.Combine(Server.MapPath("~/UploadedFiles"), _FileName);

                //change file name to user id and update to user propic entity

                var userToEdit = context.userInfoes.Find(Session["userid"]);
                userToEdit.ProPic = "UploadedFiles/" + _FileName;
                context.SaveChanges();

                file.SaveAs(_path);

            }

            else
            {
                ViewBag.Message = "Please Select a Valid Image";
                return View();
            }

            ViewBag.Message = "File Uploaded Successfully!!";
            return RedirectToAction("MyProfile");

        }


        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction("Index", "User");
        }


        [HttpGet]
        public ActionResult ReportThisUser(int id)
        {

            return View();
        }

        [HttpGet]
        public ActionResult EditOthersInfo()
        {
            int userid = (int)Session["userid"];
            return View(context.userInfoes.Find(userid));
        }

        [HttpPost]
        public ActionResult EditOthersInfo(userInfo user)
        {
            int userid = (int)Session["userid"];
            var userToEdit = context.userInfoes.Find(userid);
            userToEdit.Gender = user.Gender;
            userToEdit.NID = user.NID;
            userToEdit.Social_Profile = user.Social_Profile;
            context.SaveChanges();
            return RedirectToAction("MyProfile", "User");
        }


    }
}