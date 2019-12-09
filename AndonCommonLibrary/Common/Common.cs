using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Advantech.IFactory.Andon
{
    /// <summary>
    /// 消息类别
    /// </summary>
    public enum ErrorMsgType:int
    {
        [Description("无通知")]
        None =0,
        [Description("所有")]
        All =1,
        [Description("微信")]
        WeChat =2,
        [Description("邮件")]
        Email =3,
        [Description("语音广播")]
        Broadcast =4,
        [Description("微信&邮件")]
        WeChat_Email =5,
        [Description("邮件&语音广播")]
        Email_Broadcast =6,
        [Description("微信&语音广播")]
        WeChat_Broadcast =7
    }
    /// <summary>
    /// 消息级别
    /// </summary>
    public enum MessageLevel:int
    {
        [Description("第一级")]
        Level1 = 1,
        [Description("第二级")]
        Level2 = 2,
        [Description("第三级")]
        Level3 = 3
    }
    /// <summary>
    /// 安灯的解除确认模式
    /// </summary>
    public enum AckModeEnum:int
    {
        /// <summary>
        /// 不需要确认模式
        /// </summary>
        NoAck =0,
        /// <summary>
        /// 以异常代码方式确认
        /// </summary>
        CodeAck=1,
        /// <summary>
        /// 无需提供异常代码方式确认
        /// </summary>
        WithoutCodeAck = 2,
        /// <summary>
        /// 在web页面进行登记解除
        /// </summary>
        WebRegisterAck = 3
    }
    /// <summary>
    /// 人员签到方式
    /// </summary>
    public enum ArrivalModeEnum:int
    {
        /// <summary>
        /// 不需要确认模式
        /// </summary>
        NoArrival = 0,
        /// <summary>
        /// 员工卡号方式确认
        /// </summary>
        CardArrival = 1,
        /// <summary>
        /// 无需提供员工卡号方式确认
        /// </summary>
        WithoutCardArrival = 2
    }
}
