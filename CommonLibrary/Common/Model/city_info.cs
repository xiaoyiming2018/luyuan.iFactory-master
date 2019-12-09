using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.city_info")]
    public  class city_info
    {
        //区域id
        public int area_id { get; set; }
        //城市id
        [Key]
        public int city_id { get; set; }
        //城市英文描述
        public string city_name_en { get; set; }
        //城市繁体中文描述
        public string city_name_tw { get; set; }
        //城市简体中文描述
        public string city_name_cn { get; set; }
    }
}
