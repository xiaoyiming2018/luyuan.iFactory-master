using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 系统配置参数
    /// </summary>
    [Table("fimp.system_configs")]
    public class SystemConfigs
    {
        /// <summary>
        /// 返回表名称
        /// </summary>
        /// <returns></returns>
        public static string TableName = "fimp.system_configs";
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 配置编码
        /// </summary>
        public string config_code { set; get; }
        /// <summary>
        /// 配置值
        /// </summary>
        public string config_value { set; get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string desciption { set; get; }
        /// <summary>
        /// 返回设定的整数值
        /// </summary>
        /// <returns></returns>
        public int GetIntValue()
        {
            int value = 0;
            int.TryParse(config_value,out value);
            return value;
        }
        /// <summary>
        /// 返回设定的bool值
        /// </summary>
        /// <returns></returns>
        public bool GetBoolValue()
        {
            bool value = false;
            bool.TryParse(config_value, out value);
            return value;
        }
    }
}
