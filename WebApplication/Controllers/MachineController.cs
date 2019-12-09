using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication.Controllers
{
    public class MachineController : BaseController
    {
        MachineInfoManager MIM = new MachineInfoManager();
        LineInfoManager LIM = new LineInfoManager();
        StationManager SIM = new StationManager();

        /// <summary>
        /// 站别信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ActionResult Index(int area_id, int city_id, int plant_id, int line_id, string unit_no, int pageindex = 1, int pagesize = 15)
        {
            MachineInfo obj = new MachineInfo();

            if (area_id > 0 || city_id > 0 || plant_id > 0 || line_id > 0 || !string.IsNullOrEmpty(unit_no))
            {
                obj.area_id = area_id;
                obj.city_id = city_id;
                obj.plant_id = plant_id;
                obj.line_id = line_id;
                obj.unit_no = unit_no;
                var objList = MIM.SelectAll(obj);
                var pagedList = PagedList<MachineInfo>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = MIM.SelectAll(obj);
                var pagedList = PagedList<MachineInfo>.PageList(pageindex, pagesize, objList);
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
                    var obj = MIM.SelectSingleById(id);
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
            MachineInfo obj = new MachineInfo();
            obj.area_id = Convert.ToInt32(HttpContext.Request.Form["area_id"]);
            obj.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            obj.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            obj.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            obj.unit_no = HttpContext.Request.Form["unit_no"];
            obj.machine_code = HttpContext.Request.Form["machine_code"];
            obj.machine_name_en = HttpContext.Request.Form["machine_name_en"];
            obj.machine_name_cn = HttpContext.Request.Form["machine_name_cn"];
            obj.machine_name_tw = HttpContext.Request.Form["machine_name_tw"];
            obj.status_no = HttpContext.Request.Form["status_no"];
            obj.station_id = Convert.ToInt32(HttpContext.Request.Form["station_id"]);
            obj.set_up = Convert.ToInt32(HttpContext.Request.Form["set_up"]);

            if (id > 0)
            {
                obj.machine_id = id;
                count = MIM.Update(obj);
            }
            else
            {
                count = MIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Machine/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <returns></returns>
        public ActionResult Verification()
        {
            int id = 0;
            string machine_code = Request.Query["machine_code"];
            if (!string.IsNullOrEmpty(Request.Query["id"]))
            {
                id = Convert.ToInt32(Request.Query["id"]);
            }
            MachineInfo obj = MIM.SelectSingle(machine_code: machine_code);

            if (id <= 0)
            {
                if (obj == null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            else
            {
                if (obj == null)
                {
                    return Json("Success");
                }
                else
                {
                    if (id == obj.machine_id)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
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
                    return View("Views/Machine/alert.cshtml");
                }
                else
                {
                    return View("alert-fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetLineList()
        {
            int plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            string unit_no = HttpContext.Request.Form["unit_no"];
            ViewBag.lineList = LIM.SelectAll(plant_id: plant_id, unit_no: unit_no);
            return View();
        }

        public ActionResult GetStationList()
        {
            int line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            ViewBag.stationList = SIM.SelectAll(line_id: line_id);
            return View();
        }
    }
}