using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonHelper.ScadaAPI;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.Andon
{
    public class AndonGlobalCfg
    {
        /// <summary>
        /// 物料呼叫类别；=0为只有呼叫，没有编码类型，=1为有具体编码类型
        /// </summary>
        public static int MaterialType = 0;
        /// <summary>
        /// 当前的异常队列
        /// </summary>
        public static List<ErrorObject> ErrorWatchList = new List<ErrorObject>();
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

        static AndonGlobalCfg()
        {
        }

        public static void Initial()
        {
            StationManager stationManager = new StationManager();
            StationsList = StationsList = stationManager.SelectAll();
            MachineInfoManager machineInfoManager = new MachineInfoManager();
            MachinesList = machineInfoManager.SelectAll(null);
            LineInfoManager lineInfoManager = new LineInfoManager();
            LinesList = lineInfoManager.SelectAll();

            SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
            SystemConfigs cfg = sysConfigsManager.Get("andon_material_type");
            if (cfg != null)
            {
                MaterialType = cfg.GetIntValue();
            }
            
            cfg = sysConfigsManager.Get("Enable_ScadaApi");
            if (cfg != null)
            {
                ScadaAPIConfig.EnableScadaApi = cfg.GetBoolValue();
            }

            cfg = sysConfigsManager.Get("ServerIp");
            if (cfg != null)
            {
               // MqttClientConfig.ServerIp = cfg.config_value;
            }
            cfg = sysConfigsManager.Get("ServerUser");
            if (cfg != null)
            {
               // MqttClientConfig.ServerUser = cfg.config_value;
            }
            cfg = sysConfigsManager.Get("ServerPassword");
            if (cfg != null)
            {
                //MqttClientConfig.ServerPassword = cfg.config_value;
            }
            cfg = sysConfigsManager.Get("ServerPort");
            if (cfg != null)
            {
                //MqttClientConfig.ServerPort = cfg.GetIntValue();
            }
            if (ScadaAPIConfig.EnableScadaApi == false)
            {
                //MqttManager.ConnectToMqtt();
            }
        }
    }
}
