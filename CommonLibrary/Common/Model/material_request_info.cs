using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// ���Ϻ���������Ϣ
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
        /// ����id
        /// </summary>
        public int material_id { get; set; }
        /// <summary>
        /// վλid
        /// </summary>
        public int station_id { get; set; }
        /// <summary>
        /// ������Աid
        /// </summary>
        public int request_person_id { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// ���ֱ��
        /// </summary>
        public string part_num { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int request_count { get; set; }
        /// <summary>
        /// ������Աid
        /// </summary>
        public int take_person_id { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime take_time { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime depot_ack_time { get; set; }
        /// <summary>
        /// ����ʱ��
        /// </summary>
        public DateTime request_ack_time { get; set; }
        /// <summary>
        /// ��ע��Ϣ
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// ����ʱ��
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
        /// ��̨����
        /// </summary>
        public string station_name { get; set; }
        /// <summary>
        /// ���ϴ���
        /// </summary>
        public string material_code { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string material_name { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string material_type { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public int request_count { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public string work_order { get; set; }
        /// <summary>
        /// ȷ����Ա
        /// </summary>
        public int take_person_id { get; set; }
        /// <summary>
        /// ������Ա
        /// </summary>
        public string user_name { get; set; }
        /// <summary>
        /// ����ʱ��(����ʱ��)
        /// </summary>
        public DateTime createtime { get; set; }
        /// <summary>
        /// ������Աȷ��ʱ��
        /// </summary>
        public DateTime depot_ack_time { get; set; }
    }
}