using System;
using System.Collections.Generic;

using System.Data.Common;

using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class RestTimeService
    {
        /// <summary>
        /// 查询所有数据
        /// </summary>
        /// <returns></returns>
        public List<rest_time> SelectAll(string unit_no = null)
        {
            try
            {
                List<rest_time> objList = new List<rest_time>();
                string sql = null;
                if (string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from fimp.rest_time order by unit_no, start_time";
                }
                else
                {
                    sql = "select * from fimp.rest_time where unit_no='{0}' order by unit_no, start_time";
                    sql = string.Format(sql, unit_no);
                }
                objList = PostgreHelper.GetEntityList<rest_time>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public rest_time SelectSingle(int id)
        {
            try
            {
                string sql = "select * from fimp.rest_time where id={0}";
                sql = string.Format(sql, id);
                rest_time obj = new rest_time();
                obj = PostgreHelper.GetSingleEntity<rest_time>(sql);
                
                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(rest_time obj)
        {
            try
            {
                string sql = "Insert into fimp.rest_time(start_time,end_time,unit_no,state)values('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql, obj.start_time, obj.end_time,obj.unit_no,obj.state);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(rest_time obj)
        {
            try
            {
                string sql = "update fimp.rest_time set start_time='{0}',end_time='{1}',unit_no='{2}',state='{3}' where id={4}";
                sql = string.Format(sql, obj.start_time, obj.end_time,obj.unit_no ,obj.state,obj.id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.rest_time where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新unit_no
        /// </summary>
        /// <param name="old_unit_no">旧</param>
        /// <param name="unit_no">新</param>
        /// <returns></returns>
        public int UpdateUnitNo(string old_unit_no, string unit_no)
        {
            try
            {
                string sql = "Update fimp.rest_time set unit_no='{0}' where unit_no='{1}'";
                sql = string.Format(sql, unit_no, old_unit_no);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public rest_time SelectTime(string unit_no)
        {
            try
            {
                string sql = "select * from fimp.rest_time where unit_no={0}";
                sql = string.Format(sql, unit_no);
                rest_time obj = new rest_time();
                obj = PostgreHelper.GetSingleEntity<rest_time>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询时间是否在休息时间列表中
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        public bool SelectCount(string time, string unit_no)
        {
            try
            {
                bool result = false;
                string sql = "select * from fimp.rest_time where start_time<='{0}' and end_time>='{1}' and unit_no='{2}' and state='A'";
                sql = string.Format(sql, time, time, unit_no);
                rest_time obj = PostgreHelper.GetSingleEntity<rest_time>(sql);

                if (obj != null)
                {
                    result = true;
                }
                else
                {
                    result = false;
                }

                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
