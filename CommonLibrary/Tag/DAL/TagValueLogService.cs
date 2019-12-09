using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagValueLogService
    {
        public int Insert(tag_value_log obj)
        {
            int count = PostgreHelper.InsertSingleEntity<tag_value_log>(obj);
            return count;
        }

        public int Delete(DateTime datetime)
        {
            try
            {
                string sql = "delete from fimp.tag_value_log where insert_time<='{0}'";
                sql = string.Format(sql, datetime);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 按照日期删除
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            try
            {
                string sql = "delete from fimp.tag_value_log where insert_time<='{0}'";
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
