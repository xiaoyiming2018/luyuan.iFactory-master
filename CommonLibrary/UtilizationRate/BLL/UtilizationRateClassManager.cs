using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateClassManager
    {
        UtilizationRateClassService URCS = new UtilizationRateClassService();
        TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        TagTimeClassManager TagTimeClassManager = new TagTimeClassManager();
        DataReplaceManager dataReplaceManager = new DataReplaceManager();
        FormulaManager formulaManager = new FormulaManager();

        public List<UtilizationRateClass> SelectAll(string machine_code = null, string startDate = null, string endDate = null, string unit_no = null, string class_no = null)
        {
            List<UtilizationRateClass> objList = URCS.SelectAll(machine_code, startDate, endDate, unit_no, class_no);
            return objList;
        }

        public int Insert(UtilizationRateClass obj)
        {
            int count = URCS.Insert(obj);
            return count;
        }

        public int GetID(string machine_code, string class_no, string date)
        {
            int id = URCS.GetID(machine_code, class_no, date);
            return id;
        }
        public int Update(UtilizationRateClass obj)
        {
            int count = URCS.Update(obj);
            return count;
        }
       
        /// <summary>
        /// 更新稼动率（班）
        /// </summary>
        /// <param name="machine_code">设备编码</param>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <param name="date">年-月-日</param>
        /// <param name="class_no">班别代码</param>
        /// <param name="run_time">运行时间</param>
        /// <param name="error_time">异常时间</param>
        /// <param name="others_time">其他时间</param>
        /// <param name="rest_time">休息时间</param>
        /// <param name="boot_time">开机时间</param>
        /// <param name="utilization_rate">稼动率</param>
        public void InsertUtilizationRateClass(string machine_code, int year, int month, int day, string date, string class_no, double run_time, double error_time, double others_time, double rest_time, double boot_time, double utilization_rate)
        {
            try
            {
                UtilizationRateClass obj = new UtilizationRateClass();
                obj.machine_code = machine_code;
                obj.year = year;
                obj.month = month;
                obj.day = day;
                obj.date = date;
                obj.class_no = class_no;
                obj.run_time = run_time;
                obj.error_time = error_time;
                obj.others_time = others_time;
                obj.rest_time = rest_time;
                obj.boot_time = boot_time;
                obj.utilization_rate = utilization_rate;
                int id = GetID(obj.machine_code, obj.class_no, obj.date);
                if (id > 0)
                {
                    obj.id = id;
                    int count=Update(obj);
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
