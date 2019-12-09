using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常机种信息
    /// </summary>
    [Table("andon.error_config_pn")]
    public class error_config_pn
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 错误配置的id
        /// </summary>
        public int error_config_id { set; get; }
        /// <summary>
        /// 班次
        /// </summary>
        public string class_no { set; get; }
        /// <summary>
        /// 机种编号
        /// </summary>
        public string part_num { set; get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string description { set; get; }
    }
}
