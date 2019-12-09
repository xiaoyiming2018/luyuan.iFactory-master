using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;
namespace Advantech.IFactory.CommonLibrary
{
    public class CityInfoService
    {
        //获取城市列表(可通过区域id查询)
        public List<city_info> SelectAll(int area_id=0)
        {
            try
            {
                List<city_info> objList = new List<city_info>();
                string sql = null;
                if (area_id > 0)
                {
                    sql = "SELECT * FROM fimp.city_info where area_id={0} order by area_id,city_id";
                    sql = string.Format(sql, area_id);
                }
                else
                {
                    sql = "SELECT * FROM fimp.city_info  order by area_id,city_id";
                }
                objList = PostgreHelper.GetEntityList<city_info>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //查询单笔数据
        public city_info SelectSingle(int id)
        {
            try
            {
                city_info obj = new city_info();
                string sql = "SELECT * FROM fimp.city_info where city_id={0} order by area_id,city_id";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<city_info>(sql);
                return obj;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //插入数据
        public int Insert(city_info obj)
        {
            try
            {
                string sql = "insert into fimp.city_info(area_id,city_name_en,city_name_tw,city_name_cn)values({0},'{1}',N'{2}',N'{3}')";
                sql = string.Format(sql, obj.area_id, obj.city_name_en, obj.city_name_tw, obj.city_name_cn);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(city_info obj)
        {
            try
            {
                string sql = "Update fimp.city_info set area_id={0},city_name_en='{1}',city_name_tw=N'{2}',city_name_cn=N'{3}' where city_id={4}";
                sql = string.Format(sql, obj.area_id, obj.city_name_en, obj.city_name_tw, obj.city_name_cn, obj.city_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.city_info where city_id={0}";
                sql = string.Format(sql, id);
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
