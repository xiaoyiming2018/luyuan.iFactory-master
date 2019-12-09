using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Advantech.IFactory.WorkOrderManage
{
    /// <summary>
    /// 工件计数
    /// </summary>
    public class WorkPieceCount
    {
        private static ProScheduleManager proScheduleManager = new ProScheduleManager();
        private static ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        private static ProSchedulemachineLogManager proSchedulemachineLogManager = new ProSchedulemachineLogManager();
        private static ProductivityDailyManager productivityDailyManager = new ProductivityDailyManager();
        private static MachineInfoManager machineInfoManager = new MachineInfoManager();
        private static TagAreaAttributeEnum tagAreaAttributeMode = TagAreaAttributeEnum.station_info;//最小站位模式
        private static SRPLogManager srpLogManager = new SRPLogManager();

        /// <summary>
        /// cicle_time值变化更新
        /// </summary>
        /// <param name="ct"></param>
        public static void CtNumChangedEvent(CT ct)
        {
            Pro_schedule_machine deviceSchedule = new Pro_schedule_machine();
            pro_schedule_machine_log deviceScheduleLog = new pro_schedule_machine_log();
            MachineInfo machine = null;
            station_info station = null;
            DeviceTagValueInfo deviceTagValueInfo = new DeviceTagValueInfo();
            int count = 0;
            int deviceId, sIndex = 0;
            double mutiplier = 1;
            List<station_info> stations = new List<station_info>();
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息

            try
            {
                if (tagAreaAttributeMode == TagAreaAttributeEnum.machine_info)
                {
                    machine = machineInfoManager.SelectSingle(-1, ct.machine_code);
                }
                else if (tagAreaAttributeMode == TagAreaAttributeEnum.station_info)
                {
                    station = DataWorkerCfg.StationsList.FirstOrDefault(x => x.station_id == ct.station_id);
                    stations = DataWorkerCfg.StationsList.Where(x => x.line_id == station.line_id).OrderBy(y => y.station_id).ToList();
                    mutiplier = station.convert_multiplier;//转换系数
                    if (mutiplier == 0)
                    {
                        mutiplier = 1;
                    }
                    sIndex = stations.IndexOf(station);
                }
                if (machine != null || station != null)
                {
                    string unit_no = string.Empty;
                    if (machine != null)
                    {
                        deviceId = machine.machine_id;
                        unit_no = machine.unit_no;
                        deviceTagValueInfo.device_code = ct.machine_code;
                    }
                    else
                    {
                        deviceId = station.station_id;
                        unit_no = station.unit_no;
                        deviceTagValueInfo.device_code = station.station_name_en;
                    }

                    //更新工单完成数量
                    deviceSchedule = proScheduleMachineManager.SelectSingle(ct.wo, unit_no, ct.machine_code, (int)OrderStatusEnum.Excuting);

                    if (deviceSchedule != null)
                    {
                        dic = new Dictionary<string, object>();
                        deviceSchedule.raw_actual_num += 1;//原始数量累计
                        deviceSchedule.actual_num = deviceSchedule.raw_actual_num * mutiplier;//转换实际数量
                        //Console.WriteLine(station.station_name_en +"  " + station.convert_multiplier +"  "+ proScheduleMachineinfo.raw_actual_num  + "  " + proScheduleMachineinfo.actual_num);
                        dic.Add("raw_actual_num", deviceSchedule.raw_actual_num);
                        dic.Add("actual_num", deviceSchedule.actual_num);
                        deviceSchedule.last_time = ct.end_time;
                        dic.Add("last_time", deviceSchedule.last_time);
                        count = proScheduleMachineManager.Update(deviceSchedule, dic);
                        if (count > 0)//插入日志
                        {
                            if(deviceSchedule.actual_num>=deviceSchedule.standard_num)//数量到达，下发停机命令
                            {
                                DeviceOrdersCheck.WriteCommandToDevice(deviceSchedule.station_id, (int)RobotCommandEnum.Stop);
                            }
                            deviceScheduleLog.actual_num = deviceSchedule.actual_num;
                            deviceScheduleLog.machine_code = deviceSchedule.machine_code;
                            deviceScheduleLog.part_num = deviceSchedule.part_num;
                            deviceScheduleLog.standard_num = deviceSchedule.standard_num;
                            deviceScheduleLog.station_id = deviceSchedule.station_id;
                            deviceScheduleLog.unit_no = deviceSchedule.unit_no;
                            deviceScheduleLog.work_order = deviceSchedule.work_order;
                            deviceScheduleLog.ts = ct.end_time;
                            proSchedulemachineLogManager.Insert(deviceScheduleLog);
                        }

                        //更新达成率,生产力信息,以最后一站完成数量计算
                        if (deviceSchedule.work_order != string.Empty && sIndex == (stations.Count - 1))
                        {
                            CalScheduleProductivity(deviceSchedule);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                srpLogManager.Insert("CtNumChangedEvent error=" + ex.Message);
            }
        }
        /// <summary>
        /// 计算工单的生产力信息
        /// </summary>
        /// <param name="deviceSchedule"></param>
        private static void CalScheduleProductivity(Pro_schedule_machine deviceSchedule)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();//加入需要更新的字段信息
            pro_schedule schedule = proScheduleManager.SelectByWorkOrder(deviceSchedule.work_order);
            if (schedule != null)
            {
                schedule.actual_num = deviceSchedule.actual_num;//工单的实际数量与最后一站相同
                dic = new Dictionary<string, object>();
                dic.Add("actual_num", schedule.actual_num);
                if (schedule.standard_num != 0)
                {
                    TimeSpan TSpan = (TimeSpan)(DateTime.Now.AddHours(GlobalDefine.SysTimeZone) - schedule.start_time);
                    if (schedule.standard_num > 0)
                    {
                        schedule.achievement_rate = schedule.actual_num * 100 / schedule.standard_num;
                        dic.Add("achievement_rate", schedule.achievement_rate);
                    }
                    schedule.standard_usetime = schedule.standard_workinghour * schedule.actual_num;
                    dic.Add("standard_usetime", schedule.standard_usetime);
                    if (schedule.standard_usetime > 0)
                    {
                        schedule.productivity = (int)TSpan.TotalSeconds / schedule.standard_usetime;
                        dic.Add("productivity", schedule.productivity);
                    }
                }
                proScheduleManager.Update(schedule, dic);//工单只是更新指定的字段，否则执行状态容易被其他地方覆盖
            }

        }
    }
}
