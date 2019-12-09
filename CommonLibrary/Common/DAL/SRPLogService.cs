using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SRPLogService
    {
        /// <summary>
        /// 执行新的日志记录
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(srp_log obj)
        {
            try
            {
                string sql = "insert into fimp.srp_log(content,create_time)values('{0}','{1}')";
                sql = string.Format(sql, obj.content, obj.create_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 执行新的日志记录
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Delete(DateTime datetime)
        {
            try
            {
                string sql = "delete from fimp.srp_log where create_time<='{0}'";
                sql = string.Format(sql, datetime);
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
