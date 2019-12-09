using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Linq;

namespace Advantech.IFactory.CommonHelper
{
    public class DeviceStatusHelper
    {
        private MongoDBHelper mongoDBHelper;
        private List<scada_DeviceStatus> StatusList = new List<scada_DeviceStatus>();

        public DeviceStatusHelper(MongoDBHelper MongoDBHelper)
        {
            this.mongoDBHelper = MongoDBHelper;
        }
        
        /// <summary>
        /// 获取节点的状态
        /// </summary>
        public void GetStatusList()
        {
            var filterBuilder = Builders<scada_DeviceStatus>.Filter;
            var filter = filterBuilder.Eq(x => x.modified, false);
            StatusList = mongoDBHelper.GetList<scada_DeviceStatus>("scada_DeviceStatus", filter);
        }
        /// <summary>
        /// 检查tag所在的节点是否掉线
        /// </summary>
        /// <param name="scada_id">scada_id编码</param>
        /// <returns>false为断线，true为在线</returns>
        public bool CheckDeviceStatus(string scada_id)
        {
            scada_DeviceStatus statusItem = StatusList.FirstOrDefault(x => x.ID.ToString().Trim() == scada_id);
            if (statusItem != null && statusItem.status == false)//状态显示未在线
            {
                return false;
            }
            return true;
        }
    }
    /// <summary>
    /// MongoDB里面scada的状态列表
    /// </summary>
    public class scada_DeviceStatus
    {
        /// <summary>
        /// id为scadaid编号，或者scadaid\deviceid的组合形式
        /// </summary>
        [BsonElement("_id")]
        public string ID { get; set; }
        /// <summary>
        /// 当前更新的时间
        /// </summary>
        [BsonElement("ts")]
        public DateTime ts { get; set; }

        [BsonElement("modified")]
        public bool modified { get; set; }
        /// <summary>
        /// 当前的状态，连接在线为true,断线则为false
        /// </summary>
        [BsonElement("status")]
        public bool status { get; set; }
    }
}
