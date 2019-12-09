using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.ct_averaged")]
    public class CtAveraged
    {
        /// <summary>
        /// 关联表名称
        /// </summary>
        public static readonly string TableName = "fimp.ct_averaged";
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 设备id
        /// </summary>
        public int device_id { get; set; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string device_code { get; set; }
        /// <summary>
        /// 机种
        /// </summary>
        public string part_number { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// 平均时间(分钟)
        /// </summary>
        public double averaged_min { get; set; }
        /// <summary>
        /// 系统标签
        /// </summary>
        public string tag_code { get; set; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { get; set; }
        /// <summary>
        /// 最近更新时间
        /// </summary>
        public DateTime last_time { get; set; }
    }
}
