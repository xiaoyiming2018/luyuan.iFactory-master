using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class DeptInfoManager
    {
        DeptInfoService PIS = new DeptInfoService();
        public List<dept_info> SelectAll(int plant_id = 0)
        {
            List<dept_info> objList = PIS.SelectAll(plant_id);
            return objList;
        }
        public List<dept_info> SelectAllid(int id)
        {
            List<dept_info> objList = PIS.SelectAllid(id);
            return objList;
        }
        public dept_info SelectSingle(int id)
        {
            dept_info obj = PIS.SelectSingle(id);
            return obj;
        }

        public int Insert(dept_info obj)
        {
            int count = PIS.Insert(obj);
            return count;
        }
        public int Update(dept_info obj)
        {
            int count = PIS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = PIS.Del(id);
            return count;
        }
    }
}
