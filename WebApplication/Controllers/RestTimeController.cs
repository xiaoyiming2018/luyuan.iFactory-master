using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class RestTimeController : BaseController
    {
        RestTimeManager RTM = new RestTimeManager();
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = RTM.SelectAll();
            var pagedList = PagedList<rest_time>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }
        public ActionResult Del()
        {
            int id = Convert.ToInt32(Request.Query["id"]);
            int count = RTM.Del(id);
            if (count > 0)
            {
                return View("Views/RestTime/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

        public ActionResult Verification()
        {
            string start_time = HttpContext.Request.Form["start_time"];
            string end_time = HttpContext.Request.Form["end_time"];
            if (Convert.ToDateTime(start_time) < Convert.ToDateTime(end_time))
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }
        }

        public ActionResult Edit()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    var obj = RTM.SelectSingle(id);
                    return View(obj);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult EditHandle()
        {
            int id = 0;
            int count = 0;
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }

            rest_time obj = new rest_time();
            obj.unit_no = HttpContext.Request.Form["unit_no"].ToString();
            obj.start_time = HttpContext.Request.Form["start_time"];
            obj.end_time = HttpContext.Request.Form["end_time"];
            obj.state = HttpContext.Request.Form["state"];

            if (id > 0)
            {
                obj.id = id;
                count = RTM.Update(obj);
            }
            else
            {
                count = RTM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/RestTime/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }
	}
}