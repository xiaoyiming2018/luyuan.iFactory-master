using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AspNetCoreMvcPager;
using Advantech.IFactory.CommonLibrary;

namespace WebApplication.Controllers
{
    public class TagTypeController : BaseController
    {
        private const int _pagesize = 15;  //显示的行数
        TagTypeManager TTM = new TagTypeManager();
        // GET: SYSTagType
        /// <summary>
        /// 初始页面  加载全部Tag Type信息
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult Index(int pageindex = 1, int pagesize = _pagesize)
        {
            List<System_tag_type> list = new List<System_tag_type>();
            list = TTM.GetALL();
            if (list != null)
            {
                var pagedList = PagedList<System_tag_type>.PageList(pageindex, pagesize, list);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                return View();
            }
        }
        public ActionResult Edit(string name)
        {
            System_tag_type tag_type = new System_tag_type();
            if (string.IsNullOrEmpty(name)) //添加tag type
            {
                return View();
            }
            else         //修改TagType
            {
                tag_type = TTM.SelectOne(name);
                return View(tag_type);
            }
        }
        /// <summary>
        /// 新增和编辑Tag Type
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult EditHandle(System_tag_type TagTypeModel)
        {
            try
            {
                if (TagTypeModel.id>0)
                {
                    if(TTM.Update(TagTypeModel))
                    {
                        ViewBag.Message = "执行成功！";
                        ViewBag.Route = "Index";
                        return View("alert");
                    }
                }
                else
                {
                   if(TTM.Insert(TagTypeModel)>0)
                   {
                        ViewBag.Message = "执行成功！";
                        ViewBag.Route = "Index";
                        return View("alert");
                    }
                }
            }
            catch (Exception ex)
            {
               
            }
            ViewBag.Message = "执行出错！";
            ViewBag.Route = "Edit";
            return View("alert");
        }

        // GET: SYSTagType/Delete/5
        public ActionResult Delete()
        {
            string type_name_en = Request.Query["name"];
            bool re = TTM.Del(type_name_en);
            return Redirect("/TagType/Index");
        }
    }
}