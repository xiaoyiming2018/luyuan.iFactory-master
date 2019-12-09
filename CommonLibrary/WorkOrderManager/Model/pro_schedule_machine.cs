using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 设备在制进度信息
    /// </summary>
    [Table("fimp.Pro_schedule_machine")]
    public class Pro_schedule_machine
    {
        public Pro_schedule_machine()
        {
            work_order = "";
            part_num = "";
            actual_num = 0;
            standard_num = 0;
            defectives_count = 0;
        }
        [Key]
        public int id { get; set; }
        public string work_order { get; set; }
        public string part_num { get; set; }
        public int station_id { get; set; }
        public string machine_code { get; set; }
        public string unit_no { get; set; }
        public double standard_num { get; set; }
        /// <summary>
        /// 转换后的实际数量
        /// </summary>
        public double actual_num { get; set; }
        /// <summary>
        /// 原始数量
        /// </summary>
        public double raw_actual_num { get; set; }
        public DateTime start_time { get; set; }
        public DateTime last_time { get; set; }
        /// <summary>
        /// 工单状态
        /// </summary>
        public int order_status { get; set; }
        /// <summary>
        /// 不良品数量
        /// </summary>
        public double defectives_count { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime insert_time { get; set; }
        /// <summary>
        /// 当站工单结束时间
        /// </summary>
        public DateTime end_time { get; set; }
        /// <summary>
        /// 工位人员id
        /// </summary>
        public int person_id { get; set; }
    }
}
