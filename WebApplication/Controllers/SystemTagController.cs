using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreMvcPager;
using WebApplication.Controllers;

namespace WebApplication.Controllers
{
    public class SystemTagController : BaseController
    {
        private const int _pagesize = 15;  //显示的行数

        SystemTagCodeManager STCM = new SystemTagCodeManager();
        // GET: SystemTag
        public ActionResult Index(int pageindex = 1, int pagesize = _pagesize)
        {
            List<system_tag_code_web> list = new List<system_tag_code_web>();
            list = STCM.SeclectAllForWeb();
            if (list != null)
            {
                var pagedList = PagedList<system_tag_code_web>.PageList(pageindex, pagesize, list);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                return View();
            }
        }

        // GET: SystemTag/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: SystemTag/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: SystemTag/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        SystemTagTypeManager STTM = new SystemTagTypeManager();
        // GET: SystemTag/Edit/5
        public ActionResult Edit(string name ="")
        {
            system_tag_code_web tag_type = new system_tag_code_web();
            List<System_tag_type> sttList = new List<System_tag_type>();
            sttList = STTM.SeclectAll();
            ViewBag.TypeList = sttList;
            if (name == "") //添加tag type
            {
                return View();
            }
            else         //修改TagType
            {
                tag_type = STCM.SelectOne(name);
                ViewBag.SelectedType = tag_type.type_name_en;
                return View(tag_type);
            }
        }

        // GET: SystemTag/Delete/5
        public ActionResult Delete(int pagesize = _pagesize)
        {
            string code_name_en = Request.Query["name"];
            int pageIndex = Convert.ToInt32(Request.Query["pageIndex"]);
            bool re = STCM.DeleteOne(code_name_en);
            return Redirect("/SystemTag/Index");
        }



        /// <summary>
        /// 新增和编辑Tag Type
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult EditHandle(int pagesize = _pagesize, int pageindex = 1)
        {
            try
            {
                int Type_id = Convert.ToInt32( HttpContext.Request.Form["Type_id"]);
                string System_Tag_name_en = HttpContext.Request.Form["System_Tag_name_en"];
                string System_Tag_name_cn = HttpContext.Request.Form["System_Tag_name_cn"];
                string System_Tag_name_tw = HttpContext.Request.Form["System_Tag_name_tw"];
                string description = HttpContext.Request.Form["description"];
                if (System_Tag_name_en != null)
                {
                    system_tag_code_web obj = new system_tag_code_web();
                    obj = STCM.SelectOne(System_Tag_name_en);
                    if (obj == null)
                        STCM.InsertOne(Type_id, System_Tag_name_en, System_Tag_name_cn, System_Tag_name_tw, description);//如果没有查询到则插入一笔数据
                    else
                        STCM.Update(Type_id, System_Tag_name_en, System_Tag_name_cn, System_Tag_name_tw, description);   //如果查询到则更新这笔数据
                    List<system_tag_code_web> list = new List<system_tag_code_web>();
                    list = STCM.SeclectAllForWeb();
                    var pagedList = PagedList<system_tag_code_web>.PageList(1, pagesize, list);
                    ViewBag.model = pagedList.Item2;
                    return View("Views/SystemTag/Index.cshtml", pagedList.Item1);
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
    }
}