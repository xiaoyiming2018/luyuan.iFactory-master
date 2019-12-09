using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    [Table("fimp.srp_log")]
    public class srp_log
    {
        public srp_log()
        { }
        public srp_log(string Content,DateTime Time)
        {
            this.content = Content;
            this.create_time = Time;
        }
        [Key]
        public int id { set; get; }
        public string content { set; get; }
        public DateTime create_time { set; get; }
    }
}
