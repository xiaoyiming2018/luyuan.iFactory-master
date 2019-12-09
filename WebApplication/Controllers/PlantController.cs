using System;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class PlantController : BaseController
    {
        PlantInfoManager PIM = new PlantInfoManager();
        CityInfoManager CIM = new CityInfoManager();
        //
        // GET: /Plant/
        public ActionResult Index(int city_id, int pageindex = 1, int pagesize = 15)
        {
            //city_id = 0;
            //if (!string.IsNullOrEmpty(Request.Query["city_id"]))
            //{
            //    city_id = Convert.ToInt32(Request.Query["city_id"]);
            //}
            if (city_id > 0)
            {
                var objList = PIM.SelectAll(city_id);//.ToPagedList((int)page, (int)size);
                var pagedList = PagedList<plant_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = PIM.SelectAll();//.ToPagedList((int)page, 1000);
                var pagedList = PagedList<plant_info>.PageList(pageindex, pagesize, objList);
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
                int count = PIM.Del(id);
                if (count > 0)
                {
                    return View("Views/Plant/alert.cshtml");
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
                if (id > 0)
                {
                    ViewBag.id = id;
                    ViewBag.area_id = area_id;
                    ViewBag.city_id = city_id;
                    var obj = PIM.SelectSingle(id);
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
            plant_info obj = new plant_info();
            obj.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            obj.plant_name_en = HttpContext.Request.Form["plant_name_en"];
            obj.plant_name_cn = HttpContext.Request.Form["plant_name_cn"];
            obj.plant_name_tw = HttpContext.Request.Form["plant_name_tw"];
            if (id > 0)
            {
                obj.plant_id = id;
                count = PIM.Update(obj);
            }
            else
            {
                count = PIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Plant/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

        public ActionResult GetCityList()
        {
            int area_id = Convert.ToInt32(HttpContext.Request.Form["area_id"]);
            ViewBag.cityList = CIM.SelectAll(area_id);
            return View();
        }
	}
}