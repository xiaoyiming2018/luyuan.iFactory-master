using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.ct")]
    public class CT
    {
        ///C/T，站位的cirecle time
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
        /// 开始时间
        /// </summary>
        public DateTime start_time { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime end_time { get; set; }

        /// <summary>
        /// 差值
        /// </summary>
        public int value { get; set; }

        /// <summary>
        /// 系统标签
        /// </summary>
        public string tag_code { get; set; }
    }
}
