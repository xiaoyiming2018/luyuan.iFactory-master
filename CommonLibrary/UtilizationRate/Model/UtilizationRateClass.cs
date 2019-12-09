using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("oee.utilization_rate_class")]
    public class UtilizationRateClass
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public int year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public int month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public int day { get; set; }
        /// <summary>
        /// 年-月-日
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 班别代码
        /// </summary>
        public string class_no { get; set; }

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
    }
}
