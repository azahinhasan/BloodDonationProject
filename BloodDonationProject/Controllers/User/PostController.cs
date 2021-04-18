using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BloodDonationProject.Models;
using System.Web.Mvc;
using System.Data.Entity;
using BloodDonationProject.Repository;

namespace BloodDonationProject.Controllers.User
{
    public class PostController : Controller
    {
        BloodDonationDBEntities7 context = new BloodDonationDBEntities7();
        UserModel um = new UserModel();

        [HttpGet]
        public ActionResult AddPost()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index","User");
            }

            if (um.CheckGuestUser())
            {
                TempData["guest_message"] = "Kindly Login First";
                return RedirectToAction("Index");
            }
            Post p = new Post() { UserId = (int)Session["userid"], Time = DateTime.Now };
            return View(p);
        }


        [HttpPost]
        public ActionResult AddPost(Post p)
        {
            if (ModelState.IsValid)
            {
                p.UserId = (int)Session["userid"];
                p.Time = DateTime.Now;
                context.Posts.Add(p);
                context.SaveChanges();
                return RedirectToAction("AllPosts");
            }
            return View();
        }

        public ActionResult AllPosts()
        {
            var list = context.Posts.ToList();
            list = list.OrderByDescending(c => c.Time).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult EditPost(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index");
            }
            var postToEdit = context.Posts.Find(id);

            if (postToEdit.UserId == (int)Session["userid"])
            {
                return View(context.Posts.Find(id));
            }

            return Content("Forbidden");

        }

        [HttpPost]
        public ActionResult EditPost(int id, Post p)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                var postToEdit = context.Posts.Find(id);
                postToEdit.Address = p.Address;
                postToEdit.Text = p.Text;
                postToEdit.WantedBlood = p.WantedBlood;

                //context.Entry(postToEdit).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("Dashboard", "User");
            }
            return View();
        }

        [HttpGet]
        public ActionResult DeletePost(int id)
        {
            return View(context.Posts.Find(id));
        }

        [HttpPost, ActionName("DeletePost")]
        public ActionResult ConfirmDeletePost(int id)
        {
            context.Posts.Remove(context.Posts.Find(id));
            context.SaveChanges();
            return RedirectToAction("AllPosts");
        }

        public ActionResult PostDetails(int id)
        {
            return View(context.Posts.Find(id));
        }
    }
}