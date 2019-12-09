using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class LineBalanceRateService
    {
        //插入数据
        public int Insert(LineBalanceRate obj)
        {
            try
            {
                string sql = "Insert into oee.line_balance_rate(area_id,city_id,plant_id,unit_no,line_id,balance_rate,pn,wo,insert_time)values({0},{1},{2},'{3}',{4},{5},'{6}','{7}','{8}')";
                sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id, obj.balance_rate, obj.pn, obj.wo, obj.insert_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(LineBalanceRate obj)
        {
            try
            {
                int count = PostgreHelper.UpdateSingleEntity<LineBalanceRate>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<LineBalanceRate> SelectAll()
        {
            try
            {
                List<LineBalanceRate> objList = new List<LineBalanceRate>();
                string sql = "SELECT id,area_id,city_id,plant_id,unit_no,line_id,balance_rate,pn,wo,insert_time FROM oee.line_balance_rate order by insert_time desc";
                objList= PostgreHelper.GetEntityList<LineBalanceRate>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LineBalanceRate SelectSingle(int line_id, string date_time = null)
        {
            try
            {
                LineBalanceRate obj = new LineBalanceRate();
                string sql = null;
                if (string.IsNullOrEmpty(date_time))
                {
                    sql = "select * from oee.line_balance_rate where line_id={0} order by insert_time desc LIMIT 1";
                    sql = string.Format(sql, line_id);
                }
                else
                {
                    string insert_time = Convert.ToDateTime(date_time).ToString("yyyy-MM-dd");
                    sql = "select * from oee.line_balance_rate where line_id={0} and insert_time>'{1}' order by insert_time desc LIMIT 1";
                    sql = string.Format(sql, line_id, insert_time);
                }

                obj = PostgreHelper.GetSingleEntity<LineBalanceRate>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public LineBalanceRate SelectSingle(int line_id,string work_order, string date_time = null)
        {
            try
            {
                LineBalanceRate obj = new LineBalanceRate();
                string sql = null;
                if (string.IsNullOrEmpty(date_time))
                {
                    sql = "select * from oee.line_balance_rate where line_id={0} and wo='{1}' order by insert_time desc  LIMIT 1";
                    sql = string.Format(sql, line_id, work_order);
                }
                else
                {
                    string insert_time = Convert.ToDateTime(date_time).ToString("yyyy-MM-dd");
                    sql = "select * from oee.line_balance_rate where line_id={0} and wo='{1}' and insert_time>'{2}' order by insert_time desc  LIMIT 1";
                    sql = string.Format(sql, line_id, work_order, insert_time);
                }

                obj = PostgreHelper.GetSingleEntity<LineBalanceRate>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            try
            {
                string comm = "delete from oee.line_balance_rate where insert_time<='{0}'";
                comm = string.Format(comm, datetime);
                int count = PostgreHelper.ExecuteNonQuery(comm);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
