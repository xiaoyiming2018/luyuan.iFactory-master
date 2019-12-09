
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class material_infoManager
    {
        MaterialInfoService AIS = new MaterialInfoService();
        public List<material_info> SelectAll()
        {
            List<material_info> objList = AIS.SelectAllcol();
            return objList;
        }

        public material_info SelectSingleID(int id)
        {
            material_info obj = AIS.SelectSingleByID(id);
            return obj;
        }

        public int Insert(material_info obj)
        {
            int count = AIS.Insert(obj);
            return count;
        }

        public int Update(material_info obj)
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
