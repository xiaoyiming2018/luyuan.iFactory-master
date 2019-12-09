using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tag_log
    {
        ///Log日志表
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
        /// 插入时间
        /// </summary>
        public string insert_time { get; set; }
    }
}
