using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("oee.utilization_rate_formula")]
    public class UtilizationRateFormula
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 运行时间公式
        /// </summary>
        public string run_time_formula { get; set; }
        /// <summary>
        /// 异常时间公式
        /// </summary>
        public string error_time_formula { get; set; }
        /// <summary>
        /// 其余时间公式
        /// </summary>
        public string others_time_formula { get; set; }

        ///// <summary>
        ///// 等待(空闲)时间公式
        ///// </summary>
        //public string wait_time_formula { get; set; }
        ///// <summary>
        ///// 换线时间公式
        ///// </summary>
        //public string Linechange_time_formula { get; set; }
        /// <summary>
        /// 开机时间公式
        /// </summary>
        public string boot_time_formula { get; set; }

    }
}
