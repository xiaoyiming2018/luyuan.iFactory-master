using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.dept_info")]
    public  class dept_info
    {
        //部门id
        [Key]
        public int dept_id { get; set; }
        //部门代码
        public string dept_no { get; set; }
        //部门英文描述
        public string dept_name_en { get; set; }
        //部门繁体中文描述
        public string dept_name_tw { get; set; }
        //部门简体中文描述
        public string dept_name_cn { get; set; }
        //归属厂别id
        public int plant_id { get; set; }
    }
}
