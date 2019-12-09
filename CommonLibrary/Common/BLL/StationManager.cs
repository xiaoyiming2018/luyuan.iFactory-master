using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Advantech.IFactory.CommonLibrary
{
    public class StationManager
    {
        StationService SS = new StationService();
        LineInfoService LIS = new LineInfoService();
        /// <summary>
        /// 获取站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<station_info> SelectAll(int  line_id=0)
        {
            List<station_info> objList = SS.SelectAll(line_id);
            return objList;
        }

        /// <summary>
        /// 插入站别信息数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(station_info obj)
         {
            int count = SS.Insert(obj);
            return count;
         }

         /// <summary>
         /// 更新站别信息
         /// </summary>
         /// <param name="obj"></param>
         /// <returns></returns>
         public int Update(station_info obj)
         {
             int count = SS.Update(obj);
             return count;
         }

         /// <summary>
         /// 删除数据
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public int Del(int id)
         {
             int count = SS.Del(id);
             return count;
         }

         /// <summary>
         /// 查询单笔数据，通过id或station_code
         /// </summary>
         /// <param name="id"></param>
         /// <returns></returns>
         public station_info SelectSingle(int station_id = 0)
         {
             station_info obj = new station_info();
             if (station_id > 0)
             {
                 obj = SS.SelectSingle(station_id);
             }
             
             else
             {
                 obj = null;
             }
             return obj;
         }
        /// <summary>
        /// 查询单笔数据，通过名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public station_info SelectByStaionName(string station_name)
        {
            return SS.SelectByStaionName(station_name);
        }

        public List<station_info> SelectByLine(int line_id)
        {
            List<station_info> objList = SS.SelectByLine(line_id);
            return objList;
        }
        /// <summary>
        /// 根据unit_no获取站别信息
        /// </summary>
        /// <param name="制程"></param>
        /// <returns></returns>
        public List<station_info> SelectAllByUnit(string unit_no)
        {
            List<station_info> objList = SS.SelectAllByUnit(unit_no);
            return objList;
        }
    }
}
