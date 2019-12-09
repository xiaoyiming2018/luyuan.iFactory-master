using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.rest_time_row")]
    public class rest_time_row
    {
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// mqtt协议码，只能选择tag类型为休息，稼动率的部分，如果填写代表这段时间内全部转为该状态，如果不填代表这段时间内不接受数据
        /// </summary>
        public string tag_code { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string remarks { get; set; }


    }
}
