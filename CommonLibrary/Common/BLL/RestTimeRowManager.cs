using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class RestTimeRowManager
    {
        RestTimeRowService RTRS = new RestTimeRowService();
        public List<rest_time_row> SelectAll(string start_time = null,string end_time = null)
        {
            List<rest_time_row> objList = RTRS.SelectAll(start_time, end_time);
            return objList;
        }
        public int Insert(rest_time_row obj)
        {
            int count = RTRS.Insert(obj);
            return count;
        }
        public int Update(rest_time_row obj)
        {
            int count = RTRS.Update(obj);
            return count;
        }

        public int Del(int id,string machine_code = null)
        {
            int count = RTRS.Del(id, machine_code);
            return count;
        }
        public rest_time_row SelectSingle(string machine_code,string ts)
        {
            rest_time_row obj = RTRS.SelectSingle(machine_code, ts);
            return obj;
        }
        public rest_time_row IsUpdate(string machine_code,string start_time,string end_time)
        {
            rest_time_row obj = RTRS.IsUpdate(machine_code, start_time, end_time);
            return obj;
        }
    }
}
