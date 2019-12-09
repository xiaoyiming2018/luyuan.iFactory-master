using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.material_info")]
    public class material_info
    {
        //标识键
        [Key]
        public int id { get; set; }
        //类别编码
        public int category_id { get; set; }
        //类别名称
        public string type_name { get; set; }
        //物料编码
        public string material_code { get; set; }
        //物料名称
        public string material_name { get; set; }
        //物料型号
        public string material_type { get; set; }
        //库存数量
        public int material_inventory { get; set; }
        //备注
        public string remark { get; set; }
        //创建时间
        public DateTime createtime { get; set; }
    }
}
