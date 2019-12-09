using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常通知人员记录
    /// </summary>
    [Table("andon.error_log_person")]
    public class error_log_person
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { set; get; }
        /// <summary>
        /// error_log id
        /// </summary>
        public int error_log_id { set; get; }
        /// <summary>
        /// person_id 
        /// </summary>
        public int person_id { set; get; }
        /// <summary>
        /// 消息级别 
        /// </summary>
        public int message_level { set; get; }
        /// <summary>
        /// 消息所对应的事件流程，流程为EventHandleFlowEnum枚举名称
        /// </summary>
        public string message_flow { set; get; }
        /// <summary>
        /// 插入时间 
        /// </summary>
        public DateTime insert_time { set; get; }
    }
}
