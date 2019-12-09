using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.plant_info")]
    public  class plant_info
    {
        //城市id
        public int city_id { get; set; }
        //厂区id
        [Key]
        public int plant_id { get; set; }
        //厂区英文描述
        public string plant_name_en { get; set; }
        //厂区繁体中文描述
        public string plant_name_tw { get; set; }
        //厂区简体中文描述
        public string plant_name_cn { get; set; }
    }
}
