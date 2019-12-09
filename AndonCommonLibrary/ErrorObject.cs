using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.Andon
{
    /// <summary>
    /// 已通知人员对象
    /// </summary>
    public class ErrorLogPerson
    {
        public error_log_person eLogPerson { set; get; }
        public person ePerson { set; get; }
    }
    /// <summary>
    /// 异常配置的人员对象
    /// </summary>
    public class ErrorCfgPerson
    {
        public error_config_person eCfgPerson { set; get; }
        public person ePerson { set; get; }
    }
    
    /// <summary>
    /// 异常记录以及所对应的的配置信息
    /// </summary>
    public class ErrLogAndCfg
    {
        /// <summary>
        /// 数据记录对象
        /// </summary>
        public error_log ELog { set; get; }
        /// <summary>
        /// 配置信息
        /// </summary>
        public error_config eConfig { set; get; }
    }
    /// <summary>
    /// 一个完整的Error对象，包含记录，配置信息
    /// </summary>
    public class ErrorObject: ErrLogAndCfg
    {
        public ErrorObject()
        {
            eCfgPersonList = new List<ErrorCfgPerson>();
            eMsgedPersonList = new List<ErrorLogPerson>();
            eSignedPersons = new List<person>();
        }
        /// <summary>
        /// 异常对象所对应的设备、站位等编号，用于通知
        /// </summary>
        public string DeviceCode { set; get; }
        public int DeviceId { set; get; }

        /// <summary>
        /// 呼叫的物料信息
        /// </summary>
        public material_request_info RequireMat { set; get; }
        /// <summary>
        /// 签到人员记录
        /// </summary>
        public List<person> eSignedPersons { set; get; }
        /// <summary>
        /// 配置的人员列表,在创建时需要读取加载到内存中
        /// </summary>
        public List<ErrorCfgPerson> eCfgPersonList { set; get; }
        /// <summary>
        /// 机种列表
        /// </summary>
        public List<error_config_pn> eCfgPnList { set; get; }
        /// <summary>
        /// 已通知的人员记录，在发送信息时加载到队列中
        /// </summary>
        public List<ErrorLogPerson> eMsgedPersonList { set; get; }
        /// <summary>
        /// 异常解除后的详细描述信息。在解除异常时更新此对象
        /// </summary>
        public error_type_details eTypeDetails { set; get; }
    }

}
