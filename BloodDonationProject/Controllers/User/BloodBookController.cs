using BloodDonationProject.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BloodDonationProject.Controllers.User
{
    public class BloodBookController : Controller
    {
        BloodDonationDBEntities7 context = new BloodDonationDBEntities7();

        [HttpGet]
        public ActionResult BloodBook()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            var u = (int)Session["userid"];
            var list = context.BloodBooks.Where(x => x.UserId == u);
            return View(list);
        }

        [HttpPost]
        public ActionResult BloodBook(string SearchDonar)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            if (SearchDonar == null)
            {
                var u2 = (int)Session["userid"];
                var list2 = context.BloodBooks.Where(x => x.UserId == u2);
                return View(list2);
            }
            var u = (int)Session["userid"];
            var list = context.BloodBooks.Where(x => x.UserId == u).Where(x=>x.BloodGroup == SearchDonar);
            return View(list);
        }




        [HttpGet]
        public ActionResult AddBloodBook()
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddBloodBook(BloodBook bb)
        {
            if (ModelState.IsValid)
            {
                bb.UserId = (int)Session["userid"];
                context.BloodBooks.Add(bb);
                context.SaveChanges();
                return RedirectToAction("BloodBook");
            }
            return View();
        }

        [HttpGet]
        public ActionResult UpdateBloodBook(int? id)
        {


            if (id.Equals(null))
            {
                return RedirectToAction("BloodBook");
            }

            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            var findMyBB = context.BloodBooks.Find(id);

            if (findMyBB.UserId == (int)Session["userid"])
            {
                return View(context.BloodBooks.Find(id));
            }

            return Content("Access Denined!!");

        }

        [HttpPost]
        public ActionResult UpdateBloodBook(int id, BloodBook bb)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            if (ModelState.IsValid)
            {
                bb.UserId = (int)Session["userid"];
                context.Entry(bb).State = EntityState.Modified;
                context.SaveChanges();
                return RedirectToAction("BloodBook");
            }
            return View();
        }

        [HttpGet]
        public ActionResult RemoveBloodBook(int id)
        {
            if (Session["userid"] == null)
            {
                return RedirectToAction("Index", "User");
            }

            return View(context.BloodBooks.ToList().Find(x=>x.BookId==id));
        }

        [HttpPost, ActionName("RemoveBloodBook")]
        public ActionResult ConfirmRemoveBloodBook(int id)
        {
            var bookToDelete = context.BloodBooks.Find(id);
            context.BloodBooks.Remove(bookToDelete);
            context.SaveChanges();
            return RedirectToAction("BloodBook");
        }


    }
}