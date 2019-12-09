using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常人员配置
    /// </summary>
    [Table("andon.error_config_person")]
    public class error_config_person
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
        /// 人员级别
        /// </summary>
        public int person_level { set; get; }
        /// <summary>
        /// 人员id
        /// </summary>
        public int person_id { set; get; }
    }
    /// <summary>
    /// 人员配置所对应的视图
    /// </summary>
    [Table("andon.error_config_person_view")]
    public class ErrorCfgPersonView: error_config_person
    {
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name { set; get; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string mobile_phone { set; get; }
        /// <summary>
        /// 部门
        /// </summary>
        public string dept_name_cn { set; get; }
        /// <summary>
        /// 班次中文
        /// </summary>
        public string class_name_cn { set; get; }
    }
}
