using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;


namespace WebApplication.Controllers
{
    public class ClassController : BaseController
    {
        ClassInfoManager UIM = new ClassInfoManager();
        //
        // GET: /Class/
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = UIM.SelectAll();
            var pagedList = PagedList<class_info>.PageList(pageindex, pagesize, objList);
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
                int count = UIM.Del(id);
                if (count > 0)
                {
                    return View("Views/Class/alert.cshtml");
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
                    var obj = UIM.SelectSingle(id: id);
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
            class_info obj = new class_info();

            obj.class_no = HttpContext.Request.Form["Class_no"];
            obj.class_name_en = HttpContext.Request.Form["class_name_en"];
            obj.class_name_tw = HttpContext.Request.Form["class_name_tw"];
            obj.class_name_cn = HttpContext.Request.Form["class_name_cn"];
            obj.start_time = HttpContext.Request.Form["start_time"];
            obj.end_time = HttpContext.Request.Form["end_time"];
            if (id > 0)
            {
                obj.class_id = id;
                count = UIM.Update(obj);
            }
            else
            {
                count = UIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Class/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }
	}
}