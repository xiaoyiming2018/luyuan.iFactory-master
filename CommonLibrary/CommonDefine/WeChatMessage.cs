using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    //public class Feedback
    //{
    //    public string touser { get; set; }

    //    public string toparty { get; set; }

    //    public string totag { get; set; }

    //    public string msgtype { get; set; }

    //    public int agentid { get; set; }

    //    public string text { get; set; }

    //    public int safe { get; set; }
    //}
    /// <summary>
    /// 消息基类
    /// </summary>
    public class MessageBase
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }
        /// <summary>
        /// 消息主题
        /// </summary>
        public string subject { get; set; }
        /// <summary>
        /// 消息内容，最长不超过2048个字节
        /// </summary>
        public string text { get; set; }
        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        public int safe { get; set; }
        /// <summary>
        /// 标签
        /// </summary>
        public string tag { get; set; }
    }
    /// <summary>
    /// 微信消息通知
    /// </summary>
    public class WeChatMessage: MessageBase
    {
        /// <summary>
        /// 所要发向的成员
        /// </summary>
        public string touser { get; set; }
        /// <summary>
        /// 所要发向的部门
        /// </summary>
        public string toparty { get; set; }
    }
    /// <summary>
    /// 邮件消息
    /// </summary>
    public class eMailMessage : MessageBase
    {
        /// <summary>
        /// 邮件地址列表
        /// </summary>
        public List<String> MailAddressList { set; get; }
    }
}
