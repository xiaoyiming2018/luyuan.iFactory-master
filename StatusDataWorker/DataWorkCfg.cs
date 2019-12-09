using Advantech.IFactory.CommonHelper.ScadaAPI;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class DataWorkCfg
    {
        /// <summary>
        /// 每天计算开始时间
        /// </summary>
        public static string DailyStartTime="05:00";
        /// <summary>
        /// 当前所有站位的队列，站位是配置完成后不变的信息
        /// </summary>
        public static List<station_info> StationsList;
        /// <summary>
        /// 线别列表
        /// </summary>
        public static List<line_info> LinesList;
        /// <summary>
        /// 装置当前状态表(缓存)
        /// </summary>
        public static StatusCollection statusCollection =new StatusCollection();

        static DataWorkCfg()
        {
            
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public static void Initial()
        {
            StationManager stationManager = new StationManager();
            StationsList = StationsList = stationManager.SelectAll();
            LineInfoManager lineInfoManager = new LineInfoManager();
            LinesList = lineInfoManager.SelectAll();

            SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
            SystemConfigs cfg = sysConfigsManager.Get("daily_start_time");
            if (cfg != null)
            {
                DailyStartTime = cfg.config_value;
            }

            cfg = sysConfigsManager.Get("Enable_ScadaApi");
            if (cfg != null)
            {
                ScadaAPIConfig.EnableScadaApi = cfg.GetBoolValue();
            }
        }
    }
}
