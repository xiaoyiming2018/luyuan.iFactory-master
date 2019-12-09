using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tag_time_class
    {
        ///状态时间表（班次）
        /// <summary>
        /// id
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 设备编码
        /// </summary>
        public string machine_code { get; set; }

        /// <summary>
        /// 系统标签
        /// </summary>
        public string system_tag_code { get; set; }

        /// <summary>
        /// 对应的具体明细id
        /// </summary>
        public int details_id { get; set; }

        /// <summary>
        /// 年
        /// </summary>
        public string year { get; set; }

        /// <summary>
        /// 月
        /// </summary>
        public string month { get; set; }

        /// <summary>
        /// 日
        /// </summary>
        public string day { get; set; }

        /// <summary>
        /// 年-月-日
        /// </summary>
        public string date { get; set; }

        /// <summary>
        /// 班别代码
        /// </summary>
        public string class_no { get; set; }

        /// <summary>
        /// 持续时间(分钟)
        /// </summary>
        public double duration { get; set; }
    }
}
