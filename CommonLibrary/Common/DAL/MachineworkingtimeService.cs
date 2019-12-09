using System;
using System.Collections.Generic;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineworkingtimeService   
    {
        
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(machine_working_time obj)
        { 
            try
            {
                string sql = "insert into fimp.machine_working_time(machine_id,plant_id,unit_no,line_id,station_id,part_num,standard_time )values({0},{1},'{2}',{3},{4},'{5}',{6})";
                sql = string.Format(sql, obj.machine_id, obj.plant_id, obj.unit_no, obj.line_id,obj.station_id,obj.part_num,obj.standard_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 查询所有信息
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<machine_working_time> SelectAll(machine_working_time obj, string unit_no = null)
        {
            try
            {
                List<machine_working_time> objList = new List<machine_working_time>();
                string sql = null;

                if (!string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT * FROM fimp.machine_working_time where unit_no='{0}'  order by unit_no";
                    sql = string.Format(sql, unit_no);
                }
                else
                {
                    if (obj.plant_id > 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                    {
                        sql = "SELECT * FROM fimp.machine_working_time where plant_id={0} order by plant_id";
                        sql = string.Format(sql, obj.plant_id);
                    }
                    else if (obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                    {
                        sql = "SELECT * FROM fimp.machine_working_time where and plant_id={0} and unit_no='{1}' order by plant_id,unit_no";
                        sql = string.Format(sql, obj.plant_id, obj.unit_no);
                    }
                    else if (obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id > 0)
                    {
                        sql = "SELECT * FROM fimp.machine_working_time where plant_id={0} and unit_no='{1}' and line_id={2} order by plant_id,unit_no,line_id";
                        sql = string.Format(sql, obj.plant_id, obj.unit_no, obj.line_id);
                    }
                    else
                    {
                        sql = "SELECT * FROM fimp.machine_working_time  order by id";
                    }
                }
                objList = PostgreHelper.GetEntityList<machine_working_time>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
      

        public machine_working_time SelectSingleID(int id)
        {
            try
            {
                machine_working_time obj = new machine_working_time();
                string sql = "select * from fimp.machine_working_time where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<machine_working_time>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据设备或者站位标识去查找
        /// </summary>
        /// <param name="MachineId"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public machine_working_time SelectSingle(int MachineId=-1,int StationId=-1)
        {
            try
            {
                string sql; 
                machine_working_time obj = new machine_working_time();
                if(MachineId>0)
                {
                    sql = string.Format("select * from fimp.machine_working_time where machine_id={0}", MachineId);
                }
                else
                {
                    sql = string.Format("select * from fimp.machine_working_time where station_id={0}", StationId);
                }

                obj = PostgreHelper.GetSingleEntity<machine_working_time>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据机种和设备id或者站位id去查找
        /// </summary>
        /// <param name="MachineId"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public machine_working_time SelectTime(string part_num,int MachineId = -1, int StationId = -1)
        {
            try
            {
                string sql;
                machine_working_time obj = new machine_working_time();
                if (MachineId > 0)
                {
                    sql = string.Format("select * from fimp.machine_working_time where machine_id={0} and part_num={1}", MachineId, part_num);
                }
                else
                {
                    sql = string.Format("select * from fimp.machine_working_time where station_id={0} and part_num={1}", StationId, part_num);
                }

                obj = PostgreHelper.GetSingleEntity<machine_working_time>(sql);
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
        public int Update(machine_working_time obj)
        {
            try
            {
                int count = 0;
                string sql = "update fimp.machine_working_time set machine_id={0},plant_id={1},unit_no='{2}',line_id={3},station_id={4},part_num='{5}',standard_time={6} where id={7}";
                sql = string.Format(sql, obj.machine_id, obj.plant_id, obj.unit_no, obj.line_id, obj.station_id,obj.part_num,obj.standard_time,obj.id);
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
                string sql = "delete from  fimp.machine_working_time where id={0}";
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
