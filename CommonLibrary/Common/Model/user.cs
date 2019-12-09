using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 用户权限表
    /// </summary>
    [Table("fimp.user")]
    public class user
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 账户名称
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 用户密码
        /// </summary>
        public string pass_word { get; set; }
        /// <summary>
        /// 用户权限
        /// </summary>
        public int user_level { get; set; }
        /// <summary>
        /// 用户部门
        /// </summary>
        public int dept_id { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_time { get; set; }
    }
    /// <summary>
    /// 带有部门信息的用户，用于页面显示
    /// </summary>
    [Table("fimp.user_dept_view")]
    public class UserDept: user
    {
        /// <summary>
        /// 部门代码cn
        /// </summary>
        public string dept_name_cn { get; set; }
    }
}
