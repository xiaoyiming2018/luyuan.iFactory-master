using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class PlantInfoManager
    {
        PlantInfoService PIS = new PlantInfoService();
        public List<plant_info> SelectAll(int city_id=0)
        {
            List<plant_info> objList = PIS.SelectAll(city_id);
            return objList;
        }

        public plant_info SelectSingle(int id)
        {
            plant_info obj = PIS.SelectSingle(id);
            return obj;
        }

        public int Insert(plant_info obj)
        {
            int count = PIS.Insert(obj);
            return count;
        }
        public int Update(plant_info obj)
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
