using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.tricolor_tag_log")]
    public class Tricolor_tag_log
    {
        ///Log日志表
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
        /// 插入时间
        /// </summary>
        public DateTime insert_time { get; set; }
    }
}
