using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 品质异常记录表
    /// </summary>
    [Table("andon.error_quality_log")]
    public class error_quality_log
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { set; get; }
        /// <summary>
        /// error_log对应id，可为空
        /// </summary>
        public int error_log_id { set; get; }
        /// <summary>
        /// 异常类别id，可以为空
        /// </summary>
        public int error_type_id { set; get; }
        /// <summary>
        /// 站别id
        /// </summary>
        public int station_id { set; get; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { set; get; }
        /// <summary>
        /// 不良品数量
        /// </summary>
        public double defectives_count { set; get; }
        /// <summary>
        /// 发生的问题描述
        /// </summary>
        public string quality_description { set; get; }
        /// <summary>
        /// 发生的问题原因
        /// </summary>
        public string quality_reason { set; get; }
        /// <summary>
        /// 改进计划
        /// </summary>
        public string improve_plan { set; get; }
        /// <summary>
        /// 计划完成日期
        /// </summary>
        public DateTime plan_date { set; get; }
        /// <summary>
        /// 责任人员
        /// </summary>
        public string responsible_person { set; get; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { set; get; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { set; get; }
    }
}
