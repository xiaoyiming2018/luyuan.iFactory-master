using System;
using System.Linq;
using System.Collections.Generic;
using Advantech.IFactory.Andon;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ErrorLogController : BaseController
    {
        ErrorLogManager errorLogManager = new ErrorLogManager();
        ErrorConfigManager errorConfigManager = new ErrorConfigManager();
        ErrorTypeDetailsManager errorTypeDetailsManager = new ErrorTypeDetailsManager();//异常日志
        ErrorQualityLogManager errorQualityLogManager= new ErrorQualityLogManager();//品质异常
        ProScheduleManager proScheduleManager = new ProScheduleManager();
        PersonManager personManager = new PersonManager();
        ErrorTypeManager errorTypeManager = new ErrorTypeManager();
        ErrorLogPersonManager errorLogPersonManager = new ErrorLogPersonManager();
       
        /// <summary>
        /// 查询到的信息列表
        /// </summary>
        private static List<ErrorLogRpt> LoadRptList = new List<ErrorLogRpt>();
        private static DateTime rptStartTime, rptEndTime;
        private static string rptDeviceCode;
        private static string indexDeviceCode;
        /// <summary>
        /// Index页面导航列表
        /// </summary>
        private static List<error_log> LoadLogList = new List<error_log>();
        //
        // GET: /Area/
        public ActionResult Index()
        {
            LoadLogList = errorLogManager.GetUnMaintainByDeviceCode();
            var pagedList = PagedList<error_log>.PageList(1, 15, LoadLogList);
            ViewBag.model = pagedList.Item2;
            ViewBag.device_code = indexDeviceCode;
            return View(pagedList.Item1);
        }
        /// <summary>
        /// 分页导航跳转
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        public ActionResult IndexPageNav(int pageindex = 1, int pagesize = 15)
        {
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.device_code = rptDeviceCode;
            var pagedList = PagedList<error_log>.PageList(pageindex, pagesize, LoadLogList);
            ViewBag.model = pagedList.Item2;
            ViewBag.device_code = indexDeviceCode;
            return View("Index", pagedList.Item1);
        }
        
        /// <summary>
        /// 指定设备查询
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public ActionResult DeviceQuery(string device_code)
        {
            if (device_code != null)
            {
                LoadLogList = errorLogManager.GetUnMaintainByDeviceCode(device_code.Trim());
            }
            else
            {
                LoadLogList = errorLogManager.GetUnMaintainByDeviceCode();
            }
            var pagedList = PagedList<error_log>.PageList(1, 15, LoadLogList);
            indexDeviceCode = device_code;
            ViewBag.model = pagedList.Item2;
            ViewBag.device_code = indexDeviceCode;
            return View("Index", pagedList.Item1);
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
                int count = errorLogManager.Del(id);
                if (count > 0)
                {
                    ViewBag.Message = "删除数据成功！";
                    ViewBag.Route = "Index";
                    return View("alert");
                }
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            ViewBag.Message = "删除数据失败！";
            ViewBag.Route = "Index";
            return View("alert");
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
                    var obj = errorLogManager.GetErrorLogById(id);
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
        public ActionResult EditHandle(error_log LogModel)
        {
            int count1 = 0;
            int count2 = 0;
            int count3 = 0;
            DateTime dt= DateTime.Now.AddHours(GlobalDefine.SysTimeZone);

            error_config errCfg = errorConfigManager.GetFirstErrorConfig(LogModel.system_tag_code, LogModel.unit_no, LogModel.line_id);
            if (errCfg == null)
            {
                errCfg = errorConfigManager.GetFirstErrorConfig(LogModel.system_tag_code, LogModel.unit_no);//更大单位查找
            }
            try
            {
                if (errCfg != null)
                {
                    if (errCfg.check_ack == (int)AckModeEnum.WebRegisterAck)
                    {
                        LogModel.release_time = dt;
                        LogModel.maintenance_time = dt;
                        AndonMainTask.HandleErrorAck(LogModel, errCfg);//配置为web页面解除的
                    }
                    else
                    {
                        if (LogModel.id > 0)
                        {
                            LogModel.maintenance_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                            count1 = errorLogManager.Update(LogModel);
                        }
                    }

                    error_quality_log qualityLog = new error_quality_log();
                    qualityLog = errorQualityLogManager.GetQualityLog(LogModel.id);
                    if (qualityLog == null)
                    {
                        qualityLog = new error_quality_log();
                    }
                    qualityLog.error_log_id = LogModel.id;
                    qualityLog.error_type_id = LogModel.error_type_id;
                    qualityLog.station_id = LogModel.station_id;
                    qualityLog.machine_code = LogModel.machine_code;
                    qualityLog.defectives_count = LogModel.defectives_count;
                    qualityLog.quality_description = HttpContext.Request.Form["quality_description"];
                    qualityLog.quality_reason = HttpContext.Request.Form["quality_reason"];
                    qualityLog.improve_plan = HttpContext.Request.Form["improve_plan"];
                    if (HttpContext.Request.Form["plan_date"].ToString().Length > 0)
                    {
                        DateTime.TryParse(HttpContext.Request.Form["plan_date"], out dt);
                        qualityLog.plan_date = dt;
                    }

                    qualityLog.responsible_person = HttpContext.Request.Form["responsible_person"];
                    qualityLog.remark = LogModel.remark;
                    qualityLog.insert_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    if (qualityLog.id > 0)
                    {
                        count2 = errorQualityLogManager.Update(qualityLog);
                    }
                    else
                    {
                        count2 = errorQualityLogManager.Insert(qualityLog);//插入到品质异常表
                    }

                    //更新工单表-不良数量
                    if (LogModel.work_order.Length > 0)
                    {
                        pro_schedule res = proScheduleManager.SelectByWorkOrder(LogModel.work_order);
                        if (res != null)
                        {
                            res.defectives_count = res.defectives_count + LogModel.defectives_count;
                            count3 = proScheduleManager.Update(res);
                        }
                    }
                    ViewBag.Message = "提交数据成功！";
                    ViewBag.Route = "Index";
                    return View("alert");
                }
            }
            catch(Exception ex)
            {
            }
            ViewBag.Message = "提交数据失败！";
            ViewBag.Route = "Edit";
            return View("alert");
        }

        /// <summary>
        /// 请求详细的类别
        /// </summary>
        /// <returns></returns>
        public ActionResult GetErrorDetails(int type_id)
        {
            ViewBag.detailsList = errorTypeDetailsManager.SelectbyErrorid(type_id);
            return View();
        }

        //
        // 报表查询进入页面
        public ActionResult Report()
        {
            ViewBag.start_time = DateTime.Now;
            ViewBag.end_time = DateTime.Now.AddDays(1);
            ViewBag.device_code = "";
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
            ViewBag.device_code = rptDeviceCode;
            var pagedList = PagedList<ErrorLogRpt>.PageList(pageindex, pagesize, LoadRptList);
            ViewBag.model = pagedList.Item2;
            return View("Report", pagedList.Item1);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportQuery(DateTime start_time, DateTime end_time, string device_code)
        {
            //List<pro_schedule> list = new List<pro_schedule>();
            rptStartTime = start_time;
            rptEndTime = end_time;
            rptDeviceCode = device_code;
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.device_code = rptDeviceCode;
            LoadRptList = new List<ErrorLogRpt>();

            try
            {
                if (start_time == end_time || end_time <= Convert.ToDateTime("2001-01-01"))
                {
                    return View("alert-report-time.cshtml");
                }
                List<error_log> logList = errorLogManager.GetAllLogByTime(start_time, end_time, device_code);
                List<person> personList = personManager.SelectAll();
                List<error_type> typeList = errorTypeManager.SelectAll();
                List<error_type_details> detailList = errorTypeDetailsManager.SelectAll();

                foreach (var logItem in logList)
                {
                    ErrorLogRpt errorLogRpt = new ErrorLogRpt();
                    #region
                    errorLogRpt.id = logItem.id;
                    errorLogRpt.ack_person_id = logItem.ack_person_id;
                    errorLogRpt.arrival_person_id = logItem.arrival_person_id;
                    errorLogRpt.arrival_time = logItem.arrival_time;
                    errorLogRpt.error_name = logItem.error_name;
                    errorLogRpt.machine_code = logItem.machine_code;
                    errorLogRpt.maintenance_time = logItem.maintenance_time;
                    errorLogRpt.pn = logItem.pn;
                    errorLogRpt.release_time = logItem.release_time;
                    errorLogRpt.remark = logItem.remark;
                    errorLogRpt.start_time = logItem.start_time;
                    errorLogRpt.station_id = logItem.station_id;
                    errorLogRpt.system_tag_code = logItem.system_tag_code;
                    errorLogRpt.work_order = logItem.work_order;
                    #endregion
                    person p = personList.FirstOrDefault(x => x.id == logItem.arrival_person_id);
                    if (p != null)
                    {
                        errorLogRpt.arrival_person_name = p.user_name;
                    }
                    p = personList.FirstOrDefault(x => x.id == logItem.ack_person_id);
                    if (p != null)
                    {
                        errorLogRpt.ack_person_name = p.user_name;
                    }
                    error_type_details detail = detailList.FirstOrDefault(x => x.id == logItem.error_type_id);
                    if (detail != null)
                    {
                        errorLogRpt.error_code_name_cn = detail.code_name_cn;
                        error_type type = typeList.FirstOrDefault(x => x.id == detail.type_id);
                        if (type != null)
                        {
                            errorLogRpt.error_type_name = type.type_name;
                        }
                    }
                    LoadRptList.Add(errorLogRpt);
                }

                if (LoadRptList != null && LoadRptList.Count > 0)
                {
                    var pagedList = PagedList<ErrorLogRpt>.PageList(1, 15, LoadRptList);
                    ViewBag.model = pagedList.Item2;
                    return View("Report", pagedList.Item1);
                }

                //HttpContext.Response.ContentType = "charset=utf-8";//不写的话会导致中文乱码
                //HttpContext.Response.WriteAsync("<script type='text/javascript'>alert('查询失败，将返回默认数据');location.href='/Proschedule/Report';</script>"); //如果没设定站位 则需要先设定站位
                //return View("Report");
                return View("Report");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            ViewBag.Message = "查询数据失败！";
            ViewBag.Route = "Report";
            return View("alert");
        }
        /// <summary>
        /// 根据前端查询条件删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DelReport(DateTime start_time, DateTime end_time, string device_code)
        {
            int count = 0;

            try
            {
                count = errorLogManager.Del(start_time, end_time, device_code);
                if (count > 0)
                {
                    errorLogPersonManager.Del(start_time, end_time);
                    ViewBag.Message = "删除数据成功！";
                    ViewBag.Route = "Report";
                    return View("alert");
                }
            }
            catch (Exception ex)
            {
                //return View("alert-delete-error");
            }
            ViewBag.Message = "删除数据失败！";
            ViewBag.Route = "Report";
            return View("alert");
        }
    }
}