using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorLogPersonService
    {
        public int Insert(error_log_person Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<error_log_person>(Obj);
            return id;
        }

        public int Update(error_log_person Obj)
        {
            int count = PostgreHelper.UpdateSingleEntity<error_log_person>(Obj);
            return count;
        }
        public int Del(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_log_person where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Del(DateTime start_time, DateTime end_time)
        {
            try
            {
                int count = 0;
                string sql = "delete from andon.error_log_person where insert_time>='{0}' and insert_time<='{1}'";
                sql = string.Format(sql, start_time, end_time);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<error_log_person> GetErrorLogPersonByLogId(int LogId)
        {
            string command = string.Format("Select * from andon.error_log_person where error_log_id='{0}'", LogId);
            var list = PostgreHelper.GetEntityList<error_log_person>(command);
            return list;
        }

    }
}
