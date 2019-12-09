using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// �ͻ���������Ϣ��
    /// </summary>
    [Table("fimp.client_config_info")]
    public class client_config_info
    {
        //����id
        [Key]
        public int id { get; set; }
        //�û���
        public string user_name { get; set; }
        //����id
        public int plant_id { get; set; }
        //�Ƴ�
        public string unit_no { get; set; }
        //�߱�
        public int line_id { get; set; }
        //վ��
        public int station_id { get; set; }
        //�豸����
        public string machine_code { get; set; }
        //IP��ַ
        public string client_ip { get; set; }
        //Mac��
        public string client_mac { get; set; }
        //����ʱclient��״̬
        public string throwing_state { get; set; }
        //���͵�����
        public string push_content { get; set; }
        //����ʱ��
        public DateTime insert_time { get; set; }
    }
}
