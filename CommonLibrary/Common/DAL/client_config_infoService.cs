using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class client_config_infoService
    {
        public client_config_info SelectSingle(string client_mac)
        {
            try
            {
                client_config_info obj = new client_config_info();
                string sql = "select * from fimp.client_config_info where client_mac='{0}'";
                sql = string.Format(sql, client_mac);
                obj = PostgreHelper.GetSingleEntity<client_config_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //插入数据
        public int Insert(client_config_info obj)
        {
            try
            {
                string sql = "insert into fimp.client_config_info(user_name, plant_id, unit_no, line_id, station_id, machine_code, client_ip, client_mac, insert_time )values('{0}',{1},'{2}',{3},{4},'{5}','{6}','{7}','{8}')";
                sql = string.Format(sql, obj.user_name, obj.plant_id, obj.unit_no, obj.line_id, obj.station_id, obj.machine_code, obj.client_ip, obj.client_mac, obj.insert_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(client_config_info obj)
        {
            try
            {
                string sql = "Update fimp.client_config_info set plant_id={0}, unit_no='{1}', line_id={2}, station_id={3}, client_ip='{4}', insert_time='{5}', machine_code='{6}' where client_mac='{7}'";
                sql = string.Format(sql, obj.plant_id, obj.unit_no, obj.line_id, obj.station_id, obj.client_ip, obj.insert_time, obj.machine_code, obj.client_mac);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
