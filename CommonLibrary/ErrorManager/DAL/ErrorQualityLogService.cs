using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorQualityLogService
    {
        public int Insert(error_quality_log obj)
        {

            try
            {
                string sql = "Insert into andon.error_quality_log(error_log_id,error_type_id,station_id,machine_code,defectives_count,quality_description,quality_reason,improve_plan,plan_date,responsible_person,remark,insert_time)" +
                    "values({0},{1},{2},'{3}',{4},'{5}','{6}','{7}','{8}','{9}','{10}','{11}')";
                sql = string.Format(sql, obj.error_log_id, obj.error_type_id, obj.station_id, obj.machine_code, obj.defectives_count,obj.quality_description,obj.quality_reason,obj.improve_plan,obj.plan_date,obj.responsible_person,obj.remark,obj.insert_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(error_quality_log obj)
        {
            int count=0;
            try
            {
                count = PostgreHelper.UpdateSingleEntity<error_quality_log>(obj);
                return count;
            }
            catch (Exception ex)
            {
                return count;
            }
        }

        public error_quality_log GetQualityLog(int LogId)
        {
            error_quality_log obj = new error_quality_log();
            try
            {
                string sql = "select * from andon.error_quality_log where error_log_id='{0}'";
                sql = string.Format(sql, LogId);
                obj = PostgreHelper.GetSingleEntity<error_quality_log>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
