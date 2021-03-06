﻿using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace WebApplication.Controllers
{
    public class ErrorTypeController : BaseController
    {
        ErrorTypeManager ECDM = new ErrorTypeManager();
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            string code_no = Request.Query["code_no"];
            if (!string.IsNullOrEmpty(code_no))
            {
                var objList = ECDM.SelectAll(code_no);
                var pagedList = PagedList<error_type>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = ECDM.SelectAll();
                var pagedList = PagedList<error_type>.PageList(pageindex, pagesize, objList);
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
                int count = ECDM.Del(id);
                if (count > 0)
                {
                    return View("Views/ErrorType/alert.cshtml");
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
                    var obj = ECDM.SelectSingle(id: id);
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
            error_type obj = new error_type();
            obj.type_code = HttpContext.Request.Form["type_code"];
            obj.type_name = HttpContext.Request.Form["type_name"];
            obj.remark = HttpContext.Request.Form["remark"];
            obj.createtime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            if (id > 0)
            {
                obj.id = id;
                count = ECDM.Update(obj);
            }
            else
            {
                
                count = ECDM.Insert(obj);
            }

            if (count > 0)
            {
                return View("Views/ErrorType/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

        public ActionResult Validate()
        {
            bool result = true;
            string code_no = Request.Query["code_no"];
            int id = 0;
            if(!string.IsNullOrEmpty(Request.Query["id"]))
            {
                id = Convert.ToInt32(Request.Query["id"]);
            }
            error_type obj = ECDM.SelectSingle(code_no: code_no,id:id);
            if(id<=0)
            {
                if(obj==null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }
            }
            else
            {
                if(obj==null)
                {
                    result = true;
                }
                else
                {
                    if(id==obj.id)
                    {
                        result = true;
                    }
                    else
                    {
                        result=false;
                    }
                }
            }
            return Json(result);
        }
	}
}