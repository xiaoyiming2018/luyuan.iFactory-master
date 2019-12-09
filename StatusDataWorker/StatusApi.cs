using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    //[Route("api/[controller]/[action]")]
    //public class StatusController : Controller
    //{
    //    // 获取当前装置的运行状态
    //    //请求方式 https://localhost:44352/api/Status/GetStatus?device_id=27
    //    [HttpGet]
    //    public string GetStatus(int device_id,string device_code)
    //    {
    //        return DataWorkCfg.statusCollection.GetDuration(device_id, device_code);
    //    }
    //}

    /// <summary>
    /// 云端的MongoDB数据库Tag类
    /// </summary>
    public class DeviceStatus
    {
        [BsonId]
        public ObjectId ID { get; set; }
        /// <summary>
        /// s云端为scada id 信息
        /// </summary>
        [BsonElement("s")]
        public int device_id { get; set; }
        /// <summary>
        /// t在云端为device/tag的组合
        /// </summary>
        [BsonElement("t")]
        public string device_code { get; set; }
        [BsonElement("v")]
        public object tag_code { get; set; }
        [BsonElement("ts")]
        public DateTime ts { get; set; }
    }
}
