using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
   public class ProductivityDailyManager
    {
        ProductivityDailyService daily = new ProductivityDailyService();
        public int Insert(Productivity_daily Obj)
        {
            int flag = daily.Insert(Obj);
            return flag;
        }
        /// <summary>
        /// 根据时间条件加载产能数据
        /// </summary>
        /// <param name="line"></param>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public Productivity_daily Getproductivity(int line, int year, int month, int day)
        {
            Productivity_daily logItem = new Productivity_daily();
            logItem = daily.Getproductivity(line,year,month,day);
            return logItem;
        }

        public int Update(Productivity_daily Obj)
        {
            int flag = daily.Update(Obj);
            return flag;
        }
    }
}
