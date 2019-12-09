using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonHelper
{
    public class DateTimeHelper
    {
        /// <summary>
        /// 获取当天日期，当天开始时间，当天结束时间
        /// </summary>
        /// <param name="insert_time">当前时刻点</param>
        /// <param name="SetTime">设定的一天开始计算点</param>
        /// <returns></returns>
        public static DaysData GetDateTimeData(DateTime insert_time,string SetTime)
        {
            try
            {
                DaysData daysData = new DaysData();
                string setTime = null;
                try
                {
                    setTime = SetTime;
                }
                catch
                {
                    setTime = "00:00:00";
                }

                string sysTime = insert_time.ToString("HH:mm:ss");
                daysData.RawDate = insert_time;
                daysData.LongTimeString = insert_time.ToString("yyyy-MM-dd HH:mm:ss");
                if (Convert.ToDateTime(sysTime) >= Convert.ToDateTime(setTime))
                {
                    string date = insert_time.ToString("yyyy-MM-dd");
                    string stratTime = insert_time.ToString("yyyy-MM-dd") + " " + setTime;
                    string endTime = insert_time.AddDays(1).ToString("yyyy-MM-dd") + " " + setTime;
                    daysData.TodayDate = date;
                    daysData.TodayDateStartTime = stratTime;
                    daysData.TodayDateEndTime = endTime;
                }
                else
                {
                    string date = insert_time.AddDays(-1).ToString("yyyy-MM-dd");
                    string stratTime = insert_time.AddDays(-1).ToString("yyyy-MM-dd") + " " + setTime;
                    string endTime = insert_time.ToString("yyyy-MM-dd") + " " + setTime;
                    daysData.TodayDate = date;
                    daysData.TodayDateStartTime = stratTime;
                    daysData.TodayDateEndTime = endTime;
                }
                return daysData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    /// <summary>
    /// 日期数据
    /// </summary>
    public class DaysData
    {
        /// <summary>
        /// 当天日期YYYY-mm-DD
        /// </summary>
        public string TodayDate { set; get; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string TodayDateStartTime { set; get; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string TodayDateEndTime { set; get; }
        /// <summary>
        /// yyyy-MM-dd HH:mm:ss 格式的长日期格式
        /// </summary>
        public string LongTimeString { set; get; }
        /// <summary>
        /// 原始未转换的日期格式
        /// </summary>
        public DateTime RawDate { set; get; }
    }
}
