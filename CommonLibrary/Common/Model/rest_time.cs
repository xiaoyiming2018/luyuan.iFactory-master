using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.rest_time")]
    public class rest_time
    {
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 制程
        /// </summary>
        public string unit_no { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string start_time { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string end_time { get; set; }

        /// <summary>
        /// 是否启用A启用S停用
        /// </summary>
        public string state { get; set; }

    }
}
