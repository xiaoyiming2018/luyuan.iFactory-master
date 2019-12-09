using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace WebApplication.Controllers
{
    public class MaterialCategoryController : BaseController
    {
        material_categoryManager MIM = new material_categoryManager();
        LineInfoManager LIM = new LineInfoManager();

        /// <summary>
        /// 站别信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ActionResult Index(int line_id, string unit_no, int pageindex = 1, int pagesize = 15)
        {
            if (line_id > 0 || !string.IsNullOrEmpty(unit_no))
            {

                var objList = MIM.SelectAll(line_id, unit_no);
                var pagedList = PagedList<material_category>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = MIM.SelectAll(line_id, unit_no);
                var pagedList = PagedList<material_category>.PageList(pageindex, pagesize, objList);
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
            material_category obj = new material_category();
            obj.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            obj.unit_no = HttpContext.Request.Form["unit_no"];
            obj.type_code = HttpContext.Request.Form["type_code"];
            obj.type_name = HttpContext.Request.Form["type_name"];
            obj.remark = HttpContext.Request.Form["remark"];

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
                return View("Views/MaterialCategory/alert.cshtml");
            }
            else
            {
                return Content("<script>alert('Failure to execute');history.go(-1)</script>");
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
                    return View("Views/MaterialCategory/alert.cshtml");
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