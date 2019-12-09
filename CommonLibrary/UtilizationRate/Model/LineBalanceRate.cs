using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("oee.line_balance_rate")]
    public class LineBalanceRate
    {
        [Key]
        public int id { get; set; }

        public int area_id { get; set; }
        //城市id
        public int city_id { get; set; }
        //厂区id
        public int plant_id { get; set; }
        //制程
        public string unit_no { get; set; }
        //线别id
        public int line_id { get; set; }
        //平衡率
        public double balance_rate { get; set; }
        //机种
        public string pn { get; set; }
        //工单
        public string wo { get; set; }

        public DateTime insert_time { get; set; }
    }
}
