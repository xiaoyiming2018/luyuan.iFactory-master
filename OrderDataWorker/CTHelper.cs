using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advantech.IFactory.WorkOrderManage
{
    public class CTHelper
    {
        private static CTLogManager ctLogManager = new CTLogManager();
        private static CTManager ctManager = new CTManager();
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();
        private static SRPLogManager srpLogManager = new SRPLogManager();
        private static DeviceProScheduleHelper deviceProScheduleHelper = new DeviceProScheduleHelper();
        private const int ctStart = 1;//节拍开始
        private const int ctEnd = 2;  //节拍结束

        static CTHelper()
        {
           
        }
        /// <summary>
        /// 结算CT时间
        /// </summary>
        /// <param name="deviceTagValueInfo">设备标签对象</param>
        /// <param name="tag_value">标签值</param>
        /// <param name="insert_time">插入时间</param>
        public static float CalCTValue(DeviceTagValueInfo deviceTagValueInfo, int tag_value,
                                       DateTime insert_time,
                                       TagAreaAttributeEnum tagAreaAttributeMode= TagAreaAttributeEnum.machine_info)
        {
            int dValue = 0;
            int min_threshold=0, max_threshold = 0;
            station_info station = null;
            MachineInfo machine = null;
            CT_Log ctLog;
            string work_order = null;
            string part_num = null;
            Pro_schedule_machine curDeviceSchedule = null;//当前站工单
            CT ct;
            TimeSpan tSpan;
            bool IsZeroRecore = false;//0值是否需要记录，绿源项目配置为不记录，需要根据行业需要来扩展

            try
            {
                if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
                {
                    machine = machineInfoManager.SelectSingle(deviceTagValueInfo.device_id, null);
                }
                else if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
                {
                    station = DataWorkerCfg.StationsList.FirstOrDefault(x => x.station_id == deviceTagValueInfo.device_id);
                    min_threshold =(int) station.min_threshold;
                    max_threshold = (int)station.max_threshold;
                    machine = DataWorkerCfg.MachinesList.FirstOrDefault(x=>x.station_id==deviceTagValueInfo.device_id);
                    curDeviceSchedule = deviceProScheduleHelper.GetDeviceSchedule(deviceTagValueInfo, tagAreaAttributeMode);
                }
                if (machine != null)
                {
                    if (curDeviceSchedule != null)
                    {
                        work_order = curDeviceSchedule.work_order;
                        part_num = curDeviceSchedule.part_num;
                    }
                    else
                    {
                        srpLogManager.Insert("curDeviceSchedule workorder null=" + deviceTagValueInfo.device_code +"---"+ deviceTagValueInfo.system_tag_code + "---" + tag_value+"--"+ insert_time);
                    }
                    //以下计算节拍时间代码依据行业进行定制与扩展，目前是为金属管材焊接行业
                    if (deviceTagValueInfo.system_tag_code == SystemTagCodeEnum.cycle_time.ToString())//节拍时间
                    {
                        //当前值为1，统计两个1之间的节拍时间，还需要检查前面是否有值为2的记录，有2则此节拍忽略。
                        //统计节拍时间的时候，需要同步计算机器焊接时间，以及人员等待时间
                        if (tag_value == ctStart)//
                        {
                            ctLog = ctLogManager.SelectLatestLog(deviceTagValueInfo.device_code, deviceTagValueInfo.system_tag_code);
                            if (ctLog == null || (ctLog !=null && ctLog.value != ctEnd))//节拍开始信号之前没有节拍结束信号，则统计间隔
                            {
                                CalCircleTime(deviceTagValueInfo, insert_time, min_threshold, max_threshold, tagAreaAttributeMode);
                            }
                        }
                        else if(tag_value == ctEnd)//当前值为2，表示机器停止，为最后一个节拍，需要结束当前节拍。
                        {
                            CalCircleTime(deviceTagValueInfo, insert_time, min_threshold, max_threshold, tagAreaAttributeMode);
                        }
                    }
                    else if(deviceTagValueInfo.system_tag_code == SystemTagCodeEnum.machine_time.ToString())//机器人焊接信号
                    {
                        if (tag_value == 0)//以下降沿作为结束
                        {
                            //向前查找上一条记录是否为2停机
                            ctLog = ctLogManager.SelectLatestLog(deviceTagValueInfo.device_code, SystemTagCodeEnum.cycle_time.ToString());
                            if(ctLog==null || (ctLog != null && ctLog.value != ctEnd))//机器之前未停机
                            {
                                //以ct的开始为起始信号，中间会有转盘信号
                                ctLog = ctLogManager.SelectLatestLog(deviceTagValueInfo.device_code, SystemTagCodeEnum.cycle_time.ToString(), 1);
                                if (ctLog != null)
                                {
                                    tSpan = (TimeSpan)(insert_time - ctLog.insert_time);
                                    dValue = (int)tSpan.TotalSeconds;
                                    if ((min_threshold <= 0 && max_threshold <= 0) ||          //小于0则不限制
                                        (dValue >= min_threshold && dValue <= max_threshold))//时间在限定的范围之内
                                    {
                                        //插入记录
                                        ct = InsertCT(deviceTagValueInfo.device_code, deviceTagValueInfo.device_id, deviceTagValueInfo.system_tag_code,
                                                      ctLog.pn, ctLog.wo, ctLog.insert_time, insert_time, dValue);
                                    }
                                }
                            }
                        }
                    }
                    if (IsZeroRecore || deviceTagValueInfo.system_tag_code != SystemTagCodeEnum.cycle_time.ToString())//需要记录0值，则全记录
                    {
                        //插入日志记录//焊接时间
                        if (string.IsNullOrEmpty(work_order))//工单号码为null,此情况不在ct表做记录
                        {
                            srpLogManager.Insert("wo null=" + deviceTagValueInfo.device_code + "---" + deviceTagValueInfo.system_tag_code + "---" + tag_value+"--"+insert_time);
                        }
                        else
                        {
                            InsertCTLog(deviceTagValueInfo, insert_time, part_num, work_order, tag_value, tagAreaAttributeMode);
                        }
                    }
                    else
                    {
                        if(tag_value > 0)//节拍时间circle_time=0时不记录
                        {
                            if (string.IsNullOrEmpty(work_order))//工单号码为null,此情况不在ct表做记录
                            {
                                srpLogManager.Insert("wo null=" + deviceTagValueInfo.device_code + "---" + deviceTagValueInfo.system_tag_code + "---" + tag_value + "--" + insert_time);
                            }
                            else
                            {
                                InsertCTLog(deviceTagValueInfo, insert_time, part_num, work_order, tag_value, tagAreaAttributeMode);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                srpLogManager.Insert("CalCTValue error=" + ex.Message+" "+ deviceTagValueInfo.device_code +" "+ insert_time+" "+ tag_value);
            }
            return dValue;
        }
        /// <summary>
        /// 计算节拍时间
        /// </summary>
        /// <param name="deviceTagValueInfo"></param>
        /// <param name="insert_time"></param>
        /// <param name="min_threshold"></param>
        /// <param name="max_threshold"></param>
        /// <param name="tagAreaAttributeMode"></param>
        /// <returns></returns>
        private static void CalCircleTime(DeviceTagValueInfo deviceTagValueInfo,DateTime insert_time,
                                          int min_threshold,int max_threshold,
                                          TagAreaAttributeEnum tagAreaAttributeMode = TagAreaAttributeEnum.machine_info)
        {
            int dValue = 0;
            CT_Log ctLog, ctLog_end;
            CT ct;
            TimeSpan tSpan;

            ctLog = ctLogManager.SelectLatestLog(deviceTagValueInfo.device_code, deviceTagValueInfo.system_tag_code, ctStart);
            if (ctLog != null)//向前查找上一条记录
            {
                tSpan = (TimeSpan)(insert_time - ctLog.insert_time);
                dValue = (int)tSpan.TotalSeconds;
                if ((min_threshold <= 0 && max_threshold <= 0) ||          //小于0则不限制
                    (dValue >= min_threshold && dValue <= max_threshold))  //时间在限定的范围之内
                {
                    //此时记录的工单按照之前插入日志的时候决定
                    ct = InsertCT(deviceTagValueInfo.device_code, deviceTagValueInfo.device_id, deviceTagValueInfo.system_tag_code,
                                  ctLog.pn, ctLog.wo, ctLog.insert_time, insert_time, dValue);
                    if (ct != null)
                    {
                        CTManager.Raise_CtValueChangedEvent(ct);//计数信号事件抛送
                    }
                    //查找最近一条的记录
                    ctLog_end = ctLogManager.SelectLatestLog(deviceTagValueInfo.device_code, SystemTagCodeEnum.machine_time.ToString(), 0, ctLog.insert_time);
                    if (ctLog_end == null)//未找到机器焊接结束信号
                    {
                        //机器焊接时间为整个节拍时间
                        InsertCT(deviceTagValueInfo.device_code, deviceTagValueInfo.device_id, SystemTagCodeEnum.machine_time.ToString(),
                                 ctLog.pn, ctLog.wo, ctLog.insert_time, insert_time, dValue);
                        //人工时间为0
                        dValue = 0;
                        InsertCT(deviceTagValueInfo.device_code, deviceTagValueInfo.device_id, SystemTagCodeEnum.staff_time.ToString(),
                                    ctLog.pn, ctLog.wo, insert_time, insert_time, dValue);
                    }
                    else if (ctLog_end.insert_time < insert_time)//机器焊接记录正常结束
                    {
                        tSpan = (TimeSpan)(insert_time - ctLog_end.insert_time);
                        dValue = (int)tSpan.TotalSeconds;
                        //插入人工时间
                        InsertCT(deviceTagValueInfo.device_code, deviceTagValueInfo.device_id, SystemTagCodeEnum.staff_time.ToString(),
                                 ctLog.pn, ctLog.wo, ctLog_end.insert_time, insert_time, dValue);
                    }
                }
            }
        }
        /// <summary>
        /// 插入CT Log
        /// </summary>
        /// <param name="machine_code">设备编号</param>
        /// <param name="system_tag_code">系统标签</param>
        /// <param name="insert_time">插入时间</param>
        /// <param name="pn">机种</param>
        /// <param name="wo">工单</param>
        public static int InsertCTLog(DeviceTagValueInfo deviceTagValueItem, DateTime insert_time,
                                     string pn, string wo, int value,
                                     TagAreaAttributeEnum AreaAttributeEnum)
        {
            try
            {
                CT_Log obj = new CT_Log();
                obj.machine_code = deviceTagValueItem.device_code;
                obj.tag_code = deviceTagValueItem.system_tag_code;
                obj.insert_time = insert_time;
                obj.pn = pn;
                obj.wo = wo;
                obj.station_id = deviceTagValueItem.device_id;
                obj.value = value;
                int res = ctLogManager.Insert(obj);
                return res;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 插入CT数据
        /// </summary>
        /// <param name="device_code">装置编号</param>
        /// <param name="device_id">机台id</param>
        /// <param name="system_tag_code">系统编码</param>
        /// <param name="part_number">机种</param>
        /// <param name="work_order">工单</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="value">差值</param>
        public static CT InsertCT(string device_code,int device_id,string system_tag_code,
                                  string part_number, string work_order,
                                  DateTime start_time, DateTime end_time, 
                                  int value)
        {
            try
            {
                CT obj = new CT();
                obj.machine_code = device_code;
                obj.station_id = device_id;//station_id与machine_code统一，均指同一个，字段命名暂时未改
                obj.pn = part_number;
                obj.wo = work_order;
                obj.start_time = start_time;
                obj.end_time = end_time;
                obj.value = value;
                obj.tag_code = system_tag_code;

                int count = ctManager.Insert(obj);
                if (count > 0)
                {
                    return obj;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
