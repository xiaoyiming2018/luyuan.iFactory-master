
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class material_categoryManager
    {
        materialcategoryService AIS = new materialcategoryService();
        public List<material_category> SelectAll( int line_id=0, string unit_no = null)
        {
            List<material_category> objList = AIS.SelectAll(line_id, unit_no);
            return objList;
        }

        public material_category SelectSingleID(int id)
        {
            material_category obj = AIS.SelectSingleID(id);
            return obj;
        }

        public int Insert(material_category obj)
        {
            int count = AIS.Insert(obj);
            return count;
        }

        public int Update(material_category obj)
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
