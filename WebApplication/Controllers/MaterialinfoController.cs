using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class MaterialinfoController : BaseController
    {
        MaterialInfoManager AIM = new MaterialInfoManager();
        //
        // GET: /Area/
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            string material_name = null;
            var objList = AIM.SelectAll(material_name);
            var pagedList = PagedList<material_info>.PageList(pageindex, pagesize, objList);
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
                    return View("Views/MaterialInfo/alert.cshtml");
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
                    var obj = AIM.SelectSingle(id);
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
            material_info obj = new material_info();
            obj.category_id =Convert.ToInt32( HttpContext.Request.Form["category_id"]);
            obj.material_code = HttpContext.Request.Form["material_code"].ToString();
            obj.material_name = HttpContext.Request.Form["material_name"].ToString();
            obj.material_type = HttpContext.Request.Form["material_type"].ToString();
            obj.material_inventory = Convert.ToInt32(HttpContext.Request.Form["material_inventory"]);
            obj.remark = HttpContext.Request.Form["remark"].ToString();
            obj.createtime = Convert.ToDateTime(HttpContext.Request.Form["createtime"]);
            if (id > 0)
            {
                obj.id = id;
                count = AIM.Update(obj);
            }
            else
            {
                count = AIM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/Materialinfo/alert.cshtml");
            }
            else
            {
                return Content("<script>alert('Failure to execute');history.go(-1)</script>");
            }
        }
	}
}