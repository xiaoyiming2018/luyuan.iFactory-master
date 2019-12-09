using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonLibrary;
using MongoDB.Driver;

namespace Advantech.IFactory.MachineStatusCollect
{
    /// <summary>
    /// 网络状态检查
    /// </summary>
    public class NetworkManager
    {
        private TagInfoManager tagInfoManger = new TagInfoManager();
        private IEnumerable<IGrouping<int, StationWaTag>>deviceList;
        private DeviceStatusHelper deviceStatusHelper;
        private SRPLogManager srpLogManager = new SRPLogManager();

        public NetworkManager(MongoDBHelper MongoDBHelper)
        {
            List<StationWaTag> tags = tagInfoManger.GetStationTagInfos(TagTypeEnum.UtilizationRate.ToString());
            deviceList= tags.GroupBy(x => x.area_attribute_value);
            this.deviceStatusHelper = new DeviceStatusHelper(MongoDBHelper);
        }
        /// <summary>
        /// 检查所有的节点连接
        /// </summary>
        public void CheckAllDevice()
        {
            string scadaNode=String.Empty;

            deviceStatusHelper.GetStatusList();//抓取状态表
            
            foreach (var item in deviceList)
            {
                List<StationWaTag> deviceTags = item.ToList();
                if (deviceTags == null || deviceTags.Count == 0) continue;
                string[] tagstr= deviceTags[0].tag_code.Split('\\');//取出第一个
                if(tagstr.Length>0)
                {
                    scadaNode = deviceTags[0].scada_id + "\\" + tagstr[0] ;
                    if (deviceStatusHelper.CheckDeviceStatus(scadaNode) == false)//状态显示未在线
                    {
                        DeviceTagValueInfo deviceTagValueInfo = new DeviceTagValueInfo();
                        deviceTagValueInfo.device_code = deviceTags[0].station_name_en;
                        deviceTagValueInfo.device_id = item.Key;
                        deviceTagValueInfo.insert_time = DateTime.Now;
                        deviceTagValueInfo.system_tag_code = SystemTagCodeEnum.network_break.ToString();
                        deviceTagValueInfo.tag_value = "1";
                        deviceTagValueInfo.system_type_code = TagTypeEnum.UtilizationRate.ToString();
                        //目前webaccess升级后启用此功能
                        Raise_DeviceNetWorkBreakEvent(deviceTagValueInfo);
                        srpLogManager.Insert("检测到scadaNode="+ scadaNode+"断线");
                    }
                }
            }
        }

        public delegate void DeviceNetWorkBreakDelegate(DeviceTagValueInfo e);
        public event DeviceNetWorkBreakDelegate DeviceNetWorkBreakEvent = delegate { };
        /// <summary>
        /// 触发设备断线事件
        /// </summary>
        /// <param name="e"></param>
        public void Raise_DeviceNetWorkBreakEvent(DeviceTagValueInfo e)
        {
            DeviceNetWorkBreakEvent?.Invoke(e);
        }
    }
}
