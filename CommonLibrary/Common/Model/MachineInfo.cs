using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    //设备（站别表）
    [Table("fimp.machine_info")]
    public class MachineInfo
    {
        //设备id
        [Key]
        public int machine_id { get; set; }
        //区域id
        public int area_id { get; set; }
        //城市id
        public int city_id { get; set; }
        //厂区id
        public int plant_id { get; set; }
        //制程
        public string unit_no { get; set; }
        //线别id
        public int line_id { get; set; }
        //设备编码（唯一）
        public string machine_code { get; set; }
        //设备名称英文
        public string machine_name_en { get; set; }
        //设备名称繁体
        public string machine_name_tw { get; set; }
        //设备名称简体
        public string machine_name_cn { get; set; }
        //站别编码
        public int station_id { get; set; }
        //站别开启状态(停用S，启用A)
        public string status_no { get; set; }
        //mqtt开启状态(停用S，启用A)/////新增的，需要加入到数据库 
        public string mqtt_no { get; set; }
        //是否设置休息优先1是,0不是
        public int set_up { get; set; }
       

    }

    public class MachineInfoName
    {
        //城市id
        public int city_id { get; set; }
        //厂区id
        public int plant_id { get; set; }
        //线别id
        public int line_id { get; set; }
        //站别编码
        public int station_id { get; set; }
        //城市
        public string city_name_en { get; set; }
        //厂区
        public string plant_name_en { get; set; }
        //城市
        public string city_name_cn { get; set; }
        //厂区
        public string plant_name_cn { get; set; }
        //制程
        public string unit_no { get; set; }
        //线别
        public string line_name_en { get; set; }
        //站别
        public string station_name_en { get; set; }
        public string line_name_cn { get; set; }
        //站别
        public string station_name_cn { get; set; }
        //设备编码（唯一）
        public string machine_code { get; set; }
        //设备名称英文
        public string machine_name_en { get; set; }
        //设备名称繁体
        public string machine_name_tw { get; set; }
        //设备名称简体
        public string machine_name_cn { get; set; }
        //站别编码


    }
}
