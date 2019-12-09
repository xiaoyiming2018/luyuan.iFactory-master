using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 标签区域属性枚举
    /// </summary>
    public enum TagAreaAttributeEnum:int
    {
        /// <summary>
        /// 设备
        /// </summary>
        machine_info =0,
        /// <summary>
        /// 站位
        /// </summary>
        station_info =1,
        /// <summary>
        /// 线别
        /// </summary>
        line_info =2,
        /// <summary>
        /// 部门
        /// </summary>
        dept_info =3,
        /// <summary>
        /// 制程
        /// </summary>
        unit_info =4,
        /// <summary>
        /// 车间
        /// </summary>
        plant_info = 5,
        /// <summary>
        /// 城市
        /// </summary>
        city_info=6,
        /// <summary>
        /// 区域
        /// </summary>
        area_info = 7
    }
}
