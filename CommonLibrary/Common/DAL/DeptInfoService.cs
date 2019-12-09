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
    public class DeptInfoService
    {
        //查询多笔数据，可通过城市id查询
        public List<dept_info> SelectAll(int plant_id = 0)
        {
            try
            {
                List<dept_info> objList = new List<dept_info>();
                string sql = null;
                if(plant_id > 0)
                {
                    sql = "SELECT  * FROM fimp.dept_info where plant_id={0} order by plant_id";
                    sql = string.Format(sql, plant_id);
                }
                else
                {
                    sql = "SELECT  * FROM fimp.dept_info  order by plant_id";
                }
                objList = PostgreHelper.GetEntityList<dept_info>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        public List<dept_info> SelectAllid(int id)
        {
            try
            {
                List<dept_info> objList = new List<dept_info>();
                string sql = null;
                if (id > 0)
                {
                    sql = "SELECT  * FROM fimp.dept_info where dept_id={0} order by dept_id";
                    sql = string.Format(sql, id);
                }
                else
                {
                    sql = "SELECT  * FROM fimp.dept_info  order by plant_id";
                }

                objList = PostgreHelper.GetEntityList<dept_info>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //查询单笔数据，通过厂区id
        public dept_info SelectSingle(int id)
        {
            try
            {
                dept_info obj = new dept_info();
                string sql = "SELECT  * FROM fimp.dept_info where dept_id='{0}' order by dept_id";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<dept_info>(sql) ;
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //插入数据
        public int Insert(dept_info obj)
        {
            try
            {
                string sql = "insert into fimp.dept_info(plant_id,dept_no,dept_name_en,dept_name_cn,dept_name_tw)values({0},N'{1}','{2}',N'{3}',N'{4}')";
                sql = string.Format(sql, obj.plant_id, obj.dept_no, obj.dept_name_en, obj.dept_name_cn,obj.dept_name_tw);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(dept_info obj)
        {
            try
            {
                string sql = "Update fimp.dept_info set plant_id={0},dept_no='{1}',dept_name_en='{2}',dept_name_cn=N'{3}', dept_name_tw=N'{4}'where dept_id={5}";
                sql = string.Format(sql, obj.plant_id, obj.dept_no, obj.dept_name_en, obj.dept_name_cn, obj.dept_name_tw, obj.dept_id);
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
                string sql = "delete from fimp.dept_info where dept_id={0}";
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
