using Advantech.IFactory.CommonLibrary;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper.ScadaAPI;
using System.Threading;

namespace Advantech.IFactory.WorkOrderManage
{
    /// <summary>
    /// 工位工单检查
    /// </summary>
    public class DeviceOrdersCheck
    {
        private static ProScheduleManager proScheduleManager = new ProScheduleManager();
        private static ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        private static TagService tagService = new TagService();
        private static StationManager stationManager = new StationManager();
        private static TagAreaAttributeEnum tagAttribute = TagAreaAttributeEnum.station_info;
        private static Dictionary<int, int> StationValueDic = new Dictionary<int, int>();//工位允许位值记录.工位id与值

        static DeviceOrdersCheck()
        {
            foreach (var station in DataWorkerCfg.StationsList)//查询所有工位
            {
                if (StationValueDic.ContainsKey(station.station_id) == false)
                {
                    StationValueDic.Add(station.station_id, (int)RobotCommandEnum.Run);//分配初始值
                }
            }
        }
        /// <summary>
        /// 检查工位是否有未完成的工单，无工单则停机
        /// </summary>
        public static void CheckOrders()
        {
            Dictionary<station_info, int> StationsDic = new Dictionary<station_info, int>();

            foreach (var station in DataWorkerCfg.StationsList)//查询所有工位
            {
                station_info stationItem = stationManager.SelectSingle(station.station_id);
                if(stationItem.enable_workorder_stop==false)//未启用
                {
                    WriteCommandToDevice(stationItem.station_id, (int)RobotCommandEnum.Run);//下发正常命令
                    continue;
                }
                if (StationsDic.ContainsKey(station) == false)
                {
                    StationsDic.Add(station, (int)RobotCommandEnum.Stop);//1是停止命令，0是正常；默认是停止命令
                }

                Pro_schedule_machine schedule=  proScheduleMachineManager.SelectOnLineWorkorder(station.station_name_en);
                if(schedule!=null)
                {
                    if(schedule.actual_num< schedule.standard_num)
                    {
                        StationsDic[station] = (int)RobotCommandEnum.Run;//有工单，且小于标准数量，则算正常，值为0
                    }
                }
            }
            
            foreach(var item in StationsDic)//写入底层.1是停止，0是正常
            {
                WriteCommandToDevice(item.Key.station_id, item.Value);
            }
        }
        /// <summary>
        /// 写入命令到设备层
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool WriteCommandToDevice(int DeviceId,int value)
        {
            bool bit=false;
            if(StationValueDic.ContainsKey(DeviceId))//比较值是否发生变化
            {
                if(StationValueDic[DeviceId]==value)
                {
                    return false;//无值变化直接返回
                }
                else
                {
                    StationValueDic[DeviceId] = value;//更新当前值，并写入
                }
            }
            List<StationTagInfo> tags = tagService.GetWaTagAndTypeInfo<StationTagInfo>(tagAttribute, DeviceId, SystemTagCodeEnum.robot_stop.ToString());
            if (tags != null && tags.Count > 0)
            {
                if (ScadaAPIConfig.EnableScadaApi)
                {
                    bit = ScadaAPIHelper.WriteValueAsync(tags[0].tag_code, value);
                    if (bit == false)
                    {
                        bit = ScadaAPIHelper.WriteValueAsync(tags[0].tag_code, value);//失败再次写入
                    }
                }
            }
            if(bit==false)
            {
                StationValueDic[DeviceId] = -1;//写入失败，恢复为-1，下一次再尝试写入
                Console.WriteLine("ScadaAPIHelper.WriteValueAsync ==false,DeviceId=" + DeviceId);
            }
            Thread.Sleep(50);//延时退出
            return bit;
        }
    }
}
