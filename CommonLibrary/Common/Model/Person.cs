using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.person")]
    public class person
    {
        public static string TableName = "fimp.person";
        [Key]
        public int id { set; get; }
        public int dept_id { set; get; }
        public string id_num { set; get; }
        public string user_name { set; get; }
        public string user_level { set; get; }
        public string user_email { set; get; }
        public string card_num { set; get; }
        /// <summary>
        /// 移动号码
        /// </summary>
        public string mobile_phone { set; get; }
        /// <summary>
        /// 其他联系方式
        /// </summary>
        public string other_contact { set; get; }
        /// <summary>
        /// 用户职位
        /// </summary>
        public string user_position { set; get; }
    }
    /// <summary>
    /// 用户及部门信息
    /// </summary>
    [Table("fimp.person_dept_view")]
    public class PersonDept: person
    {
        /// <summary>
        /// 部门编码
        /// </summary>
        public string dept_no { set; get; }
        /// <summary>
        /// 部门信息
        /// </summary>
        public string dept_name_cn { set; get; }
    }
}
