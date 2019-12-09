using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 设备异常代码定义
    /// </summary>
    [Table("andon.machine_error_code")]
    public class MachineErrorCode
    {
        public static string TableName = "andon.machine_error_code";
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 设备类型
        /// </summary>
        public int machine_type { set; get; }
        /// <summary>
        /// 异常代码
        /// </summary>
        public string code_no { set; get; }
        /// <summary>
        /// 异常名称（英文）
        /// </summary>
        public string code_name_en { set; get; }
        /// <summary>
        /// 异常名称（中文）
        /// </summary>
        public string code_name_cn { set; get; }
        /// <summary>
        /// 异常名称（繁体）
        /// </summary>
        public string code_name_tw { set; get; }
        /// <summary>
        /// 对应的下位tag值
        /// </summary>
        public int tag_value { set; get; }
        /// <summary>
        /// 是否需要呼叫安灯系统(当系统发生严重错误时，需要以安灯系统流程处理，可设置此标识为true)
        /// </summary>
        public int require_andon { set; get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string desciption { set; get; }
    }
    /// <summary>
    /// 设备异常代码记录
    /// </summary>
    [Table("andon.machine_error_code_log")]
    public class MachineErrorCodeLog
    {
        public static string TableName = "andon.machine_error_code_log";
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }
        /// <summary>
        /// 设备异常代码定义id
        /// </summary>
        public int error_code_id { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { get; set; }
        /// <summary>
        /// 离开时间
        /// </summary>
        public DateTime away_time { get; set; }
        /// <summary>
        /// 标签值
        /// </summary>
        public int value { get; set; }
    }

    /// <summary>
    /// 设备异常记录报警，用于报表显示
    /// </summary>
    public class MachineErrCodeLogForRpt: MachineErrorCodeLog
    {
        /// <summary>
        /// 异常代码
        /// </summary>
        public string code_no { set; get; }
        /// <summary>
        /// 异常名称（英文）
        /// </summary>
        public string code_name_en { set; get; }
        /// <summary>
        /// 异常名称（中文）
        /// </summary>
        public string code_name_cn { set; get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string desciption { set; get; }
    }
}
