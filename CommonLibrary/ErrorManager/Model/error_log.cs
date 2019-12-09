using Advantech.IFactory.CommonLibrary;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常记录
    /// </summary>
    [Table("andon.error_log")]
    public class error_log
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string error_name { set; get; }
        /// <summary>
        /// 异常编码所对应的系统Tag编码
        /// </summary>
        public string system_tag_code { set; get; }
        /// <summary>
        /// 确认人员id
        /// </summary>
        public int ack_person_id { set; get; }
        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { set; get; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { set; get; }
        /// <summary>
        /// 机种代号
        /// </summary>
        public string pn { set; get; }
        /// <summary>
        /// 工单编号
        /// </summary>
        public string work_order { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime start_time { set; get; }
        /// <summary>
        /// 到岗人员id
        /// </summary>
        public int arrival_person_id { set; get; }
        /// <summary>
        /// 到岗时间
        /// </summary>
        public DateTime arrival_time { set; get; }
        /// <summary>
        /// 错误类别id
        /// </summary>
        public int error_type_id { set; get; }
        /// <summary>
        /// 解除时间
        /// </summary>
        public DateTime release_time { set; get; }
        /// <summary>
        /// 明细维护时间
        /// </summary>
        public DateTime maintenance_time { set; get; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { set; get; }
        /// <summary>
        /// 线别id
        /// </summary>
        public int line_id { set; get; }
        /// <summary>
        /// 制程工序
        /// </summary>
        public string unit_no { set; get; }
        /// <summary>
        /// 不良品数量
        /// </summary>
        public double defectives_count { set; get; }
        /// <summary>
        /// 停机时间
        /// </summary>
        public double downtime_min { set; get; }
    }
    /// <summary>
    /// 安灯记录报表
    /// </summary>
    public class ErrorLogRpt: error_log
    {
        /// <summary>
        /// 签到人员名字
        /// </summary>
        public string arrival_person_name { set; get; }
        /// <summary>
        /// 确认人员名字
        /// </summary>
        public string ack_person_name { set; get; }
        /// <summary>
        /// 解除原因名称
        /// </summary>
        public string error_type_name { set; get; }
        /// <summary>
        /// 解除原因类型
        /// </summary>
        public string error_code_name_cn { set; get; }
    }
}
