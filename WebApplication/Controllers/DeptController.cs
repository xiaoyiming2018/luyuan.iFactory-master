using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class DeptController : BaseController
    {
        PlantInfoManager PIM = new PlantInfoManager();
        DeptInfoManager CIM = new DeptInfoManager();
        //
        // GET: /Plant/
        public ActionResult Index(int plant_id, int pageindex = 1, int pagesize = 15)
        {
            if (plant_id > 0)
            {
                var objList = CIM.SelectAll(plant_id);//.ToPagedList((int)page, (int)size);
                var pagedList = PagedList<dept_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = CIM.SelectAll();//.ToPagedList((int)page, 1000);
                var pagedList = PagedList<dept_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
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
                int count = CIM.Del(id);
                if (count > 0)
                {
                    return View("Views/Dept/alert.cshtml");
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

        /// <summary>
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int area_id = Convert.ToInt32(Request.Query["area_id"]);
                int city_id = Convert.ToInt32(Request.Query["city_id"]);
                int plant_id = Convert.ToInt32(Request.Query["plant_id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    ViewBag.area_id = area_id;
                    ViewBag.city_id = city_id;
                    ViewBag.plant_id = plant_id;
                    var obj = CIM.SelectSingle(id);
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
           dept_info obj = new dept_info();
            obj.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            obj.dept_no= HttpContext.Request.Form["dept_no"];
            obj.dept_name_en = HttpContext.Request.Form["dept_name_en"];
            obj.dept_name_cn = HttpContext.Request.Form["dept_name_cn"];
            obj.dept_name_tw = HttpContext.Request.Form["dept_name_tw"];
            if (id > 0)
            {
                obj.dept_id = id;
                count = CIM.Update(obj);
            }
            else
            {
                count = CIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Dept/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

    
        public ActionResult GetPlantList()
        {
            int plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            ViewBag.plantList = PIM.SelectAll(plant_id);
            return View();
        }
    }
}