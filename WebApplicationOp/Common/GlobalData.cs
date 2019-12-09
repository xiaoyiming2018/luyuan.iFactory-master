using Advantech.IFactory.CommonHelper.ScadaAPI;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iFactory.Op.Common
{
    public class GlobalCfgData
    {
        /// <summary>
        /// 当前未完成的本地缓存工单
        /// </summary>
        public static List<pro_schedule> UnFinishedSchedules = new List<pro_schedule>();
        /// <summary>
        /// 当前未完成的工位工单
        /// </summary>
        public static List<Pro_schedule_machine> UnFinishedDevSchedules = new List<Pro_schedule_machine>();
        /// <summary>
        /// 本地节拍信息
        /// </summary>
        public static List<CtAveraged> CTInfomation = new List<CtAveraged>();
        private static StationManager stationManager = new StationManager();
        private static AreaInfoManager areaManager = new AreaInfoManager();
        /// <summary>
        /// 根据设置所加载的所有站位列表信息
        /// </summary>
        public static List<station_info> StationsList = new List<station_info>();
        private static List<area_info> AreaList;
        /// <summary>
        /// 系统时区
        /// </summary>
        public static int SysTimeZone = 8;
        /// <summary>
        /// 0：非班长模式 1：班长模式
        /// </summary>
        public static int OperateMode = 0;
        /// <summary>
        /// 系统流程模式
        /// </summary>
        public static int SysFlowConfig = (int)FlowModeEnum.Continuous;
        /// <summary>
        /// 物料呼叫类别；=0为只有呼叫，没有编码类型，=1为有具体编码类型
        /// </summary>
        public static int MaterialType = 0;
        /// <summary>
        /// 安灯系统web页面进行呼叫.=true为是，=false为否
        /// </summary>
        public static bool AndonWebCall = false;


        static GlobalCfgData()
        {
            SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
            SystemConfigs cfg = sysConfigsManager.Get("flow_mode");
            if(cfg != null)
            {
                SysFlowConfig = cfg.GetIntValue();
            }
            
            if (cfg != null)
            {
                MaterialType = cfg.GetIntValue();
            }
            
            cfg = sysConfigsManager.Get("Enable_ScadaApi");
            if (cfg != null)
            {
                ScadaAPIConfig.EnableScadaApi = cfg.GetBoolValue();
            }

            cfg = sysConfigsManager.Get("andon_web_call");
            if (cfg != null)
            {
                AndonWebCall = cfg.GetBoolValue();
            }
        }
        /// <summary>
        /// 初始化加载相关配置数据
        /// </summary>
        /// <param name="LineId"></param>
        public static void LoadInitialData(int LineId)
        {
            if(StationsList.Count==0 || StationsList==null)
            {
                StationsList = stationManager.SelectAll(LineId);
            }
            else if(StationsList.Count > 0 && StationsList[0].line_id != LineId)
            {
                StationsList = stationManager.SelectAll(LineId);
            }
            if(AreaList==null || AreaList.Count==0)
            {
                AreaList = areaManager.SelectAll();
                if(AreaList.Count>0)
                {
                    SysTimeZone = AreaList[0].time_zone;
                }
            }
        }
    }
}
