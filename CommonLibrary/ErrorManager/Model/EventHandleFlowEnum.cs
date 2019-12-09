using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常处理流程的几个点
    /// </summary>
    public enum EventHandleFlowEnum
    {
        [Description("事件触发")]
        Event_Trigger,
        [Description("事件发生超时")]
        Event_Timeout,
        [Description("事件责任人员签到")]
        Event_SignIn,
        [Description("事件确认结束")]
        Event_Ack
    }
}
