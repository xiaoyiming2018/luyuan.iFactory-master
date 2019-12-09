using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class LineBalanceRateManager
    {
        LineBalanceRateService LBRS = new LineBalanceRateService();

        public int Insert(LineBalanceRate obj)
        {
            int count = LBRS.Insert(obj);
            return count;
        }

        public int Update(LineBalanceRate obj)
        {
            int count = LBRS.Update(obj);
            return count;
        }

        public List<LineBalanceRate> SelectAll()
        {
            List<LineBalanceRate> objList = LBRS.SelectAll();
            return objList;
        }
        public LineBalanceRate SelectSingle(int line_id, string date_time = null)
        {
            LineBalanceRate obj = LBRS.SelectSingle(line_id, date_time: date_time);
            return obj;
        }
        public LineBalanceRate SelectSingle(int line_id,string work_order, string date_time = null)
        {
            LineBalanceRate obj = LBRS.SelectSingle(line_id, work_order, date_time: date_time);
            return obj;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int DelByTime(DateTime delTime)
        {
            int count = LBRS.DeleteByTime(delTime);
            return count;
        }
    }
}
