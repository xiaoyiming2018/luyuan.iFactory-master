using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonHelper
{
    public class MqttTagEventArgs : EventArgs
    {
        private String _tagName;
        private String _tagValue;
        public MqttTagEventArgs(String TagName, string TagValue)
        {
            this._tagName = TagName;
            this._tagValue = TagValue;
        }
        /// <summary>
        /// 标签名称
        /// </summary>
        public string TagName
        {
            get { return _tagName; }
        }
        /// <summary>
        /// 标签值
        /// </summary>
        public string TagValue
        {
            get { return _tagValue; }
        }
    }
    /// <summary>
    /// mqtt消息对象
    /// </summary>
    public class MqttMessage
    {
        private String _Topic;
        private String _Message;
        public MqttMessage(String Topic, string Message)
        {
            this._Topic = Topic;
            this._Message = Message;
        }
        /// <summary>
        /// 主题
        /// </summary>
        public String Topic
        {
            get
            {
                return _Topic;
            }
        }
        /// <summary>
        /// 消息内容
        /// </summary>
        public String Message
        {
            get
            {
                return _Message;
            }
        }
    }
}
