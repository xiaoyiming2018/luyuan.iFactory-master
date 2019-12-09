using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.line_info")]
    public  class line_info
    {
        //部门id
        public int dept_id { get; set; }
        //归属厂别id
        public int plant_id { get; set; }
        //线别id
        [Key]
        public int line_id { get; set; }
        //制程代码
        public string unit_no { get; set; }
        //线别英文描述
        public string line_name_en { get; set; }
        //线别繁体中文描述
        public string line_name_tw { get; set; }
        //线别简体中文描述
        public string line_name_cn { get; set; }
        //位置
        public string story { get; set; }
        //状态 (停用S，启用A)
        public string status_no { get; set; }

    }
}
