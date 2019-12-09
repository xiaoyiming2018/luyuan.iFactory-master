using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;
namespace Advantech.IFactory.CommonLibrary
{
    public class StationService
    {
        /// <summary>
        /// 获取站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<station_info> SelectAll(int line_id=0)
        {
            try
            {
                List<station_info> objList = new List<station_info>();
                string sql = null;
                if (line_id > 0)
                {
                    sql = "SELECT * FROM fimp.station_info where line_id={0} order by line_id";
                    sql = string.Format(sql, line_id);
                }
                else
                {
                    sql = "SELECT * FROM fimp.station_info order by station_id";
                }
                objList=PostgreHelper.GetEntityList<station_info>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }

        /// <summary>
        /// 插入站别信息数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(station_info obj)
        {
            try
            {
                string sql = "Insert into fimp.station_info(station_name_en,station_name_cn,station_name_tw,type_no,status_no,unit_no,line_id)values('{0}','{1}','{2}','{3}','{4}','{5}',{6})";
                sql = string.Format(sql, obj.station_name_en, obj.station_name_cn, obj.station_name_tw, obj.type_no, obj.status_no, obj.unit_no,obj.line_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(station_info obj)
        {
            try
            {
                int count = PostgreHelper.UpdateSingleEntity<station_info>(obj);
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
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.station_info where station_id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询单笔数据，通过id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public station_info SelectSingle(int station_id)
        {
            try
            {
                string sql = "select * from fimp.station_info where station_id={0}";
                sql = string.Format(sql, station_id);
                station_info obj = new station_info();
                obj = PostgreHelper.GetSingleEntity<station_info>(sql);
                
                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数据，通过名称
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public station_info SelectByStaionName(string station_name)
        {
            try
            {
                string sql = "select * from fimp.station_info where station_name_en={0}";
                sql = string.Format(sql, station_name);
                station_info obj = new station_info();
                obj=PostgreHelper.GetSingleEntity<station_info>(sql);
                
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数据，通过line
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<station_info> SelectByLine(int line_id)
        {
            try
            {
                string sql = "select a.station_id,c.station_name_en,c.station_name_cn,c.station_name_tw from fimp.machine_info a, fimp.line_info b, fimp.station_info c where a.line_id=b.line_id and a.station_id = c.station_id and a.unit_no=b.unit_no and b.unit_no=c.unit_no and a.line_id={0}";
                sql = string.Format(sql, line_id);
                List<station_info> objList = new List<station_info>();
                objList = PostgreHelper.GetEntityList<station_info>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<station_info> SelectAllByUnit(string unit_no)
        {
            List<station_info> objList = new List<station_info>();
            string sql = null;
            sql = "select * from fimp.station_info where unit_no='{0}'";
            sql = string.Format(sql, unit_no);
            objList = PostgreHelper.GetEntityList<station_info>(sql);
            return objList;
        }

    }
}
