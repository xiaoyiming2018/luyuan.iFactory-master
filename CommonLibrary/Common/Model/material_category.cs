using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.material_category")]
    public class material_category
    {
        [Key]
        public int id { get; set; }
        public string type_code { get; set; }
        public string type_name { get; set; }
        public string unit_no { get; set; }
        public int line_id { get; set; }
        public string remark { get; set; }
        public DateTime createtime { get; set; }

    }
}
