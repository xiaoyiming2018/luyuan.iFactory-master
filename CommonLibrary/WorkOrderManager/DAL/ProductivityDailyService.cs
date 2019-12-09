using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public  class ProductivityDailyService
    {
        /// <summary>
        /// 返回插入的id
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Insert(Productivity_daily Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<Productivity_daily>(Obj);
            return id;
        }

        /// <summary>
        /// 加载指定记录
        /// </summary>
        /// <param name="line">id</param>
        /// <returns></returns>
        public Productivity_daily Getproductivity(int line,int year,int month,int day)
        {
            Productivity_daily logItem = new Productivity_daily();
            string command = "Select * from fimp.Productivity_daily where line_id={0} and year={1} and month={2} and day={3} limit 1";
            command = string.Format(command, line, year, month,day);

            logItem = PostgreHelper.GetSingleEntity<Productivity_daily>(command);

            return logItem;
        }
        /// <summary>
        /// 更新error_log对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Update(Productivity_daily Obj)
        {
            int id = PostgreHelper.UpdateSingleEntity<Productivity_daily>(Obj);
            return id;
        }

    }
}
