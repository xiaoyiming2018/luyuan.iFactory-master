using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 警示灯输出
    /// </summary>
    public enum LightTowerEnum:int
    {
        /// <summary>
        /// 无输出
        /// </summary>
        None =0,
        /// <summary>
        /// 红灯输出
        /// </summary>
        red_light_on = 1,
        /// <summary>
        /// 黄灯输出
        /// </summary>
        yellow_light_on = 2,
        /// <summary>
        /// 绿灯输出
        /// </summary>
        green_light_on = 3,
        /// <summary>
        /// 蓝灯输出
        /// </summary>
        blue_light_on = 4,
        /// <summary>
        /// 白灯输出
        /// </summary>
        white_light_on = 5
    }
}
