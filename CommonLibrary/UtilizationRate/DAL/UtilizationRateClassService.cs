using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateClassService
    {
        public List<UtilizationRateClass> SelectAll(string machine_code, string startDate, string endDate, string unit_no, string class_no)
        {
            try
            {
                List<UtilizationRateClass> objList = new List<UtilizationRateClass>();
                string sql = null;
                if (!string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(unit_no) && string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from oee.utilization_rate_class where machine_code in({0}) and date>='{1}' and date<='{2}' order by date desc,class_no,machine_code";
                    sql = string.Format(sql, machine_code, startDate, endDate);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(unit_no) && string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from oee.utilization_rate_class where  date>='{0}' and date<='{1}' order by date desc,class_no,machine_code";
                    sql = string.Format(sql, startDate, endDate);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(unit_no) && string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from oee.utilization_rate_class where  date>='{0}' and date<='{1}' and machine_code like'{2}%' order by date desc,class_no,machine_code";
                    sql = string.Format(sql, startDate, endDate, unit_no);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && string.IsNullOrEmpty(unit_no) && !string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from oee.utilization_rate_class where  date>='{0}' and date<='{1}' and class_no='{2}' order by date desc,class_no,machine_code";
                    sql = string.Format(sql, startDate, endDate, class_no);
                }
                else if (string.IsNullOrEmpty(machine_code) && !string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(endDate) && !string.IsNullOrEmpty(unit_no) && !string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from oee.utilization_rate_class where  date>='{0}' and date<='{1}' and machine_code like'{2}%' and class_no='{3}'  order by date desc,class_no,machine_code";
                    sql = string.Format(sql, startDate, endDate, unit_no, class_no);
                }
                else
                {
                    sql = "select * from utilization_rate_class order by date desc,class_no,machine_code";
                }
                objList = PostgreHelper.GetEntityList<UtilizationRateClass>(sql);
                
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
        public int Insert(UtilizationRateClass obj)
        {
            try
            {
                string sql = "insert into oee.utilization_rate_class(machine_code,year,month,day,date,run_time,error_time,others_time,rest_time,boot_time,utilization_rate,class_no)values('{0}','{1}','{2}','{3}','{4}',{5},{6},{7},{8},{9},{10},'{11}')";
                sql = string.Format(sql, obj.machine_code, obj.year, obj.month, obj.day, obj.date, obj.run_time, obj.error_time, obj.others_time, obj.rest_time, obj.boot_time, obj.utilization_rate, obj.class_no);
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
        public int GetID(string machine_code, string class_no, string date)
        {
            try
            {
                int id = 0;
                string sql = "select * from oee.utilization_rate_class where machine_code='{0}' and date='{1}' and class_no='{2}'";
                sql = string.Format(sql, machine_code, date, class_no);
               
                UtilizationRateClass obj = PostgreHelper.GetSingleEntity<UtilizationRateClass>(sql);
                if(obj !=null)
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
        public int Update(UtilizationRateClass obj)
        {
            try
            {
                string sql = "Update oee.utilization_rate_class set run_time='{0}',error_time='{1}',others_time='{2}',rest_time='{3}',boot_time='{4}',utilization_rate='{5}' where id='{6}'";
                sql = string.Format(sql, obj.run_time, obj.error_time, obj.others_time, obj.rest_time, obj.boot_time, obj.utilization_rate, obj.id);
                
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
