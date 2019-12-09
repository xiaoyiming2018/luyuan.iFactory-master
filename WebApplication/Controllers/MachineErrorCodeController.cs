using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace WebApplication.Controllers
{
    public class MachineErrorCodeController : BaseController
    {
        MachineErrorCodeManager machineErrorCodeManager = new MachineErrorCodeManager();
        MachineErrorCodeLogManager machineErrorCodeLogManager = new MachineErrorCodeLogManager();
        /// <summary>
        /// 查询到的信息列表
        /// </summary>
        private static List<MachineErrCodeLogForRpt> LoadRptList = new List<MachineErrCodeLogForRpt>();
        private static DateTime rptStartTime, rptEndTime;
        private static string rptMachineCode;

        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            string code_no = Request.Query["code_no"];
            if (!string.IsNullOrEmpty(code_no))
            {
                var objList = machineErrorCodeManager.SelectAll(code_no);
                var pagedList = PagedList<MachineErrorCode>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = machineErrorCodeManager.GetAllErrorCode();
                var pagedList = PagedList<MachineErrorCode>.PageList(pageindex, pagesize, objList);
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
                int count = machineErrorCodeManager.Del(id);
                if (count > 0)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = "删除失败！";
                    ViewBag.Route = "Index";
                    return View("alert");
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
                    var obj = machineErrorCodeManager.GetErrorCodeById(id: id);
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
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult EditHandle(MachineErrorCode model)
        {
            int count = 0;
            if (model == null)
            {
                return View();
            }
            else
            {
                if (model.id > 0)
                {
                    count=machineErrorCodeManager.Update(model);
                }
                else
                {
                    count=machineErrorCodeManager.Insert(model);
                }
                if (count > 0)
                {
                    return View();
                }
                else
                {
                    ViewBag.Message = "数据提交失败！";
                    ViewBag.Route = "Index";
                    return View("alert");
                }
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
            MachineErrorCode obj = machineErrorCodeManager.GetErrorCodeByNo(code_no: code_no);
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

        //
        // 报表查询进入页面
        public ActionResult Report()
        {
            ViewBag.start_time = DateTime.Now;
            ViewBag.end_time = DateTime.Now.AddDays(1);
            ViewBag.machine_code = "";
            return View();
        }
        /// <summary>
        /// 分页导航跳转
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult PageNav(int pageindex = 1, int pagesize = 15)
        {
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.machine_code = rptMachineCode;
            var pagedList = PagedList<MachineErrCodeLogForRpt>.PageList(pageindex, pagesize, LoadRptList);
            ViewBag.model = pagedList.Item2;
            return View("Report", pagedList.Item1);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportQuery(DateTime start_time, DateTime end_time, string machine_code)
        {
            //List<pro_schedule> list = new List<pro_schedule>();
            rptStartTime = start_time;
            rptEndTime = end_time;
            rptMachineCode = machine_code;
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.machine_code = rptMachineCode;
            LoadRptList = new List<MachineErrCodeLogForRpt>();
            try
            {
                if (start_time == end_time || end_time <= Convert.ToDateTime("2001-01-01"))
                {
                    //Response.ContentType = "charset=utf-8";//不写的话会导致中文乱码
                    //Response.WriteAsync("<script type='text/javascript'>alert('请选择正确的开始与结束时间');location.href='/Proschedule/Report';</script>"); //如果没设定站位 则需要先设定站位
                    //return View("Views/Proschedule/alert-report.cshtml");
                    ViewBag.Message = "请选择正确的查询时间！";
                    ViewBag.Route = "Report";
                    return View("alert");
                }
                LoadRptList = machineErrorCodeLogManager.SelectMachineCodeLogRpt(start_time, end_time, machine_code);

                if (LoadRptList != null && LoadRptList.Count > 0)
                {
                    var pagedList = PagedList<MachineErrCodeLogForRpt>.PageList(1, 15, LoadRptList);
                    ViewBag.model = pagedList.Item2;
                    return View("Report", pagedList.Item1);
                }

                ViewBag.Message = "数据查询失败！";
                ViewBag.Route = "Report";
                return View("alert");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Message = "数据查询出错！";
                ViewBag.Route = "Report";
                return View("alert");
            }
        }
        /// <summary>
        /// 根据前端查询条件删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DelReport(DateTime start_time, DateTime end_time, string machine_code)
        {
            int count = 0;
            try
            {
                count = machineErrorCodeLogManager.Delete(start_time, end_time, machine_code);
                if (count > 0)
                {
                    return View("Report");
                }
                ViewBag.Message = "数据删除失败！";
                ViewBag.Route = "Report";
                return View("alert");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "数据删除出错！";
                ViewBag.Route = "Report";
                return View("alert");
            }
        }
    }
}