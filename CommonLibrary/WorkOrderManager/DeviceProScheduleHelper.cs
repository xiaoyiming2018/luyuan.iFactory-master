using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advantech.IFactory.CommonLibrary
{
    public class DeviceProScheduleHelper
    {
        private ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        private ProSchedulemachineLogManager proSchedulemachineLogManager = new ProSchedulemachineLogManager();
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private StationManager stationManager = new StationManager();
        private ProScheduleManager proScheduleManager = new ProScheduleManager();
        private TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        private ClassInfoManager classInfoManager = new ClassInfoManager();
        private SRPLogManager srpLogManager = new SRPLogManager();
        private SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
        private SystemConfigs flowConfig;//工艺配置
        /// <summary>
        /// 当前所有站位的队列，站位是配置完成后不变的信息
        /// </summary>
        private List<station_info> StationsList;//初始化加载所有的站位队列

        public DeviceProScheduleHelper()
        {
            StationsList = stationManager.SelectAll();//初始化加载所有的站位队列
            flowConfig = sysConfigsManager.Get("flow_mode");
        }
        /// <summary>
        /// 增加工单所关联下属的设备所有站位工单信息，由页面调用
        /// </summary>
        /// <param name="schedule">工单对象</param>
        /// <param name="AreaAttributeEnum"></param>
        public void AddDeviceProSchedule(pro_schedule schedule, TagAreaAttributeEnum AreaAttributeEnum = TagAreaAttributeEnum.station_info)
        {
            Pro_schedule_machine deviceSchedule = new Pro_schedule_machine();
            if (AreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                List<station_info> stationList = StationsList.Where(x => x.line_id == schedule.line_id).ToList();

                foreach (station_info station in stationList)
                {
                    if (station.status_no == "A")
                    {
                        //查询设备在制进度中是否有维护当前工单信息
                        Pro_schedule_machine obj = new Pro_schedule_machine();
                        obj = proScheduleMachineManager.SelectSingle(schedule.work_order, schedule.unit_no, station.station_name_en);
                        if (obj == null)
                        {
                            InsertDeviceProSchedule(schedule.work_order, schedule.part_num, station.station_id,
                                                    station.station_name_en, schedule.unit_no, schedule.standard_num,
                                                    schedule.order_status);
                        }
                    }
                }
            }
        }
        
        /// <summary>
        /// 此程序适用于自动模式下，前面一站的工单，会自动流转到下一站
        /// 获取当前站位/设备的在制工单.如果工单不存在，则插入。数量需要在外面计算
        /// </summary>
        /// <param name="deviceTagValueInfo"></param>
        /// <param name="AreaAttributeEnum"></param>
        /// <returns></returns>
        public Pro_schedule_machine GetDeviceSchedule(DeviceTagValueInfo deviceTagValueInfo,
                                                      TagAreaAttributeEnum AreaAttributeEnum = TagAreaAttributeEnum.station_info)
        {
            int sIndex = 0;
            station_info station = null;
            MachineInfo machine = null;
            Pro_schedule_machine curDeviceSchedule = null;//当前站工单
            Pro_schedule_machine preDeviceSchedule;//前一站工单

            try
            {
                if (AreaAttributeEnum == TagAreaAttributeEnum.machine_info)
                {
                    machine = machineInfoManager.SelectSingle(deviceTagValueInfo.device_id, null);
                }
                else if (AreaAttributeEnum == TagAreaAttributeEnum.station_info)
                {
                    if (flowConfig != null && flowConfig.GetIntValue() == (int)FlowModeEnum.Discrete)//离散型工艺，由工位自行开始，不允许自动插入
                    {
                        curDeviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(deviceTagValueInfo.device_code);
                    }
                    else
                    {
                        station = StationsList.FirstOrDefault(x => x.station_id == deviceTagValueInfo.device_id);
                        List<station_info> stations = StationsList.Where(x => x.line_id == station.line_id).ToList();
                        sIndex = stations.IndexOf(station);//取出站位的顺序号

                        if (sIndex <= 0)//第一站
                        {
                            curDeviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(deviceTagValueInfo.device_code);
                            if (curDeviceSchedule == null)//首站无工单，取出在线的插入
                            {
                                pro_schedule proSchedule = proScheduleManager.SelectFirstByLineInfo(station.line_id, (int)OrderStatusEnum.Excuting);
                                if (proSchedule != null && proSchedule.order_status == (int)OrderStatusEnum.Excuting)
                                {
                                    curDeviceSchedule = InsertDeviceProSchedule(proSchedule.work_order, proSchedule.part_num,
                                                                                station.station_id, station.station_name_en, station.unit_no,
                                                                                proSchedule.standard_num, proSchedule.order_status);
                                    //StartDeviceProSchedule()
                                }
                            }
                        }
                        else
                        {
                            //比较当前站与前一站的工单差别
                            preDeviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(stations[sIndex - 1].station_name_en);
                            curDeviceSchedule = proScheduleMachineManager.SelectOnLineWorkorder(deviceTagValueInfo.device_code);
                            if (curDeviceSchedule == null && preDeviceSchedule != null)//当前站位工单无数据,前一站有数据,取出前一站的插入
                            {
                                curDeviceSchedule = InsertDeviceProSchedule(preDeviceSchedule.work_order, preDeviceSchedule.part_num,
                                                                            station.station_id, station.station_name_en, station.unit_no,
                                                                            preDeviceSchedule.standard_num, preDeviceSchedule.order_status);


                            }
                            else if (curDeviceSchedule != null && preDeviceSchedule != null)//比较当前站位的工单是否与前一站的工单相同
                            {
                                if (preDeviceSchedule.actual_num > 0 &&
                                    preDeviceSchedule.work_order.Trim() != curDeviceSchedule.work_order.Trim())
                                {

                                    //工单不一致，发生变化，插入新的工单
                                    curDeviceSchedule = InsertDeviceProSchedule(preDeviceSchedule.work_order, preDeviceSchedule.part_num,
                                                                                station.station_id, station.station_name_en, station.unit_no,
                                                                                preDeviceSchedule.standard_num, preDeviceSchedule.order_status);

                                }
                                else if (preDeviceSchedule.actual_num > 0 &&
                                         preDeviceSchedule.work_order.Trim() != curDeviceSchedule.work_order.Trim())
                                {
                                    //工单与前一站一致，直接取出返回

                                }
                            }
                            else if (curDeviceSchedule == null && preDeviceSchedule == null)//当前站与前一站均无数据
                            {
                                pro_schedule proSchedule = proScheduleManager.SelectFirstByLineInfo(station.line_id, (int)OrderStatusEnum.Excuting);
                                if (proSchedule != null)
                                {
                                    curDeviceSchedule = InsertDeviceProSchedule(proSchedule.work_order, proSchedule.part_num,
                                                                                station.station_id, station.station_name_en, station.unit_no,
                                                                                proSchedule.standard_num, proSchedule.order_status);

                                    //前一站工单补缺
                                    InsertDeviceProSchedule(proSchedule.work_order, proSchedule.part_num,
                                                            stations[sIndex - 1].station_id, stations[sIndex - 1].station_name_en, stations[sIndex - 1].unit_no,
                                                            proSchedule.standard_num, proSchedule.order_status);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                srpLogManager.Insert("GetDeviceSchedule error=" + ex.Message + deviceTagValueInfo.device_code);
            }

            return curDeviceSchedule;
        }
        /// <summary>
        /// 开始主工单
        /// </summary>
        /// <returns></returns>
        public bool StartMainSchedule(pro_schedule ProSchedule)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            DateTime nowDate = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);

            if (ProSchedule != null && ProSchedule.order_status == (int)OrderStatusEnum.Scheduled)
            {
                ProSchedule.start_time = nowDate;
                ProSchedule.order_status = (int)OrderStatusEnum.Excuting;//主工单开始
                dic.Add("start_time", ProSchedule.start_time);
                dic.Add("order_status", ProSchedule.order_status);
                int count = proScheduleManager.Update(ProSchedule, dic);
                if (count > 0)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 结束主工单
        /// </summary>
        /// <returns></returns>
        public bool FinishMainSchedule(string work_order)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            DateTime nowDate = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            pro_schedule schedule = proScheduleManager.SelectByWorkOrder(work_order);
            if (schedule != null && schedule.order_status == (int)OrderStatusEnum.Excuting)
            {
                dic = new Dictionary<string, object>();
                schedule.order_status = (int)OrderStatusEnum.Finished;
                dic.Add("order_status", schedule.order_status);
                schedule.end_time = nowDate;
                dic.Add("end_time", schedule.end_time);
                if (proScheduleManager.Update(schedule, dic) > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 单个站位开始新的工单
        /// </summary>
        /// <param name="schedule">计划</param>
        public bool StartDeviceProSchedule(Pro_schedule_machine DeviceSchedule)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            DateTime nowDate = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);

            if (DeviceSchedule !=null && DeviceSchedule.order_status== (int)OrderStatusEnum.Scheduled)
            {
                DeviceSchedule.order_status = (int)OrderStatusEnum.Excuting;//切换到选中的工单
                DeviceSchedule.start_time = nowDate;
                dic.Add("order_status", DeviceSchedule.order_status);
                dic.Add("start_time", DeviceSchedule.start_time);
                if (proScheduleMachineManager.Update(DeviceSchedule,dic) > 0)
                {
                    srpLogManager.Insert("工单开始="+ DeviceSchedule.machine_code +" "+ DeviceSchedule.start_time);
                    return true;
                }
            }
            
            return false;
        }
        /// <summary>
        /// 设备或者工单完成信息
        /// </summary>
        /// <param name="DeviceSchedule">设备工单对象</param>
        /// <param name="FinishMainSchedule">是否结束主工单</param>
        public bool FinishDeviceProSchedule(Pro_schedule_machine DeviceSchedule,bool IsFinishMainSchedule=false)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            DateTime nowDate = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            DeviceSchedule.order_status = (int)OrderStatusEnum.Finished;//结束工位正在执行的工单 
            dic.Add("order_status", DeviceSchedule.order_status);
            DeviceSchedule.last_time = nowDate;
            dic.Add("last_time", DeviceSchedule.last_time);
            DeviceSchedule.end_time = nowDate;
            dic.Add("end_time", DeviceSchedule.end_time);
            if(proScheduleMachineManager.Update(DeviceSchedule, dic)>0)
            {
                if(IsFinishMainSchedule)//结束主工单
                {
                    return FinishMainSchedule(DeviceSchedule.work_order);
                }
                return true;
            }
            return false;
        }
        
        
        /// <summary>
        /// 关闭设备的当前工单
        /// </summary>
        /// <param name="device_code"></param>
        public bool CloseDeviceOnlineSchedule(string device_code,string unit_no,string work_order=null)
        {
            int count = 0;
            List<Pro_schedule_machine> list=new List<Pro_schedule_machine>();
            if(string.IsNullOrEmpty(work_order)==true)
            {
                list = proScheduleMachineManager.SelectSchedule(device_code, unit_no, (int)OrderStatusEnum.Excuting);
            }
            else
            {
                Pro_schedule_machine devSchedule = proScheduleMachineManager.SelectByWorkOrder(work_order, unit_no, device_code);
                if(devSchedule !=null)
                {
                    list.Add(devSchedule);
                }
            }

            if (list != null && list.Count>0)
            {
                foreach(var item in list)
                {
                    item.order_status = (int)OrderStatusEnum.Finished;
                    item.last_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    if(proScheduleMachineManager.Update(item)>0)
                    {
                        ++count;
                    }
                }
                if(count== list.Count)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        /// <summary>
        /// 计算换线时间
        /// </summary>
        /// <param name="NewDeviceSchedule">新的工单对象</param>
        public void CalOrderChangeTime(Pro_schedule_machine NewDeviceSchedule)
        {
            DateTime dateTimeNow = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            class_info objClassInfo = classInfoManager.SelectClass(dateTimeNow.ToString("yyyy-MM-dd HH:mm:ss"), dateTimeNow.ToString("yyyy-MM-dd"));
            List<Pro_schedule_machine> list = proScheduleMachineManager.SelectSchedule(NewDeviceSchedule.machine_code, NewDeviceSchedule.unit_no, (int)OrderStatusEnum.Finished);//查找上一个完成的工单
            if (list != null && list.Count>0)
            {
                Pro_schedule_machine lastSchedule = list[0];
                TimeSpan TSpan = (TimeSpan)(Convert.ToDateTime(NewDeviceSchedule.start_time) - Convert.ToDateTime(lastSchedule.last_time));
                if (TSpan.TotalSeconds > 0)
                {
                    Tricolor_tag_duration duration = new Tricolor_tag_duration();
                    duration.machine_code = NewDeviceSchedule.machine_code;
                    duration.station_id = NewDeviceSchedule.station_id;
                    duration.tag_code = SystemTagCodeEnum.workorder_change.ToString();//换线
                    duration.tag_status = false;
                    duration.whether = false;
                    duration.duration_second = (int)TSpan.TotalSeconds;
                    duration.insert_time = dateTimeNow;
                    duration.away_time = dateTimeNow;
                    tricolorTagDurationManager.Insert(duration);
                }
            }
        }
        /// <summary>
        /// 从tag值解析出工单信息
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="station_id"></param>
        /// <param name="tag_value">工单的信息,此信息为下位上抛的组合信息，需要进行拆分</param>
        /// <param name="insert_time"></param>
        public void ParsTagValueToSchedule(string machine_code, int station_id, string tag_value, DateTime insert_time)
        {
            try
            {
                List<string> objList = tag_value.Split('_').ToList();
                if (objList.Count == 6)
                {
                    string work_order = objList[0];
                    string part_num = objList[1];
                    string unit_no = objList[2];
                    int standard_num = Convert.ToInt32(objList[3]);
                    int actual_num = Convert.ToInt32(objList[4]);
                    //量产0试产1
                    int type = Convert.ToInt32(objList[5]);
                    //如果为试产，那么实际数量永远为零
                    if (type == 1)
                    {
                        actual_num = 0;
                    }
                    //添加Log日志
                    pro_schedule_machine_log objProScheduleMachineLog = new pro_schedule_machine_log();
                    objProScheduleMachineLog.work_order = work_order;
                    objProScheduleMachineLog.part_num = part_num;
                    objProScheduleMachineLog.unit_no = unit_no;
                    objProScheduleMachineLog.standard_num = standard_num;
                    objProScheduleMachineLog.actual_num = actual_num;
                    objProScheduleMachineLog.type = type;
                    objProScheduleMachineLog.ts = insert_time;
                    objProScheduleMachineLog.machine_code = machine_code;
                    objProScheduleMachineLog.station_id = station_id;
                    int count = proSchedulemachineLogManager.Insert(objProScheduleMachineLog);
                }
            }
            catch (Exception ex)
            {
                string dateone = System.DateTime.Now.ToString("yyyy-MM-dd");
            }
        }

        /// <summary>
        /// 插入在制工单.插入时，当前的实际数量默认为0，累加计算放在外部
        /// </summary>
        /// <param name="work_order">工单</param>
        /// <param name="part_num">机种</param>
        /// <param name="machine_code">设备</param>
        /// <param name="unit_no">制程</param>
        /// <param name="standard_num">标准数量</param>
        public Pro_schedule_machine InsertDeviceProSchedule(string work_order, string part_num,
                                                            int station_id, string machine_code,
                                                            string unit_no, double standard_num,
                                                            int OrderStatus=(int) OrderStatusEnum.Excuting)
        {
            try
            {
                //添加Log日志
                DateTime dtNow = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                Pro_schedule_machine deviceSchedule = new Pro_schedule_machine();
                if (OrderStatus == (int)OrderStatusEnum.Excuting)//如果新增的计划是执行，则关闭之前的工单
                {
                    CloseDeviceOnlineSchedule(machine_code, unit_no);  //关闭设备的上一个在线工单，设备均允许当前一个在线工单
                    deviceSchedule.start_time = dtNow;
                    deviceSchedule.last_time = dtNow;
                }
                
                deviceSchedule.work_order = work_order;
                deviceSchedule.part_num = part_num;
                deviceSchedule.station_id = station_id;
                deviceSchedule.machine_code = machine_code;
                deviceSchedule.unit_no = unit_no;
                deviceSchedule.standard_num = standard_num;
                deviceSchedule.actual_num = 0;//当前实际数量为0，累加计算放在外部
                deviceSchedule.order_status = OrderStatus;
                deviceSchedule.insert_time = dtNow;
                //执行插入站位或者设备的新的工单数据
                int count = proScheduleMachineManager.Insert(deviceSchedule);
                if (count > 0)
                {
                    CalOrderChangeTime(deviceSchedule);//计算换线时间.当前班次内上一个工单结束与当前工单开始的时间差值
                    return deviceSchedule;
                }
            }
            catch (Exception ex)
            {
                string dateone = System.DateTime.Now.ToString("yyyy-MM-dd");
                srpLogManager.Insert("InsertDeviceProSchedule error=" + ex.Message + work_order);
            }
            return null;
        }
    }
}
