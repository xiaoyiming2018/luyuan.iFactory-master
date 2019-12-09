using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// Tag类型枚举,值为后台所对应的id
    /// </summary>
    public enum TagTypeEnum:int
    {
        [Description("稼动率")]
        UtilizationRate=1,
        [Description("在制进度")]
        ProductionSchedule=4,
        [Description("安灯系统异常，包含品质等呼叫")]
        Error=2,
        [Description("现场警示灯")]
        LightTower=3,
        [Description("其他")]
        Other=5,
        [Description("节拍时间类别")]
        CircleTime = 6
    }
}
