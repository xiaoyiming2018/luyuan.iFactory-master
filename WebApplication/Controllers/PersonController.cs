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
    public class PersonController : BaseController
    {
        PersonManager UM = new PersonManager();
        StationManager SM = new StationManager();
        DeptInfoManager CIM = new DeptInfoManager();
        /// <summary>
        /// 人员列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        //public ActionResult Index(int? page = 1, int? size = 11)
        //{
        //    string name = Request.Query["name"];
        //    if (!string.IsNullOrEmpty(name))
        //        {
        //            var objList = UM.SelectAll(name).ToPagedList((int)page, 1000);
        //            return View(objList);
        //        }
        //        else
        //        {
        //            var objList = UM.SelectAll().ToPagedList((int)page, (int)size);
        //            return View(objList);
        //        }
        //}

        public ActionResult Index(int dept_id,string username, int pageindex = 1, int pagesize = 15)
        {
            //int dept_id = 0;
            //if (!string.IsNullOrEmpty(Request.Query["dept_id"]))
            //{
            //    dept_id = Convert.ToInt32(Request.Query["dept_id"]);
            //}
            if (dept_id > 0)
            {
                var objList = UM.SelectAll(dept_id);
                var pagedList = PagedList<person>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = UM.SelectAll();
                var pagedList = PagedList<person>.PageList(pageindex, pagesize, objList);
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
                int count = UM.Del(id);
                if (count > 0)
                {
                    return View("Views/Person/alert.cshtml");
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
        /// 进入编辑页面(添加，修改)
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            //try
            //{
            //    string name = Request.Query["name"];
            //    if (string.IsNullOrEmpty(name))
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //        int id = Convert.ToInt32(Request.Query["id"]);
            //        ViewBag.id = id;
            //        var obj = UM.SelectSingle(name);
            //        return View(obj);
            //    }
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    var obj = UM.SelectSingleByID(id);
                    return View(obj);
                }
                else
                {
                    //Station obj = new Station();
                    //ViewBag.objList = SM.SelectAll(obj);
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
        public ActionResult EditHandle(person PersonModel)
        {
            int count = 0;
            if (PersonModel.id > 0)
            {
                count = UM.Update(PersonModel);
            }
            else
            {
                count = UM.Insert(PersonModel);
            }
            if (count > 0)
            {
                return View("Views/Person/alert.cshtml");
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
            string user_name = Request.Query["user_name"];
            if (!string.IsNullOrEmpty(Request.Query["id"]))
            {
                id = Convert.ToInt32(Request.Query["id"]);
            }
            person obj = UM.SelectSingle(user_name: user_name);

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
                    if (id == obj.id)
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
    }
}