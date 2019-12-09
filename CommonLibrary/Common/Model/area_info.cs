using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.area_info")]
    public  class area_info
    {
        //区域id
        [Key]
        public int area_id { get; set; }
        //区域英文描述
        public string area_name_en { get; set; }
        //区域繁体中文描述
        public string area_name_tw { get; set; }
        //区域简体中文描述
        public string area_name_cn { get; set; }
        //区域时区
        public int time_zone { get; set; }
    }
}
