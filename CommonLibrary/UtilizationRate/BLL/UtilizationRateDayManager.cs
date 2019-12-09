using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateDayManager
    {
        UtilizationRateDayService URDS = new UtilizationRateDayService();

        public List<UtilizationRateDay> SelectAll(string machine_code = null, string startDate = null, string endDate = null, string unit_no = null)
        {
            //if (string.IsNullOrEmpty(startDate) || string.IsNullOrEmpty(endDate))
            //{
            //    startDate = System.DateTime.Now.ToString("yyyy-MM-dd");
            //    endDate = startDate;
            //}
            List<UtilizationRateDay> objList = URDS.SelectAll(machine_code, startDate, endDate, unit_no);
            return objList;
        }
        public int Insert(UtilizationRateDay obj)
        {
            int count = URDS.Insert(obj);
            return count;
        }
        public int GetID(string machine_code, string date)
        {
            int id = URDS.GetID(machine_code, date);
            return id;
        }
        public int Update(UtilizationRateDay obj)
        {
            int count = URDS.Update(obj);
            return count;
        }
        public UtilizationRateDay SelectSingle(string machine_code, string date)
        {
            UtilizationRateDay obj = URDS.SelectSingle(machine_code, date);
            return obj;
        }

        /// <summary>
        /// 更新稼动率
        /// </summary>
        /// <param name="machine_code">设备编码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="date">年https://www.baidu.com/?tn=98012088_5_dg&ch=16月-日</param>
        /// <param name="run_time">运行时间</param>
        /// <param name="error_time">异常时间</param>
        /// <param name="others_time">其他时间</param>
        /// <param name="rest_time">休息时间</param>
        /// <param name="boot_time">开机时间</param>
        /// <param name="utilization_rate">稼动率</param>
        public void InsertUtilizationRate(string machine_code, int year, int month, int day, string date, double run_time, double error_time, double others_time, double rest_time, double boot_time, double utilization_rate)
        {
            try
            {
                UtilizationRateDay obj = new UtilizationRateDay();
                obj.machine_code = machine_code;
                obj.year = year;
                obj.month = month;
                obj.day = day;
                obj.date = date;
                obj.run_time = run_time;
                obj.error_time = error_time;
                obj.others_time = others_time;
                obj.rest_time = rest_time;
                obj.boot_time = boot_time;
                obj.utilization_rate = utilization_rate;
                int id = GetID(obj.machine_code, obj.date);
                if (id > 0)
                {
                    obj.id = id;
                    Update(obj);
                }
                else
                {
                    Insert(obj);
                }
            }
            catch (Exception ex)
            {
                string dateone = System.DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd");
                //SM.WriteLog(dateone, ex);
            }
        }
    }
}
