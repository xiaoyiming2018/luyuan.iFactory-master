using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.ct_log")]
    public class CT_Log
    {
        ///C/T 日志表
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }

        /// <summary>
        /// 机种
        /// </summary>
        public string pn { get; set; }

        /// <summary>
        /// 工单
        /// </summary>
        public string wo { get; set; }

        /// <summary>
        /// 系统标签
        /// </summary>
        public string tag_code { get; set; }

        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { get; set; }



        /// <summary>
        /// io
        /// </summary>
        public int value { get; set; }
    }
}
