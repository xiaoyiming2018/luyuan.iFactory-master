using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace iFactory.Op.Models
{
    /// <summary>
    /// 缓存的配置信息类别
    /// </summary>
    public enum CookiesEnum
    {
        /// <summary>
        /// 城市id
        /// </summary>
        CityId,
        /// <summary>
        /// 车间id
        /// </summary>
        plantId,
        /// <summary>
        /// 制程编码
        /// </summary>
        UnitNo,
        /// <summary>
        /// 线别id
        /// </summary>
        LineId,
        /// <summary>
        /// 站位id
        /// </summary>
        StationId,
        /// <summary>
        /// 设备编码
        /// </summary>
        MachineCode
    }
}
