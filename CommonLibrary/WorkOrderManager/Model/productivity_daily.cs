using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.Productivity_daily")]
    public class Productivity_daily
    {
        ///日表
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }

        /// <summary>
        /// 线别
        /// </summary>
        public int line_id { get; set; }

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
        /// 实际数量
        /// </summary>
        public double real_num { get; set; }

        /// <summary>
        /// 投入工时
        /// </summary>
        public double real_workinghour { get; set; }

        /// <summary>
        /// 产出工时
        /// </summary>
        public double product_workinghour { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public string createtime { get; set; }
    }
}
