using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication.Controllers
{
    public class MachineWorkingTimeController : BaseController
    {
        MachineworkingtimeManager MIM = new MachineworkingtimeManager();
        LineInfoManager LIM = new LineInfoManager();

        /// <summary>
        /// 站别信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ActionResult Index(int area_id, int city_id, int plant_id, int line_id, string unit_no, int pageindex = 1, int pagesize = 15)
        {
            machine_working_time obj = new machine_working_time();

            if (area_id > 0 || city_id > 0 || plant_id > 0 || line_id > 0 || !string.IsNullOrEmpty(unit_no))
            {
                //obj.area_id = area_id;
                //obj.city_id = city_id;
                obj.plant_id = plant_id;
                obj.line_id = line_id;
                obj.unit_no = unit_no;
                var objList = MIM.SelectAll(obj);
                var pagedList = PagedList<machine_working_time>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = MIM.SelectAll(obj);
                var pagedList = PagedList<machine_working_time>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
        }


        /// <summary>
        /// 站别编辑页面（添加，编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    ViewBag.area_id = Convert.ToInt32(Request.Query["area_id"]);
                    ViewBag.city_id = Convert.ToInt32(Request.Query["city_id"]);
                    ViewBag.plant_id = Convert.ToInt32(Request.Query["plant_id"]);
                    var obj = MIM.SelectSingleID(id);
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

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditHandle()
        {
            int id = 0;
            int count = 0;
            if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
            {
                id = Convert.ToInt32(HttpContext.Request.Form["id"]);
            }
            machine_working_time obj = new machine_working_time();
            obj.machine_id = Convert.ToInt32(HttpContext.Request.Form["machine_id"]);
            obj.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            obj.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            obj.unit_no = HttpContext.Request.Form["unit_no"];
            obj.part_num = HttpContext.Request.Form["part_num"].ToString();
            obj.station_id = Convert.ToInt32(HttpContext.Request.Form["station_id"]);
            obj.standard_time =Convert.ToDecimal( Convert.ToDecimal(HttpContext.Request.Form["standard_time"]));

            if (id > 0)
            {
                obj.id = id;
                count = MIM.Update(obj);
            }
            else
            {
                count = MIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/MachineWorkingTime/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

       
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int count = MIM.Del(id);
                if (count > 0)
                {
                    return View("Views/MachineWorkingTime/alert.cshtml");
                }
                else
                {
                    return Content("<script>alert('Failure to execute');history.go(-1)</script>");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetLineList()
        {
            int plant_id = Convert.ToInt32(Request.Query["plant_id"]);
            string unit_no = Request.Query["unit_no"];
            ViewBag.lineList = LIM.SelectAll(plant_id: plant_id, unit_no: unit_no);
            return View();
        }
	}
}