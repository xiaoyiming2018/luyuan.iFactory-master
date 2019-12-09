using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 错误类别，对应一级类别
    /// </summary>
    [Table("andon.error_type")]
    public class error_type
    {
        [Key]
        public int id { set; get; }
        public string type_code { set; get; }
        public string type_name { set; get; }
        public string remark { set; get; }
        public DateTime createtime { set; get; }
    }
    /// <summary>
    /// 错误明细，对应详细描述信息
    /// </summary>
    [Table("andon.error_type_details")]
    public class error_type_details
    {
        [Key]
        public int id { set; get; }
        public int type_id { set; get; }
        public string code_no { set; get; }
        public string code_name_en { set; get; }
        public string code_name_cn { set; get; }
        public string code_name_tw { set; get; }
        public string remark { set; get; }
        public DateTime createtime { set; get; }
    }
}
