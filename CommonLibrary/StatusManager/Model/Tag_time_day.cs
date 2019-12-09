using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.tag_time_day")]
    public class Tag_time_day
    {
        ///状态时间表（班次）
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
        public DateTime date { get; set; }

        /// <summary>
        /// 持续时间(分钟)
        /// </summary>
        public double duration { get; set; }
    }
}
