using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// srp系统内部记录
    /// </summary>
    [Table("fimp.srp_inner_log")]
    public class srp_inner_log
    {
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 编码名称
        /// </summary>
        public string srp_code { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string srp_description { set; get; }
        /// <summary>
        /// 上一次更新时间
        /// </summary>
        public string last_time { set; get; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { set; get; }
    }
}
