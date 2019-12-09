using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;


namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateDayService
    {
        public List<UtilizationRateDay> SelectAll(string machine_code, string startDate, string endDate, string unit_no)
        {
            try
            {
                List<UtilizationRateDay> objList = new List<UtilizationRateDay>();
                string sql = null;
                if (!string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from oee.utilization_rate_day where machine_code in({0}) and date>='{1}' and date<='{2}' order by date desc,machine_code";
                    sql = string.Format(sql, machine_code, startDate, endDate);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from oee.utilization_rate_day where  date>='{0}' and date<='{1}' order by date desc,machine_code";
                    sql = string.Format(sql, startDate, endDate);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from oee.utilization_rate_day where  date>='{0}' and date<='{1}' and machine_code like'{2}%' order by date desc,machine_code";
                    sql = string.Format(sql, startDate, endDate, unit_no);
                }
                else
                {
                    sql = "select * from oee.utilization_rate_day order by date desc,machine_code";
                }

                objList = PostgreHelper.GetEntityList<UtilizationRateDay>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(UtilizationRateDay obj)
        {
            try
            {
                string sql = "insert into oee.utilization_rate_day(machine_code,year,month,day,date,run_time,error_time,others_time,rest_time,boot_time,utilization_rate)values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10})";
                sql = string.Format(sql, obj.machine_code, obj.year, obj.month, obj.day, obj.date, obj.run_time, obj.error_time, obj.others_time, obj.rest_time, obj.boot_time, obj.utilization_rate);
                //int count = Helper.SqlHelper.ExecuteNonQuery(sql);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取ID
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public int GetID(string machine_code, string date)
        {
            try
            {
                int id = 0;
                string sql = "select * from oee.utilization_rate_day where machine_code='{0}' and date='{1}'";
                sql = string.Format(sql, machine_code, date);
                
                UtilizationRateDay obj = PostgreHelper.GetSingleEntity<UtilizationRateDay>(sql);
                if (obj != null)
                {
                    id = obj.id;
                }
                return id;
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
        public int Update(UtilizationRateDay obj)
        {
            try
            {
                int count = PostgreHelper.UpdateSingleEntity<UtilizationRateDay>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public UtilizationRateDay SelectSingle(string machine_code, string date)
        {
            try
            {
                UtilizationRateDay obj = new UtilizationRateDay();
                string sql = "select * from oee.utilization_rate_day where machine_code='{0}' and date='{1}'";
                sql = string.Format(sql, machine_code, date);
                
                obj = PostgreHelper.GetSingleEntity<UtilizationRateDay>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
