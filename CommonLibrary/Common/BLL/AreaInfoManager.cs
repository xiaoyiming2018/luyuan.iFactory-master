using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class AreaInfoManager
    {
        area_infoService AIS = new area_infoService();
        public List<area_info> SelectAll()
        {
            List<area_info> objList = AIS.SelectAll();
            return objList;
        }

        public area_info SelectSingleID(int id)
        {
            area_info obj = AIS.SelectSingleID(id);
            return obj;
        }

        public int Insert(area_info obj)
        {
            int count = AIS.Insert(obj);
            return count;
        }

        public int Update(area_info obj)
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
