using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace BloodDonationProject.Controllers
{
    public class ChartController : Controller
    {
        Models.BloodDonationDBEntities12 context = new Models.BloodDonationDBEntities12();
        //
        // GET: /Chart/

        public ActionResult Index()
        {
            return View();
        }
        public class Part
        {
            public string Employee { get; set; }

            public int Credit { get; set; }
        }

        [HttpPost]
        public JsonResult NewChart()
        {
            //List<Part> iData = new List<Part>();
            List<object> iData = new List<object>();
 
            DataTable dt = new DataTable();
            var data = context.userInfoes.Where(r => r.Type == "Admin").ToList();
            int adminCount = data.Count;
            data = context.userInfoes.Where(r => r.Type == "Moderator").ToList();
            int ModeratorCount = data.Count;
            data = context.userInfoes.Where(r => r.Type == "Doner").ToList();
            int DonerCount = data.Count;
            data = context.userInfoes.Where(r => r.Type == "User").ToList();
            int UserCount = data.Count;



            dt.Columns.Add("Type", System.Type.GetType("System.String"));
                    dt.Columns.Add("Count", System.Type.GetType("System.Int32"));
           
            DataRow dr = dt.NewRow();
                    dr["Type"] = "Admin";
                    dr["Count"] = adminCount;
                    dt.Rows.Add(dr);

                    dr = dt.NewRow();
                    dr["Type"] = "Modaretor";
                    dr["Count"] = ModeratorCount;
                    dt.Rows.Add(dr);
             dr = dt.NewRow();
            dr["Type"] = "Doner";
            dr["Count"] = DonerCount;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Type"] = "User";
            dr["Count"] = 2;
            dt.Rows.Add(dr);

            foreach (DataColumn dc in dt.Columns)
               {
                   List<object> x = new List<object>();
                   x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                   iData.Add(x);
               }


            return Json(iData, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        public JsonResult NewChart2()
        {
            //List<Part> iData = new List<Part>();
            List<object> iData = new List<object>();

            DataTable dt = new DataTable();
            var data = context.userInfoes.ToList();
            int TotalUserCount = data.Count;
            var data2 = context.bannedUsers.ToList();
            int BanUserCount = data2.Count;



            dt.Columns.Add("Type", System.Type.GetType("System.String"));
            dt.Columns.Add("Count", System.Type.GetType("System.Int32"));


            DataRow dr = dt.NewRow();
            dr["Type"] = "Total Users";
            dr["Count"] = TotalUserCount;
            dt.Rows.Add(dr);


            dr = dt.NewRow();
            dr["Type"] = "Not Banned Users";
            dr["Count"] = TotalUserCount - BanUserCount;
            dt.Rows.Add(dr);

            dr = dt.NewRow();
            dr["Type"] = "Banned Users";
            dr["Count"] = BanUserCount;
            dt.Rows.Add(dr);

            foreach (DataColumn dc in dt.Columns)
            {
                List<object> x = new List<object>();
                x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
                iData.Add(x);
            }


            return Json(iData, JsonRequestBehavior.AllowGet);
        }
    }
}

