using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.station_info")]
    public class station_info
    { 
        [Key]
        public int station_id { get; set; } 
        /// <summary>
        /// 站别名称
        /// </summary>
        public string station_name_en { get; set; }
        /// <summary>
        /// 站别名称
        /// </summary>
        public string station_name_cn { get; set; }
        /// <summary>
        /// 站别名称
        /// </summary>
        public string station_name_tw { get; set; } 
        public string type_no { get; set; }
        public string status_no { get; set; }
        /// <summary>
        /// 站位所属制程
        /// </summary>
        public string unit_no { get; set; }
        /// <summary>
        /// 站位所属线别，可为空，不使用的时候为-1
        /// </summary>
        public int line_id { get; set; }
        /// <summary>
        /// 最小阈值
        /// </summary>
        public double min_threshold { get; set; }
        /// <summary>
        /// 最大阈值
        /// </summary>
        public double max_threshold { get; set; }
        /// <summary>
        /// 转换系数
        /// </summary>
        public double convert_multiplier { get; set; }
        /// <summary>
        /// 是否允许工单停机
        /// </summary>
        public bool enable_workorder_stop { get; set; }
    }
}
