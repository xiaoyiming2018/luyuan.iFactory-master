using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.class_info")]
    public  class class_info
    {
        //班别id
        [Key]
        public int class_id { get; set; }
        //班别代码
        public string class_no { get; set; }
        //班别英文描述
        public string class_name_en { get; set; }
        //班别繁体中文描述
        public string class_name_tw { get; set; }
        //班别简体中文描述
        public string class_name_cn { get; set; }
        //开始时间
        public string start_time { get; set; }
        //结束时间
        public string end_time { get; set; }

    }
}
