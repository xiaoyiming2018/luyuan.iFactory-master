using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class CityController : BaseController
    {
        CityInfoManager CIM = new CityInfoManager();
        //
        // GET: /City/
        public ActionResult Index(int area_id, int pageindex = 1, int pagesize = 15)
        {
            //int area_id = 0;
            //if (!string.IsNullOrEmpty(Request.Query["area_id"]))
            //{
            //    area_id = Convert.ToInt32(Request.Query["area_id"]);
            //}
            if (area_id > 0)
            {
                var objList = CIM.SelectAll(area_id);
                var pagedList = PagedList<city_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = CIM.SelectAll();
                var pagedList = PagedList<city_info>.PageList(pageindex, pagesize, objList);
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
                    return View("Views/City/alert.cshtml");
                }
                else
                {
                    return View("alert-fail");
                }
            }
            catch (Exception ex)
            {
                return View("alert-fail");
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
                if (id > 0)
                {
                    ViewBag.id = id;
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
            city_info obj = new city_info();
            obj.area_id = Convert.ToInt32(HttpContext.Request.Form["area_id"]);
            obj.city_name_en = HttpContext.Request.Form["city_name_en"];
            obj.city_name_cn = HttpContext.Request.Form["city_name_cn"];
            obj.city_name_tw = HttpContext.Request.Form["city_name_tw"];
            if (id > 0)
            {
                obj.city_id = id;
                count = CIM.Update(obj);
            }
            else
            {
                count = CIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/City/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }
	}
}