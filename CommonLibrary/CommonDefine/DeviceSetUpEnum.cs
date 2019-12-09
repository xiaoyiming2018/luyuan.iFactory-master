using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 设备状态设置枚举
    /// </summary>
    public enum DeviceSetUpEnum:int
    {
        /// <summary>
        /// 休息优先
        /// </summary>
        BreakPriority =1,
        /// <summary>
        /// 运行优先
        /// </summary>
        RunPriority=2
    }
}
