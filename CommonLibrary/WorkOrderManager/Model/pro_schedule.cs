using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.pro_schedule")]
    public class pro_schedule
    {
        public pro_schedule()
        {
            this.standard_num = 300;
            this.actual_num = 0;
            this.order_index = 1;
            this.defectives_count = 0;
            this.standard_workinghour = 8;
        }
        [Key]
        public int id { get; set; }
        public string work_order { get; set; }
        public string part_num { get; set; }
        public string unit_no { get; set; }
        public double plan_num { get; set; }
        /// <summary>
        /// 标准数量
        /// </summary>
        public double standard_num { get; set; }
        /// <summary>
        /// 实际数量
        /// </summary>
        public double actual_num { get; set; }
        public DateTime start_time { get; set; }
        public DateTime end_time { get; set; }
        public int order_status { get; set; }
        public int order_index { get; set; }
        public double standard_workinghour { get; set; }
        public double standard_usetime { get; set; }
        public double productivity { get; set; }
        public double achievement_rate { get; set; }
        public int line_id { get; set; }
        /// <summary>
        /// 不良品数量
        /// </summary>
        public double defectives_count { get; set; }
        /// <summary>
        /// 数据创建时间
        /// </summary>
        public DateTime insert_time { get; set; }
        /// <summary>
        /// 班次选项
        /// </summary>
        public int class_id { get; set; }
        /// <summary>
        /// 参数选项。=1为自动结束
        /// </summary>
        public int parameter_option { get; set; }
        /// <summary>
        /// 排产时间
        /// </summary>
        public DateTime schedule_time { get; set; }
    }
    /// <summary>
    /// 包含制程和线别的工单对象
    /// </summary>
    public class ProScheduleObject
    {
        /// <summary>
        /// 工单信息
        /// </summary>
        public pro_schedule SchduleObject { get; set; }
        /// <summary>
        /// 线别信息
        /// </summary>
        public line_info LineObject { get; set; }
        /// <summary>
        /// 制程信息
        /// </summary>
        public unit_info UnitObject { get; set; }
    }
}
