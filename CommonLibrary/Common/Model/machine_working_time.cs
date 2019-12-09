using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 设备工时对象
    /// </summary>
    [Table("fimp.machine_working_time")]
    public  class machine_working_time
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 设备id
        /// </summary>
        public int machine_id { get; set; }
        /// <summary>
        /// 厂区id
        /// </summary>
        public int plant_id { get; set; }
        /// <summary>
        /// 制程
        /// </summary>
        public string unit_no { get; set; }
        /// <summary>
        /// 线别id
        /// </summary>
        public int line_id { get; set; }
        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { get; set; }
        /// <summary>
        /// 机种列表
        /// </summary>
        public string part_num { get; set; }
        /// <summary>
        /// 标准工时
        /// </summary>
        public decimal standard_time { get; set; }

    }
}
