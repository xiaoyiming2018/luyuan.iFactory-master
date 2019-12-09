using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public  class ProScheduleMachineLogService
    {
        
        /// <summary>
        /// 插入Log日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(pro_schedule_machine_log obj)
        {
            int count = 0;
            try
            {
                string sql = "Insert into fimp.pro_schedule_machine_log(work_order,part_num,machine_code,station_id,unit_no,standard_num,actual_num,type,ts)values('{0}','{1}','{2}',{3},'{4}',{5},{6},{7},'{8}')";
                sql = string.Format(sql, obj.work_order, obj.part_num, obj.machine_code,obj.station_id ,obj.unit_no, obj.standard_num, obj.actual_num, obj.type, obj.ts);
                count = PostgreHelper.ExecuteNonQuery(sql);
            }
            catch
            {
                count = 0;
            }
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int Del(string machine_code, string delTime)
        {
            try
            {
                string sql = "delete from fimp.pro_schedule_machine_log where machine_code='{0}' and ts<='{1}'";
                sql = string.Format(sql, machine_code, delTime);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 按照日期删除数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int DelByTime(DateTime delTime)
        {
            try
            {
                string sql = "delete from fimp.pro_schedule_machine_log where ts<='{0}'";
                sql = string.Format(sql, delTime);
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
