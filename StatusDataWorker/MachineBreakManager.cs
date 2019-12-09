using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    /// <summary>
    /// 设备休息状态检查
    /// </summary>
    public class MachineBreakManager
    {
        private static TricolorTagLogManager triColorTagLogManager = new TricolorTagLogManager();
        private static RestTimeManager restTimeManager = new RestTimeManager();
        private static RestTimeRowManager restTimeRowManager = new RestTimeRowManager();

        /// <summary>
        /// 设备休息状态检查
        /// </summary>
        /// <param name="MachineObj">设备对象。因休息时间目前在设备设置，站位没有休息时间</param>
        /// <param name="device_code">装置编码，可以为设备或者站位</param>
        /// <param name="insert_time">当前经过转换的时间点</param>
        /// <param name="NewTagCode">新进入的状态</param>
        /// <returns>转换过后的代码</returns>
        public static string MachineBreakCheck(MachineInfo MachineObj,string device_code, DateTime insert_time, string NewTagCode = "")
        {
            string tag_code = "";
            string time = insert_time.ToString("HH:mm:ss");
            bool result = restTimeManager.SelectCount(time, MachineObj.unit_no);//查找当前时间是否在休息时间
            if (result)
            {
                //如果有设置休息优先，则替换为休息时间的代码，如果不是就不替换
                if (MachineObj.set_up == (int)DeviceSetUpEnum.BreakPriority) //设置为休息优先
                {
                    tag_code = SystemTagCodeEnum.machine_break.ToString().ToLower();
                }
                else if (MachineObj.set_up == (int)DeviceSetUpEnum.RunPriority)//设置运行优先
                {
                    NewTagCode=triColorTagLogManager.GetLatestTagCode(device_code);//查找上一个状态
                    if (NewTagCode == SystemTagCodeEnum.machine_running.ToString())///
                    {
                        tag_code = SystemTagCodeEnum.machine_running.ToString();//设置为运行状态
                    }
                    else if (NewTagCode == SystemTagCodeEnum.machine_breakdown.ToString())///
                    {
                        tag_code = SystemTagCodeEnum.machine_breakdown.ToString();//设置为故障状态
                    }
                    else
                    {
                        tag_code = SystemTagCodeEnum.machine_break.ToString();//设置为休息状态
                    }
                }
            }
            else
            {
                time = insert_time.ToString("yyyy-MM-dd HH:mm:ss");//当前发生的时刻点，是否为设备排休
                rest_time_row objRestTimeRow = restTimeRowManager.SelectSingle(MachineObj.machine_code, time);

                if (objRestTimeRow != null && !string.IsNullOrEmpty(objRestTimeRow.tag_code))//设备有排休时间，按照设备排休进行转换
                {
                    tag_code = objRestTimeRow.tag_code;
                }
                else//如果不是休息时间读取的还是休息时间的状态，那就读取log日志中的最新一笔数据
                {
                    tag_code = triColorTagLogManager.GetLatestTagCode(device_code);
                }
            }

            return tag_code;
        }

    }
}
