using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tag
    {
        ///及时状态表
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

        /// <summary>
        /// 状态是否是有变更过
        /// </summary>
        public string whether { get; set; }

    }
}
