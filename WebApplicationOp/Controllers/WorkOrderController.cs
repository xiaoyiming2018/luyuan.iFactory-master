using System;
using System.Collections.Generic;
using System.Linq;
using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.WebCommonLibrary;
using iFactory.Op.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplicationForOp.Common;

namespace iFactory.Op.Controllers
{
    public class WorkOrderController : BaseController
    {
        ProScheduleManager proScheduleManager = new ProScheduleManager();
        ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        client_config_info clientCfg;
        Pro_schedule_machine deviceSchedule;
        DeviceProScheduleHelper deviceProScheduleHelper = new DeviceProScheduleHelper();
        /// <summary>
        /// 当前制程和线别下的站位列表
        /// </summary>
        List<station_info> CurrentStations;

        public IActionResult Index()
        {
            clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            
            GlobalCfgData.LoadInitialData(clientCfg.line_id);
            CurrentStations= GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).OrderBy(y => y.station_id).ToList();
            station_info station = CurrentStations.FirstOrDefault(x => x.station_id == clientCfg.station_id);
            if (clientCfg.station_id > 0 && station!=null)
            {
                ViewBag.station_name = station.station_name_en;
                ViewBag.SysFlowConfig = GlobalCfgData.SysFlowConfig;
                
                DateTime nowDate = DateTime.Now.AddHours(GlobalCfgData.SysTimeZone);
                deviceSchedule= proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);
                if(deviceSchedule==null)
                {
                    deviceSchedule = new Pro_schedule_machine();
                }
                ViewBag.Order = deviceSchedule;
                List<Pro_schedule_machine> list = new List<Pro_schedule_machine>();
                list = proScheduleMachineManager.SelectUnFinishedSchedule(station.station_name_en, clientCfg.unit_no);

