using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class CityInfoManager
    {
        CityInfoService CIS = new CityInfoService();
        public List<city_info> SelectAll(int area_id=0)
        {
            List<city_info> objList = CIS.SelectAll(area_id);
            return objList;
        }

        public city_info SelectSingle(int id)
        {
            city_info obj = CIS.SelectSingle(id);
            return obj;
        }

        public int Insert(city_info obj)
        {
            int count = CIS.Insert(obj);
            return count;
        }

        public int Update(city_info obj)
        {
            int count = CIS.Update(obj);
            return count;
        }

        public int Del(int id)
        {
            int count = CIS.Del(id);
            return count;
        }
    }
}
