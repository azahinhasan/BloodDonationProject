using BloodDonationProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationProject.Controllers.User
{
    public class FlagPostController : Controller
    {
        BloodDonationDBEntities7 context = new BloodDonationDBEntities7();

        [HttpGet]
        public ActionResult FlagThisPost(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            var postId = context.Posts.Find(id);

            var checkMyPost = postId.UserId == (int)Session["userid"];
            if (checkMyPost)
            {
                return Content("You can not Flag your own post");
            }
            var myFlags = context.FlagPosts.ToList().Where(x => x.userID == (int)Session["userid"]);
            var checkAlreadyFlaged = myFlags.Any(x => x.PostID == id);

            if (checkAlreadyFlaged)
            {
                return Content("Post has been already Flagged by You!!");
            }
            return View();
        }

        [HttpPost]
        public ActionResult FlagThisPost(int id, FlagPost fp)
        {
            fp.userID = (int)Session["userid"];
            fp.PostID = id;
            context.FlagPosts.Add(fp);
            context.SaveChanges();
            return RedirectToAction("AllPosts", "Post");
        }
    }
}