using Advantech.IFactory.CommonHelper;
using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Advantech.IFactory.CommonLibrary
{

    #region SQL Base
    /// <summary>
    /// 标签类型信息
    /// </summary>
    [Table("fimp.system_tag_type")]
    public class System_tag_type
    {
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string type_name_en { set; get; }
        /// <summary>
        /// 中文类别名称
        /// </summary>
        public string type_name_cn { set; get; }
        /// <summary>
        /// 繁体类别名称
        /// </summary>
        public string type_name_tw { set; get; }
        /// <summary>
        /// 描述信息
        /// </summary>
        public string type_description { set; get; }
    }
    /// <summary>
    /// 系统标签信息
    /// </summary>
    [Table("fimp.system_tag_code")]
    public class system_tag_code
    {
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 类别id
        /// </summary>
        public int type_id { set; get; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string code_name_en { set; get; }
        /// <summary>
        /// 中文类别名称
        /// </summary>
        public string code_name_cn { set; get; }
        /// <summary>
        /// 繁体类别名称
        /// </summary>
        public string code_name_tw { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string code_description { set; get; }
    }
    /// <summary>
    /// web扩展类
    /// </summary>
    public class system_tag_code_web: system_tag_code
    {
        public string type_name_en { set; get; }
    }

        /// <summary>
        /// webaccess标签信息
        /// </summary>
        public class webaccess_tag_info_Base
    {
        public int id { set; get; }
        /// <summary>
        /// 系统类别编码(英文)
        /// </summary>
        public string system_type_code { set; get; }
        /// <summary>
        /// 标签编码
        /// </summary>
        public string tag_code { set; get; }
        /// <summary>
        /// 系统标签编码(英文)
        /// </summary>
        public string system_tag_code { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string tag_description { set; get; }
    }
    /// <summary>
    /// webaccess标签信息
    /// </summary>
    [Table("fimp.webaccess_tag_info")]
    public class webaccess_tag_info : webaccess_tag_info_Base
    {
        /// <summary>
        /// 阈值最小值
        /// </summary>
        public decimal min_threshold { set; get; }
        /// <summary>
        /// 阈值最大值
        /// </summary>
        public decimal max_threshold { set; get; }
        /// <summary>
        /// 所属区域
        /// 0=machine_info;1=station_info;2=line_info;3=dept_info;4=unit_info;5=plant_info;6=city_info;7=area_info;
        /// </summary>
        public int area_attribute { set; get; }
        /// <summary>
        /// 所属区域属性所对应的值，一般为id
        /// </summary>
        public int area_attribute_value { set; get; }
        /// <summary>
        /// 所对应的云端scada_id号码
        /// </summary>
        public string scada_id { set; get; }
    }
    /// <summary>
    /// 标签值
    /// </summary>
    public class tag_value_trend
    {
        public int id { set; get; }
        /// <summary>
        /// 标签编码
        /// </summary>
        public string tag_code { set; get; }
        /// <summary>
        /// 值
        /// </summary>
        public string tag_value { set; get; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { set; get; }
    }
    #endregion
    /// <summary>
    /// Web页面扩展类
    /// </summary>
    [Table("fimp.area_attribute_view")]
    public class webaccess_tag_info_web : webaccess_tag_info
    {
        public  string area_attribute_name { set; get; }

        public  string area_attribute_sub_name { set; get; }
    }
    /// <summary>
    /// 工位tag
    /// </summary>
    [Table("fimp.station_tag")]
    public class StationWaTag: webaccess_tag_info
    {
        /// <summary>
        /// 工位英文
        /// </summary>
        public string station_name_en { set; get; }
        /// <summary>
        /// 工位中文
        /// </summary>
        public string station_name_cn { set; get; }
    }
    /// <summary>
    /// 系统标签的所有详细信息
    /// </summary>
    public class SysTagCodeAndType : system_tag_code
    {
        /// <summary>
        /// 类别名称
        /// </summary>
        public string type_name_en { set; get; }
        /// <summary>
        /// 中文类别名称
        /// </summary>
        public string type_name_cn { set; get; }
        /// <summary>
        /// 繁体类别名称
        /// </summary>
        public string type_name_tw { set; get; }
        /// <summary>
        /// 类别描述
        /// </summary>
        public string type_description { set; get; }
    }
    /// <summary>
    ///组态设定的Tag和类型的完整信息
    /// </summary>
    public class WATagInfoAndType : webaccess_tag_info_Base
    {
        /// <summary>
        /// 系统标签中文名称
        /// </summary>
        public string system_code_name_cn { set; get; }
        /// <summary>
        /// 中文类别名称
        /// </summary>
        public string system_type_code_cn { set; get; }
    }

    #region TagValueInfo扩展类
    /// <summary>
    ///Tag值完整的信息
    /// </summary>
    public class WATagValueInfo : WATagInfoAndType, IGetDeviceTagValue
    {
        /// <summary>
        /// 值
        /// </summary>
        public string tag_value { set; get; }
        /// <summary>
        /// 插入时间
        /// </summary>
        public DateTime insert_time { set; get; }

        public virtual List<T> GetDeviceTagValue<T>(List<MongoDbTag> Tags)
        {
            throw new NotImplementedException();
        }
    }
    /// <summary>
    /// 设备所对应的完整Tag值信息
    /// </summary>
    public class DeviceTagValueInfo : WATagValueInfo
    {
        /// <summary>
        /// 所对应的设备id
        /// </summary>
        public int device_id { set; get; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string device_code { set; get; }
        
    }
    #endregion

    /// <summary>
    ///Tag和类型的完整信息
    /// </summary>
    public class TagAndTypeInfo
    {
        /// <summary>
        /// taginfo所对应的id
        /// </summary>
        public int id { set; get; }
        /// <summary>
        /// 标签编码
        /// </summary>
        public string tag_code { set; get; }
        /// <summary>
        /// 标签所对应系统的编码
        /// </summary>
        public String system_code { set; get; }
        /// <summary>
        /// 描述
        /// </summary>
        public string description { set; get; }
        /// <summary>
        /// 类别名称
        /// </summary>
        public string type_name_en { set; get; }
        /// <summary>
        /// 中文类别名称
        /// </summary>
        public string type_name_cn { set; get; }
    }
    /// <summary>
    /// 设备所对应的完整Tag信息
    /// </summary>
    public class MachineTagInfo : WATagInfoAndType
    {
        /// <summary>
        /// 所对应的设备id
        /// </summary>
        public int machine_id { set; get; }
        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { set; get; }
    }

    /// <summary>
    /// Station所对应的完整Tag信息
    /// </summary>
    public class StationTagInfo : WATagInfoAndType
    {
        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { set; get; }
        /// <summary>
        /// 站位名称
        /// </summary>
        public string station_name_en { set; get; }
        
    }
    public interface IGetDeviceTagValue
    {
        List<T> GetDeviceTagValue<T>(List<MongoDbTag> Tags);
    }
    public class TagIdAndValue
    {
        public TagIdAndValue()
        {
            id = -1;
            value = String.Empty;
            value2 = String.Empty;
        }
        public int id { set; get; }
        public string value { set; get; }
        /// <summary>
        /// 第二个值
        /// </summary>
        public string value2 { set; get; }
    }
}
