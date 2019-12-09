using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.unit_info")]
    public class unit_info
    {
        //制程代码
        public string unit_no { get; set; }
        //制程英文描述
        public string unit_name_en { get; set; }
        //制程繁体中文描述
        public string unit_name_tw { get; set; }
        //制程简体中文描述
        public string unit_name_cn { get; set; }
        //制程排序顺序
        [Key]
        public int seq { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public int dept_id { get; set; }
    }
}