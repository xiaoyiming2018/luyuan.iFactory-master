using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.pro_schedule_machine_log")]
    public class pro_schedule_machine_log
    {
        ///在制进度日志
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string work_order { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public string part_num { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }

        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { get; set; }

        /// <summary>
        /// 制程代码
        /// </summary>
        public string unit_no { get; set; }

        /// <summary>
        /// 标准数量
        /// </summary>
        public double standard_num { get; set; }

        /// <summary>
        /// 实际数量
        /// </summary>
        public double actual_num { get; set; }

        /// <summary>
        /// 量产试产
        /// </summary>
        public int type { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime ts { get; set; }
    }
}
