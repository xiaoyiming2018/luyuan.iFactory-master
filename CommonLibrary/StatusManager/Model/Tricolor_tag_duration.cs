using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.tricolor_tag_duration")]
    public class Tricolor_tag_duration
    {
        ///状态持续时间表
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }

        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { get; set; }

        /// <summary>
        /// 系统标签
        /// </summary>
        public string tag_code { get; set; }

        /// <summary>
        /// 持续时间(秒)
        /// </summary>
        public int duration_second { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { get; set; }
        /// <summary>
        /// 状态是否是有变更过
        /// </summary>
        public bool whether { get; set; }
        /// <summary>
        /// 标签当前的状态。=true标识还在，=false标识当前标签的状态已离开
        /// </summary>
        public bool tag_status { get; set; }
        /// <summary>
        /// 状态消失时间
        /// </summary>
        public DateTime away_time { get; set; }
    }
    /// <summary>
    /// 标签持续时间数据
    /// </summary>
    public class TagDurationData
    {
        /// <summary>
        /// tag标签编码
        /// </summary>
        public string tag_code { get; set; }

        public double value { get; set; }
    }
}
