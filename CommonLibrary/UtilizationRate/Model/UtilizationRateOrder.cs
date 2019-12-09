using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 工单机种稼动率
    /// </summary>
    [Table("oee.utilization_rate_order")]
    public class UtilizationRateOrder
    {
        /// <summary>
        /// 关联表名称
        /// </summary>
        public static readonly string TableName = "oee.utilization_rate_order";
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// 机种
        /// </summary>
        public string part_number { get; set; }
        /// <summary>
        /// 线别id
        /// </summary>
        public int line_id { get; set; }
        /// <summary>
        /// 运行时间
        /// </summary>
        public double run_time { get; set; }
        /// <summary>
        /// 异常时间
        /// </summary>
        public double error_time { get; set; }
        /// <summary>
        /// 其余时间
        /// </summary>
        public double others_time { get; set; }
        /// <summary>
        /// 休息时间
        /// </summary>
        public double rest_time { get; set; }

        /// <summary>
        /// 开机时间
        /// </summary>
        public double boot_time { get; set; }

        /// <summary>
        /// 稼动率
        /// </summary>
        public double utilization_rate { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime insert_time { get; set; }
    }
}
