
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class AttendanceController : BaseController
    {
        AttendanceManager UM = new AttendanceManager();
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = UM.SelectAll();
            var pagedList = PagedList<Attendance>.PageList(pageindex, pagesize, objList);
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
                int count = UM.Del(id);
                if (count > 0)
                {
                    return View("Views/Attendance/alert.cshtml");
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
        /// 编辑页面(添加，修改)
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
                    var obj = UM.SelectById(id);
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
        public ActionResult EditHandle(Attendance AttendanceModel)
        {
            int count = 0;

            if (AttendanceModel.standard_num==0 || AttendanceModel.real_num==0)
            {
                return View("alert-fail");
            }
            if (AttendanceModel.id>0)
            {
                count = UM.Update(AttendanceModel);
            }
            else
            {
                AttendanceModel.createtime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                count = UM.Update(AttendanceModel);
            }

            if (count > 0)
            {
                return View("Views/Attendance/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }
	}
}