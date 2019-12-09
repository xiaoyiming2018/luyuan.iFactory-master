using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.tag_value_log")]
    public class tag_value_log
    {
        [Key]
        public long id { set; get; }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string tag_code { set; get; }
        /// <summary>
        /// srp编码
        /// </summary>
        public string srp_code { set; get; }
        /// <summary>
        /// 对应的系统标签
        /// </summary>
        public string system_tag_code { set; get; }
        /// <summary>
        /// 标签值
        /// </summary>
        public string tag_value { set; get; }
        /// <summary>
        /// 插入的时间，时区转换后
        /// </summary>
        public DateTime insert_time { set; get; }

    }
}
