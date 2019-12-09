using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class area_infoService   
    {
        
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(area_info obj)
        { 
            try
            {
                string sql = "insert into fimp.area_info(area_name_en,area_name_tw,area_name_cn,time_zone )values('{0}','{1}','{2}',{3})";
                sql = string.Format(sql, obj.area_name_en, obj.area_name_tw, obj.area_name_cn, obj.time_zone);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<area_info> SelectAll()
        {
            try
            {
                List<area_info> objList = new List<area_info>();
                string sql = null;
                sql = "select * from fimp.area_info order by area_id";
                objList=PostgreHelper.GetEntityList<area_info>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
      

        public area_info SelectSingleID(int area_id)
        {
            try
            {
                area_info obj = new area_info();
                string sql = "select * from fimp.area_info where area_id={0}";
                sql = string.Format(sql, area_id);
                obj=PostgreHelper.GetSingleEntity<area_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(area_info obj)
        {
            try
            {
                int count = 0;
                string sql = "update fimp.area_info set area_name_en='{0}',area_name_tw='{1}',area_name_cn='{2}',time_zone='{3}' where area_id={4}";
                sql = string.Format(sql, obj.area_name_en, obj.area_name_tw, obj.area_name_cn, obj.time_zone,obj.area_id);
                count= PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

     

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int area_id)
        {
            try
            {
                string sql = "delete from fimp.area_info where area_id={0}";
                sql = string.Format(sql, area_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       
      
    }
}
