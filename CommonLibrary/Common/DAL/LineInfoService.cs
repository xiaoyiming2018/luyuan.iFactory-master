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
    public class LineInfoService
    {
        //查询所有
        public List<line_info> SelectAll(int plant_id=0,string unit_no = null,int line_id=0)
        {
            try
            {
                List<line_info> objList = new List<line_info>();
                string sql = null;
                if (plant_id > 0 && !string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info where plant_id={0} and unit_no='{1}' order by plant_id,unit_no";
                    sql = string.Format(sql, plant_id, unit_no);
                }
                else if (plant_id <= 0 && !string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info where  unit_no='{0}' order by plant_id,unit_no";
                    sql = string.Format(sql, unit_no);
                }
                else if (plant_id > 0 && string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info where plant_id={0} order by plant_id,unit_no";
                    sql = string.Format(sql, plant_id);
                } else if (line_id >0 ) {
                    sql = "SELECT  *  FROM fimp.line_info where line_id={0} order by line_id";
                    sql = string.Format(sql, line_id);
                }
                else
                {
                    sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info  order by plant_id,unit_no";
                }

                objList = PostgreHelper.GetEntityList<line_info>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       
        //查询单笔数据，通过id
        public line_info SelectSingle(int id)
        {
            try
            {
                line_info obj = new line_info();
                string sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info where line_id='{0}'  order by plant_id,unit_no";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<line_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过名称查询单笔数据
        /// </summary>
        /// <param name="line_name_en"></param>
        /// <returns></returns>
        public line_info SelectByName(string line_name_en)
        {
            try
            {
                line_info obj = new line_info();
                string sql = "SELECT  plant_id,unit_no,line_id,line_name_en,line_name_tw,line_name_cn,story,status_no FROM fimp.line_info where line_name_en='{0}'  order by plant_id,unit_no";
                sql = string.Format(sql, line_name_en);
                obj = PostgreHelper.GetSingleEntity<line_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //插入数据

        public int Insert(line_info obj)
        {
            try
            {
                //string sql = "Insert into fimp.line_info(plant_id,dept_id,unit_no,line_name_en,line_name_tw,line_name_cn,story,status_no)values({0},-1,'{1}','{2}',N'{3}',N'{4}',N'{5}','{6}')";
                //sql = string.Format(sql, obj.plant_id, obj.unit_no, obj.line_name_en, obj.line_name_tw, obj.line_name_cn, obj.story, obj.status_no);
                int count =PostgreHelper.InsertSingleEntity<line_info>(obj);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(line_info obj)
        {
            try
            {
                string sql = "Update fimp.line_info set plant_id='{0}',unit_no='{1}',line_name_en='{2}',line_name_tw=N'{3}',line_name_cn=N'{4}',story=N'{5}',status_no='{6}' where line_id={7}";
                sql = string.Format(sql, obj.plant_id, obj.unit_no, obj.line_name_en, obj.line_name_tw, obj.line_name_cn, obj.story, obj.status_no,obj.line_id);
                int count =PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = "delete from fimp.line_info where line_id={0}";
                sql = string.Format(sql, id);
                int count =PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //更新线别状态
        public int UpdateStatus(int line_id,string status_no)
        {
            try
            {
                string sql = "Update fimp.line_info set status_no='{0}' where line_id={1}";
                sql = string.Format(sql, status_no, line_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //查询单笔数据，通过名称
        public line_info SelectSingleByName(string line_name)
        {
            string sql = "SELECT  *FROM fimp.line_info where line_name_en='{0}'";
            sql = string.Format(sql, line_name);
            return PostgreHelper.GetSingleEntity<line_info>(sql);
        }
    }
}
