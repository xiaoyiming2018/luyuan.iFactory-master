using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 客户端抛送信息表
    /// </summary>
    [Table("fimp.client_config_info")]
    public class client_config_info
    {
        //区域id
        [Key]
        public int id { get; set; }
        //用户名
        public string user_name { get; set; }
        //厂区id
        public int plant_id { get; set; }
        //制程
        public string unit_no { get; set; }
        //线别
        public int line_id { get; set; }
        //站别
        public int station_id { get; set; }
        //设备编码
        public string machine_code { get; set; }
        //IP地址
        public string client_ip { get; set; }
        //Mac码
        public string client_mac { get; set; }
        //抛送时client的状态
        public string throwing_state { get; set; }
        //抛送的内容
        public string push_content { get; set; }
        //插入时间
        public DateTime insert_time { get; set; }
    }
}
