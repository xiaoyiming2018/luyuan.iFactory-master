using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.attendance")]
    public class Attendance
    {
        [Key]
        public int id { get; set; }
        public string unit_no { get; set; }
        public int line_id { get; set; }
        public string class_no { get; set; }
        public float standard_num { get; set; }
        public float real_num { get; set; }
        public Decimal attendance_rate { get; set; }
        public DateTime createtime { get; set; }

    }
}
