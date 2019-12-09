using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class LineInfoManager
    {
        LineInfoService LIS = new LineInfoService();

        public List<line_info> SelectAll(int plant_id=0,string unit_no = null,int line_id=0)
        {
            List<line_info> objList = LIS.SelectAll(plant_id: plant_id, unit_no: unit_no,line_id:line_id);
            return objList;
        }
      
        public line_info SelectSingle(int id)
        {
            line_info obj = LIS.SelectSingle(id);
            return obj;
        }
        public int Insert(line_info obj)
        {
            int count = LIS.Insert(obj);
            return count;
        }
        public int Update(line_info obj)
        {
            int count = LIS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = LIS.Del(id);
            return count;
        }
        public int UpdateStatus(int line_id,string status_no)
        {
            int count = LIS.UpdateStatus(line_id, status_no);
            return count;
        }
        /// <summary>
        /// 通过名称查询单笔数据
        /// </summary>
        /// <param name="line_name_en"></param>
        /// <returns></returns>
        public line_info SelectByName(string line_name_en)
        {
            return LIS.SelectByName(line_name_en);
        }
    }
}
