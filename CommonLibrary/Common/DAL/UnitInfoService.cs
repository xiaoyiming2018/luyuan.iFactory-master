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
    public class UnitInfoService
    {
        //查询多笔数据,可以同过制程编码搜索
        public List<unit_info> SelectAll(string unit_no = null)
        {
            try
            {
                List<unit_info> objList = new List<unit_info>();
                string sql = null;
                if(!string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT  unit_no,unit_name_en,unit_name_tw,unit_name_cn,seq FROM fimp.unit_info where unit_no='{0}' order by seq";
                    sql = string.Format(sql, unit_no);
                }
                else
                {
                    sql = "SELECT  unit_no,unit_name_en,unit_name_tw,unit_name_cn,seq FROM fimp.unit_info  order by seq";
                }
                objList = PostgreHelper.GetEntityList<unit_info>(sql);
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据部门编码获取所有制程
        /// </summary>
        /// <param name="dept_id"></param>
        /// <returns></returns>
        public List<unit_info> SelectAll(int dept_id)
        {
            try
            {
                List<unit_info> objList = new List<unit_info>();
                string sql = null;

                sql = "SELECT  * FROM fimp.unit_info where dept_id='{0}' order by seq";
                sql = string.Format(sql, dept_id);
                objList=PostgreHelper.GetEntityList<unit_info>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //查询单笔数据，通过制程编码搜索
        public unit_info SelectSingle(string unit_no = null,int id=0)
        {
            try
            {
                unit_info obj = new unit_info();
                string sql = null;
                if (!string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT * FROM fimp.unit_info where unit_no='{0}' order by seq";
                    sql = string.Format(sql, unit_no);
                }
                else if (id>0)
                {
                    sql = "SELECT * FROM fimp.unit_info where seq={0} order by seq";
                    sql = string.Format(sql, id);
                }
                obj=PostgreHelper.GetSingleEntity<unit_info>(sql);
                return obj;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //插入数据
        public int Insert(unit_info obj)
        {
            try
            {
                int count = PostgreHelper.InsertSingleEntity<unit_info>(obj);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(unit_info obj)
        {
            try
            {
                string sql = "Update fimp.unit_info set unit_no='{0}',unit_name_en='{1}',unit_name_tw='{2}',unit_name_cn='{3}',dept_id='{4}' where seq={5}";
                sql = string.Format(sql, obj.unit_no, obj.unit_name_en, obj.unit_name_tw, obj.unit_name_cn, obj.dept_id, obj.seq);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.unit_info where seq={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
