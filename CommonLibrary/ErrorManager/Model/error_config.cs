using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常配置
    /// </summary>
    [Table("andon.error_config")]
    public class error_config
    {
        /// <summary>
        /// id
        /// </summary>
        [Key]
        public int id { set; get; }
        /// <summary>
        /// 制程
        /// </summary>
        public string unit_no { set; get; }
        /// <summary>
        /// 线别id
        /// </summary>
        public int line_id { set; get; }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string error_name { set; get; }
        /// <summary>
        /// 异常编码所对应的系统Tag编码
        /// </summary>
        public string system_tag_code { set; get; }
        /// <summary>
        /// 是否激活
        /// </summary>
        public int error_active { set; get; }
        /// <summary>
        /// 激活次数
        /// </summary>
        public int trigger_count { set; get; }
        /// <summary>
        /// 触发时输出颜色
        /// </summary>
        public int trigger_out_color { set; get; }
        /// <summary>
        /// 触发时消息通知类型
        /// </summary>
        public int trigger_message_type { set; get; }
        /// <summary>
        /// 多级别通知。=0单级别，=1多级别
        /// </summary>
        public int message_multilevel { set; get; }
        /// <summary>
        /// 是否需要确认才能解除
        /// </summary>
        public int check_ack { set; get; }
        /// <summary>
        /// 超时设置(分钟)
        /// </summary>
        public int timeout_setting { set; get; }
        /// <summary>
        /// 微信通知类型
        /// </summary>
        public int wechat_type { set; get; }
        /// <summary>
        /// 是否需要检查到岗
        /// </summary>
        public int check_arrival { set; get; }
        /// <summary>
        /// 到岗通知类型
        /// </summary>
        public int arrival_message_type { set; get; }
        /// <summary>
        /// 到岗输出的颜色灯
        /// </summary>
        public int arrival_out_color { set; get; }
        /// <summary>
        /// 机种列表
        /// </summary>
        public string part_num { set; get; }

        public string description { set; get; }
    }
}
