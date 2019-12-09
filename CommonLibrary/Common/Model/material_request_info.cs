using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 物料呼叫需求信息
    /// </summary>
    [Table("fimp.material_request_info")]
    public class material_request_info
    {
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id { get; set; }
        /// <summary>
        /// 物料id
        /// </summary>
        public int material_id { get; set; }
        /// <summary>
        /// 站位id
        /// </summary>
        public int station_id { get; set; }
        /// <summary>
        /// 呼叫人员id
        /// </summary>
        public int request_person_id { get; set; }
        /// <summary>
        /// 工单号码
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// 机种编号
        /// </summary>
        public string part_num { get; set; }
        /// <summary>
        /// 需求数量
        /// </summary>
        public int request_count { get; set; }
        /// <summary>
        /// 领料人员id
        /// </summary>
        public int take_person_id { get; set; }
        /// <summary>
        /// 领料时间
        /// </summary>
        public DateTime take_time { get; set; }
        /// <summary>
        /// 领料时间
        /// </summary>
        public DateTime depot_ack_time { get; set; }
        /// <summary>
        /// 领料时间
        /// </summary>
        public DateTime request_ack_time { get; set; }
        /// <summary>
        /// 备注信息
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime createtime { get; set; }
    }

    public class RequestAndInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public int id { get; set; }
        /// <summary>
        /// 机台名称
        /// </summary>
        public string station_name { get; set; }
        /// <summary>
        /// 物料代码
        /// </summary>
        public string material_code { get; set; }
        /// <summary>
        /// 物料名称
        /// </summary>
        public string material_name { get; set; }
        /// <summary>
        /// 物料类型
        /// </summary>
        public string material_type { get; set; }
        /// <summary>
        /// 呼叫数量
        /// </summary>
        public int request_count { get; set; }
        /// <summary>
        /// 工单
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// 确认人员
        /// </summary>
        public int take_person_id { get; set; }
        /// <summary>
        /// 领料人员
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// 创建时间(呼叫时间)
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// 配送人员确认时间
        /// </summary>
        public DateTime depot_ack_time { get; set; }
    }
}