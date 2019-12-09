using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class RestTimeManager
    {
        RestTimeService RTS = new RestTimeService();
        public List<rest_time> SelectAll(string unit_no = null)
        {
            List<rest_time> objList = RTS.SelectAll(unit_no);
            return objList;
        }

        public rest_time SelectSingle(int id)
        {
            rest_time obj = RTS.SelectSingle(id);
            return obj;
        }

        public int Insert(rest_time obj)
        {
            int count = RTS.Insert(obj);
            return count;
        }

        public int Update(rest_time obj)
        {
            int count = RTS.Update(obj);
            return count;
        }

        public int Del(int id)
        {
            int count = RTS.Del(id);
            return count;
        }

        /// <summary>
        /// 更新unit_no
        /// </summary>
        /// <param name="old_unit_no">旧</param>
        /// <param name="unit_no">新</param>
        /// <returns></returns>
        public int UpdateUnitNo(string old_unit_no, string unit_no)
        {
           int count= RTS.UpdateUnitNo(old_unit_no, unit_no);
           return count;
        }

        public rest_time SelectTime(string unit_no)
        {
            rest_time obj = RTS.SelectTime(unit_no);
            return obj;
        }

        public bool SelectCount(string time, string unit_no)
        {
            bool result = RTS.SelectCount(time, unit_no);
            return result;
        }
    }
}