                return View(list);
            }
            else
            {
                //Response.ContentType = "charset=utf-8";//不写的话会导致中文乱码
                //Response.WriteAsync("<script type='text/javascript'>alert('请先配置站位信息');location.href='/Configuration/Index';</script>"); //如果没设定站位 则需要先设定站位
                //return View();
                return View("Views/Configuration/alert.cshtml");
            }
        }

        /// <summary>
        /// 工单执行开始
        /// </summary>
        /// <param name="wo_name"></param>
        /// <param name="wo_id"></param>
        /// <returns></returns>
        public IActionResult ProScheduleStart(int order_id)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            List<Pro_schedule_machine> list = new List<Pro_schedule_machine>();
            bool ret = false,changeOrder=false;
            DateTime nowDate = DateTime.Now.AddHours(GlobalCfgData.SysTimeZone);
            pro_schedule selectedProSchedule=null;
            Pro_schedule_machine selectedDevSchedule = proScheduleMachineManager.SelectById(order_id);
            CurrentStations = GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).OrderBy(y => y.station_id).ToList();
            station_info station = CurrentStations.FirstOrDefault(x => x.station_id == clientCfg.station_id);

            if (station==null || selectedDevSchedule==null || selectedDevSchedule.order_status != (int)OrderStatusEnum.Scheduled)
            {
                return Json("device_order_err");//查找不到选中的工单，或者选中的工单不在排产状态，返回
            }
            if (selectedDevSchedule != null)
            {
                selectedProSchedule = proScheduleManager.SelectByWorkOrder(selectedDevSchedule.work_order);//根据设备工单查找主工单
            }
            if(selectedProSchedule==null)
            {
                return Json("main_order_err");//查找不到主工单，返回
            }
            List<pro_schedule> orderList = proScheduleManager.SelectByLineInfo(clientCfg.line_id, (int)OrderStatusEnum.Excuting);
            if (orderList.Count >= 2 && !orderList.Any(x=>x.work_order== selectedProSchedule.work_order))
            {
                return Json("order_num_err");//执行的数量已经有2个，不允许同时执行2个以上的工单，返回
            }

            deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);//当前工位在线工单
            if (deviceSchedule == null)
            {
                deviceSchedule = new Pro_schedule_machine();
            }
            ViewBag.Order = deviceSchedule;
            ViewBag.station_name = station.station_name_en;
            ViewBag.SysFlowConfig = GlobalCfgData.SysFlowConfig;

            if (station.station_id == CurrentStations[0].station_id)//只允许第一站下发开始命令
            {
                 changeOrder = deviceProScheduleHelper.StartMainSchedule(selectedProSchedule);//开始主工单
            }
            else            //首站已经开始执行，其他站点击的开始
            {
                if (GlobalCfgData.SysFlowConfig == (int)FlowModeEnum.Discrete &&
                    selectedProSchedule.order_status == (int)OrderStatusEnum.Excuting)//连续型，工单按照各自站位操作
                {
                    //查找前一站，检查是否已经开始
                    Pro_schedule_machine preDevSchedule = GetPreviousStationSchedule(station, selectedDevSchedule.work_order);
                    if (preDevSchedule == null)
                    {
                        return Json("excute_index_err");//前一站工单没开始，当前工位不能开始
                    }
                    else if (preDevSchedule.order_status != (int)OrderStatusEnum.Excuting && 
                             preDevSchedule.order_status != (int)OrderStatusEnum.Finished)
                    {
                        return Json("excute_index_err");//前一站工单没开始，当前工位不能开始
                    }
                    else
                    {
                        changeOrder = true;//可以切换工单
                    }
                }
            }
            if (changeOrder == true)//当前工位切换工单
            {
                bool excutBit = true;
                if (deviceSchedule !=null && deviceSchedule.order_status==(int)OrderStatusEnum.Excuting)
                {
                    if (station.station_id == CurrentStations[CurrentStations.Count - 1].station_id)//最后一站
                    {
                        excutBit = deviceProScheduleHelper.FinishDeviceProSchedule(deviceSchedule,true);//结束工位工单与上一个主工单
                    }
                    else
                    {
                        excutBit = deviceProScheduleHelper.FinishDeviceProSchedule(deviceSchedule);//结束当前工位工单
                    }
                }
               
                if (excutBit)
                {
                    ret= deviceProScheduleHelper.StartDeviceProSchedule(selectedDevSchedule);//开始工位工单
                }
            }

            if (ret)
            {
                return Json("Success");
            }
            return Json("Fail");
        }
        /// <summary>
        /// 工单完成结束,只能针对当前工位正在执行的进行操作
        /// </summary>
        /// <returns></returns>
        public IActionResult WoFinish()
        {
            clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            bool ret = false;
            DateTime nowDate = DateTime.Now.AddHours(GlobalCfgData.SysTimeZone);
            pro_schedule proSchedule = null;
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息

            CurrentStations = GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).OrderBy(y => y.station_id).ToList();
            station_info station = CurrentStations.FirstOrDefault(x => x.station_id == clientCfg.station_id);
            if(station==null)
            {
                return Json("system_config_err");//系统配置错误
            }
            //查找工位上正在执行的工单
            deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);
            if (deviceSchedule == null || deviceSchedule.order_status !=(int)OrderStatusEnum.Excuting)//工位工单未找到或未执行
            {
                deviceSchedule = new Pro_schedule_machine();
                ViewBag.Order = deviceSchedule;
                return Json("device_order_err");
            }
            ViewBag.Order = deviceSchedule;
            ViewBag.station_name = station.station_name_en;
            ViewBag.SysFlowConfig = GlobalCfgData.SysFlowConfig;
            proSchedule = proScheduleManager.SelectByWorkOrder(deviceSchedule.work_order);
            if(proSchedule==null || proSchedule.order_status!= (int)OrderStatusEnum.Excuting)//主工单查找失败或者未在执行中
            {
                return Json("main_order_err");
            }

            if (station.station_id == CurrentStations[CurrentStations.Count - 1].station_id)//最后一站结束工单
            {
                if (deviceProScheduleHelper.FinishDeviceProSchedule(deviceSchedule, true) == true)//结束工位工单与主工单
                {
                    for (int i = 0; i < CurrentStations.Count - 1; i++)//检查其他工位是否有执行的工单，直接全部结束
                    {
                        Pro_schedule_machine devSchedule = proScheduleMachineManager.SelectByWorkOrder(deviceSchedule.work_order, deviceSchedule.unit_no, CurrentStations[i].station_name_en);
                        if (devSchedule != null && devSchedule.order_status == (int)OrderStatusEnum.Excuting)
                        {
                            deviceProScheduleHelper.FinishDeviceProSchedule(devSchedule);//结束工位工单
                        }
                    }
                    ret = true;
                }
            }
            else if (station.station_id == CurrentStations[0].station_id)//第一站,可以直接结束
            {
                ret = deviceProScheduleHelper.FinishDeviceProSchedule(deviceSchedule);
            }
            else//首尾中间站，对其他站的当前工单结束
            {
                if (GlobalCfgData.SysFlowConfig == (int)FlowModeEnum.Discrete &&
                     proSchedule != null)//连续型，工单按照各自站位操作
                {
                    //查找前一站，检查是否已经结束
                    Pro_schedule_machine preDevSchedule = GetPreviousStationSchedule(station, deviceSchedule.work_order);
                    if (preDevSchedule != null && preDevSchedule.order_status == (int)OrderStatusEnum.Excuting)
                    {
                        return Json("excute_index_err");//前一站仍在执行中，当前不能结束
                    }
                    else
                    {
                        ret = deviceProScheduleHelper.FinishDeviceProSchedule(deviceSchedule);
                    }
                }
            }

            if (ret)
            {
                return Json("Success");
            }
            return Json("Fail");
        }
        /// <summary>
        /// 周期刷新，从本地抓取
        /// </summary>
        /// <returns></returns>
        public IActionResult Refresh()
        {
            clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            station_info station;
            DateTime nowDate = DateTime.Now.AddHours(GlobalCfgData.SysTimeZone); 
            List<Pro_schedule_machine> list = new List<Pro_schedule_machine>();
            try
            {
                CurrentStations = GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).OrderBy(y => y.station_id).ToList();
                station = CurrentStations.FirstOrDefault(x => x.station_id == clientCfg.station_id);

                ViewBag.station_name = station.station_name_en;
                ViewBag.SysFlowConfig = GlobalCfgData.SysFlowConfig;

                //deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);
                deviceSchedule = GlobalCfgData.UnFinishedDevSchedules.FirstOrDefault(x=>x.machine_code== station.station_name_en && 
                                                                                         x.order_status==(int)OrderStatusEnum.Excuting);

                if (deviceSchedule == null)
                {
                    deviceSchedule = new Pro_schedule_machine();
                    ViewBag.CostTime = "0";
                }
                else
                {
                    TimeSpan ts = nowDate - deviceSchedule.start_time; //计算时间差
                    ViewBag.CostTime = Math.Round(ts.TotalHours, 1).ToString();
                }
                ViewBag.Order = deviceSchedule;
                if (station.station_id == CurrentStations[CurrentStations.Count - 1].station_id)//最后一站
                {
                    ViewBag.FirstStation = false;
                    ViewBag.EndStation = true;
                }
                else if (station.station_id == CurrentStations[0].station_id)//首站
                {
                    ViewBag.FirstStation = true;
                    ViewBag.EndStation = false;
                }
                if (clientCfg.line_id >= 0 && clientCfg.unit_no != null)
                {
                    //list = proScheduleMachineManager.SelectUnFinishedSchedule(station.station_name_en, clientCfg.unit_no);
                    list = GlobalCfgData.UnFinishedDevSchedules.Where(x => x.machine_code == station.station_name_en &&
                                                                      x.order_status== (int)OrderStatusEnum.Scheduled).ToList();//从本地获取
                }
                //Console.WriteLine("Refresh() finish"+ station.station_name_en+ DateTime.Now);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Refresh() error=" + ex.Message);
            }
            return View(list);
        }
        /// <summary>
        /// 实际数量编辑,只能编辑当前在线的工单
        /// </summary>
        /// <param name="actual_num"></param>
        /// <param name="select_id"></param>
        /// <param name="wo_name"></param>
        /// <returns></returns>
        public IActionResult Edit(int actual_num)
        {
            clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            bool ret = false;
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            pro_schedule proSchedule = null;
            CurrentStations = GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).OrderBy(y => y.station_id).ToList();
            station_info station = CurrentStations.FirstOrDefault(x => x.station_id == clientCfg.station_id);
            Pro_schedule_machine deviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);
            
            if (WebUserManager.Current.GetLevel < UserLevelEnum.Manager)
            {
                return View("alert-error");
            }
            if (actual_num>0 && deviceSchedule != null)
            {
                proSchedule = proScheduleManager.SelectByWorkOrder(deviceSchedule.work_order);

                deviceSchedule.actual_num = actual_num;          //设备的工单数量更新
                dic.Add("actual_num", deviceSchedule.actual_num);
                if (station.convert_multiplier!=0 )
                {
                    deviceSchedule.raw_actual_num = (int)(actual_num / station.convert_multiplier);
                    dic.Add("raw_actual_num", deviceSchedule.raw_actual_num);
                }
                proScheduleMachineManager.Update(deviceSchedule, dic);

                //最后一站修改工单数量
                if (station.station_id == CurrentStations[CurrentStations.Count - 1].station_id)//最后一站才允许结束工单
                {
                    if (proSchedule != null)
                    {
                        dic = new Dictionary<string, object>();
                        proSchedule.actual_num = actual_num;//更新整个工单数量
                        dic.Add("actual_num", proSchedule.actual_num);
                        if (proScheduleManager.Update(proSchedule, dic) > 0)
                        {
                            ret = true;
                        }
                    }
                }
                else
                {
                    ret = true;
                }
            }
            if (ret)
            {
                return Json("Success");
            }
            return Json("Fail");
        }
        /// <summary>
        /// 获取前一站的指定工单
        /// </summary>
        /// <returns></returns>
        private Pro_schedule_machine GetPreviousStationSchedule(station_info station,string work_order)
        {
            int index = 0;
            List<station_info> stations;
            //当前工艺下的站位
            stations = GlobalCfgData.StationsList.Where(x => x.line_id == station.line_id).OrderBy(y => y.station_id).ToList();
            
            index=stations.IndexOf(station)-1;
            if (index>=0)
            {
                Pro_schedule_machine deviceSchedule = proScheduleMachineManager.SelectByWorkOrder(work_order, stations[index].unit_no,stations[index].station_name_en);
                return deviceSchedule;
            }
            return null;
        }
    }

}