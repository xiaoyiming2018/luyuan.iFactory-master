using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MaterialInfoManager
    {
        MaterialInfoService MIS = new MaterialInfoService();
        public List<material_info> SelectAll(string material_name)
        {
            List<material_info> objList = MIS.SelectAll(material_name);
            return objList;
        }

        public material_info SelectSingle(int id)
        {
            material_info objList = MIS.SelectSingleByID(id);
            return objList;
        }

        public int Update(material_info obj)
        {
            int count = MIS.Update(obj);
            return count;
        }
        public int Insert(material_info obj)
        {
            int count = MIS.Insert(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = MIS.Del(id);
            return count;
        }



    }
}
