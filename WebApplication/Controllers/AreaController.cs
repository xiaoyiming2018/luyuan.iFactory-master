using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class AreaController : BaseController
    {
        AreaInfoManager AIM = new AreaInfoManager();
        //
        // GET: /Area/
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = AIM.SelectAll();
            var pagedList = PagedList<area_info>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
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
                int count = AIM.Del(id);
                if (count > 0)
                {
                    return View("Views/Area/alert.cshtml");
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
                if (id > 0)
                {
                    ViewBag.id = id;
                    var obj = AIM.SelectSingleID(id);
                    return View(obj);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("alert-fail");
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
            area_info obj = new area_info();
            obj.area_name_en = HttpContext.Request.Form["area_name_en"];// Request.Params["area_name_en"];
            obj.area_name_tw = HttpContext.Request.Form["area_name_tw"];
            obj.area_name_cn = HttpContext.Request.Form["area_name_cn"];
            if (obj.time_zone== 0)
            {
                obj.time_zone = 8;
            }
            else {
                obj.time_zone =Convert.ToInt32( HttpContext.Request.Form["time_zone"]);
            }
            if (id > 0)
            {
                obj.area_id = id;
                count = AIM.Update(obj);
            }
            else
            {
                count = AIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Area/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }
	}
}