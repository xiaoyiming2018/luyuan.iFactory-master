using System;
using System.Collections.Generic;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class ProscheduleController : BaseController
    {
        ProScheduleManager proScheduleManager = new ProScheduleManager();
        ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        DeviceProScheduleHelper deviceProScheduleHelper = new DeviceProScheduleHelper();
        LineInfoManager lineInfoManager = new LineInfoManager();
        ClassInfoManager classInfoManager = new ClassInfoManager();
        /// <summary>
        /// 查询到的工单信息列表
        /// </summary>
        private static List<pro_schedule> LoadSchduleList = new List<pro_schedule>();
        private DateTime rptStartTime, rptEndTime;
        private static string rptWorkOrder;
        /// <summary>
        /// 工单页面上的复合工单
        /// </summary>
        private static List<ProScheduleObject> ScheduleList=new List<ProScheduleObject>();
        
        //
        // GET: /Area/
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = proScheduleManager.SelectUnFinished();
            List<class_info> classList = classInfoManager.SelectAll();
            ViewBag.ClassList = classList;
            ScheduleList = new List<ProScheduleObject>();
            foreach (var item in objList)
            {
                ProScheduleObject proScheduleObject = new ProScheduleObject();
                proScheduleObject.SchduleObject = item;
                proScheduleObject.LineObject = lineInfoManager.SelectSingle(item.line_id);
                ScheduleList.Add(proScheduleObject);
            }
            var pagedList = PagedList<ProScheduleObject>.PageList(pageindex, pagesize, ScheduleList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }
        //
        // 报表查询进入页面
        public ActionResult Report(int line_id, int pageindex = 1, int pagesize = 15)
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
        public ActionResult SchdulePageNav(int pageindex = 1, int pagesize = 15)
        {
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.work_order = rptWorkOrder;
            var pagedList = PagedList<pro_schedule>.PageList(pageindex, pagesize, LoadSchduleList);
            ViewBag.model = pagedList.Item2;
            return View("Report", pagedList.Item1);
        }
        /// <summary>
        /// 查询数据
        /// </summary>
        /// <returns></returns>
        public ActionResult ReportQuery(DateTime start_time, DateTime end_time, string work_order)
        {
            //List<pro_schedule> list = new List<pro_schedule>();
            rptStartTime= start_time;
            rptEndTime = end_time;
            rptWorkOrder = work_order;
            ViewBag.start_time = rptStartTime;
            ViewBag.end_time = rptEndTime;
            ViewBag.work_order = rptWorkOrder;
            LoadSchduleList = new List<pro_schedule>();

            try
            {
                if (start_time == end_time || end_time <= Convert.ToDateTime("2001-01-01"))
                {
                    ViewBag.Message = "请选择正确的查询时间！";
                    ViewBag.Route = "Report";
                    return View("alert");
                }
                if (string.IsNullOrEmpty(work_order) == false)
                {
                    pro_schedule schedule = proScheduleManager.SelectByWorkOrder(work_order);
                    if (schedule != null)
                    {
                        LoadSchduleList.Add(schedule);
                    }
                }
                else
                {
                    LoadSchduleList = proScheduleManager.SelectScheduleByTime(start_time, end_time);
                }
                if (LoadSchduleList != null && LoadSchduleList.Count > 0)
                {
                    var pagedList = PagedList<pro_schedule>.PageList(1, 15, LoadSchduleList);
                    ViewBag.model = pagedList.Item2;
                    return View("Report", pagedList.Item1);
                }
                ViewBag.Message = "未查询到数据，请更换时间再试！";
                ViewBag.Route = "Report";
                return View("alert");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                ViewBag.Message = "查询数据出错，请更换时间再试！";
                ViewBag.Route = "Report";
                return View("alert");
            }

        }
        /// <summary>
        /// 根据前端查询条件删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult DelReport(DateTime start_time, DateTime end_time, string work_order)
        {
            int count = 0;

            try
            {
                if (string.IsNullOrEmpty(work_order) == false)
                {
                    pro_schedule schedule = proScheduleManager.SelectByWorkOrder(work_order);
                    if (schedule != null && schedule.order_status != (int)OrderStatusEnum.Excuting)
                    {
                        count = proScheduleManager.Del(schedule.id);
                        if (count > 0)
                        {
                            count = proScheduleMachineManager.DeleteByWorkOrder(schedule.work_order);
                            if (count > 0)
                            {
                                return View();
                            }
                        }
                    }
                }
                else
                {
                    List<pro_schedule> list = proScheduleManager.SelectScheduleByTime(start_time, end_time);
                    if (list.Count > 0)
                    {
                        foreach (var item in list)
                        {
                            count = proScheduleManager.Del(item.id);
                            if (count > 0)
                            {
                                count = proScheduleMachineManager.DeleteByWorkOrder(item.work_order);
                            }
                        }
                        return View();
                    }
                }

                ViewBag.Message = "删除数据失败，请重试！";
                ViewBag.Route = "Report";
                return View("alert");
            }
            catch (Exception ex)
            {
                ViewBag.Message = "删除数据出错，请重试！";
                ViewBag.Route = "Report";
                return View("alert");
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {
            int count1=0, count2 = 0;

            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                pro_schedule schedule = proScheduleManager.SelectByID(id);
                if (schedule != null && schedule.order_status != (int)OrderStatusEnum.Excuting)
                {
                    count1 = proScheduleManager.Del(id);
                    if (count1 > 0)
                    {
                        count2 = proScheduleMachineManager.DeleteByWorkOrder(schedule.work_order);
                        if (count2 > 0)
                        {

                        }
                        ViewBag.Message = "删除数据成功！";
                        ViewBag.Route = "Index";
                        return View("alert");
                    }
                }
            }
            catch (Exception ex)
            {
                
            }
            ViewBag.Message = "删除数据出错，请重试！";
            ViewBag.Route = "Index";
            return View("alert");
        }

        /// <summary>
        /// 进入新增页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                List<class_info> classList= classInfoManager.SelectAll();
                List<line_info>lineList= lineInfoManager.SelectAll();
                ViewBag.ClassList = classList;
                ViewBag.LineList = lineList;
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    ViewBag.line_id = Convert.ToInt32(Request.Query["line_id"]);
                    pro_schedule schedule = proScheduleManager.SelectByID(id);
                    return View(schedule);
                }
                else
                {
                    pro_schedule schedule = new pro_schedule();
                    return View(schedule);
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
        public ActionResult EditHandle(pro_schedule schdule)
        {
            int count = 0;
            Dictionary<string, Object> Dic = new Dictionary<string, object>();
            pro_schedule dbSchedule = proScheduleManager.SelectByWorkOrder(schdule.work_order);
            line_info line= lineInfoManager.SelectSingle(schdule.line_id);
            if(line !=null)
            {
                schdule.unit_no = line.unit_no;
            }
            else
            {
                ViewBag.Message = "线别信息查找失败，请重新编辑！";
                ViewBag.Route = "Edit";
                return View("alert");
            }
            
            if (schdule.id>0)
            {
                if (schdule.order_status == (int)OrderStatusEnum.Aborted)//状态修改为取消，同步修改工位工单
                {
                    List<Pro_schedule_machine> list = proScheduleMachineManager.SelectByWorkOrder(schdule.work_order, schdule.unit_no);
                    foreach (var item in list)
                    {
                        Dic = new Dictionary<string, object>();
                        item.order_status = (int)OrderStatusEnum.Aborted;
                        Dic.Add("order_status", item.order_status);
                        proScheduleMachineManager.Update(item, Dic);
                    }
                    count = proScheduleManager.Update(schdule);
                }
                else//状态变为取消，将工位工单的状态变更为取消
                {
                    count = proScheduleManager.Update(schdule);
                    proScheduleMachineManager.DeleteByWorkOrder(schdule.work_order);//直接先删除
                    deviceProScheduleHelper.AddDeviceProSchedule(schdule);//重新插入到数据库
                }
            }
            else
            {
                schdule.insert_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);//创建时间
                if (schdule.order_index == 0)
                {
                    pro_schedule schedule = proScheduleManager.GetMaxIndexSchedule();
                    schdule.order_index = 1;
                    if (schedule != null)
                    {
                        schdule.order_index = schedule.order_index + 1;
                    }
                }
                
                if (dbSchedule == null)
                {
                    count = proScheduleManager.Insert(schdule);
                    deviceProScheduleHelper.AddDeviceProSchedule(schdule);
                }
                else
                {
                    ViewBag.Message = "工单号码重复，请重新输入工单！";
                    ViewBag.Route = "Edit";
                    return View("alert");
                }
            }

            if (count > 0)
            {
                ViewBag.Message = "数据提交成功！";
                ViewBag.Route = "Index";
                return View("alert");
            }
            else
            {
                ViewBag.Message = "数据提交失败！";
                ViewBag.Route = "Index";
                return View("alert");
            }
        }
	}
}