using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class MachineworkingtimeManager
    {
        MachineworkingtimeService AIS = new MachineworkingtimeService();
        public List<machine_working_time> SelectAll(machine_working_time obj, string unit_no = null)
        {
            List<machine_working_time> objList = AIS.SelectAll(obj,unit_no);
            return objList;
        }

        public machine_working_time SelectSingleID(int id)
        {
            machine_working_time obj = AIS.SelectSingleID(id);
            return obj;
        }
        /// <summary>
        /// 根据设备或者站位标识去查找
        /// </summary>
        /// <param name="MachineId"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public machine_working_time SelectSingle(int MachineId = -1, int StationId = -1)
        {
            machine_working_time obj = AIS.SelectSingle(MachineId, StationId);
            return obj;
        }
        public int Insert(machine_working_time obj)
        {
            int count = AIS.Insert(obj);
            return count;
        }

        public int Update(machine_working_time obj)
        {
            int count = AIS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = AIS.Del(id);
            return count;
        }
    }
}
