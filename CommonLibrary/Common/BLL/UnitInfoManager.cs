using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class UnitInfoManager
    {
        UnitInfoService UIS = new UnitInfoService();
        public List<unit_info> SelectAll(string unit_no = null)
        {
            List<unit_info> objList = UIS.SelectAll(unit_no);
            return objList;
        }
        /// <summary>
        /// 根据部门的id查询所有的制程信息
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public List<unit_info> SelectAll(int dept_id)
        {
            List<unit_info> objList = UIS.SelectAll(dept_id);
            return objList;
        }
        public unit_info SelectSingle(string unit_no = null,int id=0)
        {
            unit_info obj = UIS.SelectSingle(unit_no: unit_no, id: id);
            return obj;
        }
        public int Insert(unit_info obj)
        {
            int count = UIS.Insert(obj);
            return count;
        }
        public int Update(unit_info obj)
        {
            int count = UIS.Update(obj);
            return count;
        }
        public int Del(int id)
        {
            int count = UIS.Del(id);
            return count;
        }
    }
}
