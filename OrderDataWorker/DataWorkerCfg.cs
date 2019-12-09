using Advantech.IFactory.CommonHelper.ScadaAPI;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.WorkOrderManage
{
    public class DataWorkerCfg
    {
        /// <summary>
        /// 当前所有站位的队列，站位是配置完成后不变的信息
        /// </summary>
        public static List<station_info> StationsList;
        /// <summary>
        /// 当前所有站位的队列，站位是配置完成后不变的信息
        /// </summary>
        public static List<MachineInfo> MachinesList;
        /// <summary>
        /// 线别列表
        /// </summary>
        public static List<line_info> LinesList;
        /// <summary>
        /// 标签列表
        /// </summary>
        public static List<system_tag_code> CircleTimeTagCodeList;
        /// <summary>
        /// 系统时区
        /// </summary>
        public static int time_zone = 8;          //系统默认的时区差

        static DataWorkerCfg()
        {
            StationManager stationManager = new StationManager();
            StationsList = StationsList = stationManager.SelectAll();

            MachineInfoManager machineInfoManager = new MachineInfoManager();
            MachinesList = machineInfoManager.SelectAll(null);

            LineInfoManager lineInfoManager = new LineInfoManager();
            LinesList = lineInfoManager.SelectAll();

            CircleTimeTagCodeList = new List<system_tag_code>();
            AreaInfoManager areaInfoManager = new AreaInfoManager();
            var list = areaInfoManager.SelectAll();
            if (list != null && list.Count > 0)
            {
                time_zone = list[0].time_zone;//获取系统设定的时区
            }

            //加载在制进度类别
            SystemTagCodeManager systemTagCodeManager = new SystemTagCodeManager();
            CircleTimeTagCodeList = systemTagCodeManager.SeclectByTagType((int)TagTypeEnum.CircleTime);

            SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
            SystemConfigs cfg = sysConfigsManager.Get("daily_start_time");
            if (cfg != null)
            {
                //DailyStartTime = cfg.config_value;
            }

            cfg = sysConfigsManager.Get("Enable_ScadaApi");
            if (cfg != null)
            {
                ScadaAPIConfig.EnableScadaApi = cfg.GetBoolValue();
            }
        }
    }
}
