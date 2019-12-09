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
    public class PlantInfoService
    {
        //查询多笔数据，可通过城市id查询
        public List<plant_info> SelectAll(int city_id=0)
        {
            try
            {
                List<plant_info> objList = new List<plant_info>();
                string sql = null;
                if(city_id>0)
                {
                    sql = "SELECT  * FROM fimp.plant_info where city_id='{0}' order by city_id,plant_id";
                    sql = string.Format(sql, city_id);
                }
                else
                {
                    sql = "SELECT * FROM fimp.plant_info  order by city_id,plant_id";
                }
                objList = PostgreHelper.GetEntityList<plant_info>(sql);
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //查询单笔数据，通过厂区id
        public plant_info SelectSingle(int id)
        {
            try
            {
                plant_info obj = new plant_info();
                string sql = "SELECT * FROM fimp.plant_info where plant_id='{0}' order by city_id,plant_id";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<plant_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //插入数据
        public int Insert(plant_info obj)
        {
            try
            {
                string sql = "insert into fimp.plant_info(city_id,plant_name_en,plant_name_tw,plant_name_cn)values({0},'{1}',N'{2}',N'{3}')";
                sql = string.Format(sql, obj.city_id, obj.plant_name_en, obj.plant_name_tw, obj.plant_name_cn);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(plant_info obj)
        {
            try
            {
                string sql = "Update fimp.plant_info set city_id={0},plant_name_en='{1}',plant_name_tw=N'{2}',plant_name_cn=N'{3}' where plant_id={4}";
                sql = string.Format(sql, obj.city_id, obj.plant_name_en, obj.plant_name_tw, obj.plant_name_cn, obj.plant_id);
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
                string sql = "delete from fimp.plant_info where plant_id={0}";
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
