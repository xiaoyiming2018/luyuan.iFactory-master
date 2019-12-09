using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 所有区域的信息
    /// </summary>
    [Table("fimp.area_attribute_info")]
    public class area_attribute_info
    {
        //表格名称
        public string tablename { set; get; }
        [Key]
        public int id { set; get; }

        //具体区域下一级的对应项
        public string name { set; get; }
    }
}
