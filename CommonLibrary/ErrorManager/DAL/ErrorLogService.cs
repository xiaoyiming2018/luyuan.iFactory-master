using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorLogService
    {
        /// <summary>
        /// 返回插入的id
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Insert(error_log Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<error_log>(Obj);
            return id;
        }
        /// <summary>
        /// 加载指定的异常记录
        /// </summary>
        /// <param name="MachineCode">设备编码</param>
        /// <returns></returns>
        public error_log GetErrorLogById(int id)
        {
            error_log logItem = new error_log();
            string command = string.Format("Select * from andon.error_log where id='{0}'", id);

            logItem = PostgreHelper.GetSingleEntity<error_log>(command);

            return logItem;
        }
        /// <summary>
        /// 加载所有未完成的异常记录
        /// </summary>
        /// <param name="device_code">装置编码</param>
        /// <returns></returns>
        public List<error_log> GetAllUnfinishedByDeviceCode(string device_code)
        {
            List<error_log> list = new List<error_log>();
            string command = "";
            if (device_code != null)
            {
                 command = "Select * from andon.error_log where machine_code='{0}' and (release_time<='{1}' OR release_time is NULL)";
                 command = string.Format(command, device_code, "2001-01-01 00:00:00");
            }
            else
            {
                 command = "Select * from andon.error_log where release_time<='{0}' or release_time is null";
                 command = string.Format(command, "2001-01-01 00:00:00");
            }
            
            list= PostgreHelper.GetEntityList<error_log>(command);

            return list;
        }
        /// <summary>
        /// 加载所有未维护过的异常记录
        /// </summary>
        /// <param name="device_code">设备编码</param>
        /// <returns></returns>
        public List<error_log> GetUnMaintainByDeviceCode(string device_code = null)
        {
            List<error_log> list = new List<error_log>();
            string command = "";
            if (device_code != null)
            {
                command = string.Format("Select * from andon.error_log where machine_code='{0}' and maintenance_time<='2001-01-01' order by start_time desc", device_code);
            }
            else
            {
                command = string.Format("Select * from andon.error_log where maintenance_time<='2001-01-01' order by start_time desc");
            }

            list = PostgreHelper.GetEntityList<error_log>(command);

            return list;
        }
        /// <summary>
        /// 加载所有未完成的异常记录
        /// </summary>
        /// <param name="device_id">装置id</param>
        /// <returns></returns>
        public List<error_log> GetAllUnfinishedByDeviceId(int device_id)
        {
            List<error_log> list = new List<error_log>();
            string command;
            if (device_id >= 0)
            {
                command = "Select * from andon.error_log where station_id='{0}' and (release_time<='{1}' OR release_time is NULL)";
                command = string.Format(command, device_id, "2001-01-01 00:00:00");
            }
            else
            {
                command = "Select * from andon.error_log where release_time<='{0}' OR release_time is NULL";
                command = string.Format(command, "2001-01-01 00:00:00");
            }
            
            list = PostgreHelper.GetEntityList<error_log>(command);

            return list;
        }
        /// <summary>
        /// 加载指定的异常记录
        /// </summary>
        /// <param name="StationId">站位id</param>
        /// <returns></returns>
        public error_log GetUnAckLogByDeviceId(int device_id, string system_tag_code)
        {
            error_log obj = new error_log();
            string command;

            command = "Select * from andon.error_log where station_id='{0}' and system_tag_code='{1}' and (release_time<='{2}' OR release_time is NULL) limit 1";
            command = string.Format(command, device_id, system_tag_code, "2001-01-01 00:00:00");

            obj = PostgreHelper.GetSingleEntity<error_log>(command);

            return obj;
        }

        /// <summary>
        /// 更新error_log对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Update(error_log Obj)
        {
            int id = PostgreHelper.UpdateSingleEntity<error_log>(Obj);
            return id;
        }
        /// <summary>
        /// 更新error_log对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Update(error_log Obj,Dictionary<string, object> UpdateDic)
        {
            int id = PostgreHelper.UpdateSingleEntity<error_log>("andon.error_log", Obj.id, UpdateDic);
            return id;
        }
        public int Del(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_log where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 检查是否已经存在异常
        /// </summary>
        /// <param name="SysTagCode"></param>
        /// <param name="MachineCode"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public bool CheckExist(String SysTagCode,string MachineCode="",int StationId=-1)
        {
            string sql;
            if(StationId>0)
            {
                sql = "select * from andon.error_log where station_id={0} and system_tag_code='{1}' and (release_time<='2001-01-01 00:00:00' or release_time is null)";
                sql = string.Format(sql, StationId, SysTagCode);
            }
            else
            {
                sql = "select * from andon.error_log where machine_code='{0}' and system_tag_code='{1}' and (release_time<='2001-01-01 00:00:00' or release_time is null)";
                sql = string.Format(sql, MachineCode, SysTagCode);
            }
            
            var list = PostgreHelper.GetEntityList<error_log>(sql);
            if(list == null || list.Count== 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 检查是否已经存在异常
        /// </summary>
        /// <param name="SysTagCode"></param>
        /// <param name="MachineCode"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public bool CheckMaterialRequireExist(String SysTagCode, int MaterialId, string MachineCode = "", int StationId = -1)
        {
            string sql;
            if (StationId > 0)
            {
                sql = "select * from andon.error_log where station_id={0} and system_tag_code='{1}' and error_type_id='{2}' and (release_time<='2001-01-01 00:00:00' or release_time is null)";
                sql = string.Format(sql, StationId, SysTagCode, MaterialId);
            }
            else
            {
                sql = "select * from andon.error_log where machine_code='{0}' and system_tag_code='{1}' and error_type_id='{2}' and (release_time<='2001-01-01 00:00:00' or release_time is null)";
                sql = string.Format(sql, MachineCode, SysTagCode, MaterialId);
            }

            var list = PostgreHelper.GetEntityList<error_log>(sql);
            if (list == null || list.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(DateTime start_time, DateTime end_time, string device_code = null)
        {
            try
            {
                int count = 0;
                string sql;
                if (string.IsNullOrEmpty(device_code) == false)
                {
                    sql = "delete from andon.error_log where start_time>='{0}' and start_time<='{1}' and machine_code='{2}'";
                    sql = string.Format(sql, start_time, end_time, device_code);
                }
                else
                {
                    sql = "delete from andon.error_log where start_time>='{0}' and start_time<='{1}'";
                    sql = string.Format(sql, start_time, end_time);
                }
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        /// <summary>
        /// 查询指定时间段内的记录
        /// </summary>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="device_code">装置编号</param>
        /// <returns></returns>
        public List<error_log> GetAllLogByTime(DateTime start_time, DateTime end_time, string device_code = null)
        {
            List<error_log> list = new List<error_log>();
            try
            {
                string sql;
                if (string.IsNullOrEmpty(device_code) == false)
                {
                    sql = "select * from andon.error_log where start_time>='{0}' and start_time<='{1}' and machine_code='{2}'";
                    sql = string.Format(sql, start_time, end_time, device_code);
                }
                else
                {
                    sql = "select * from andon.error_log where start_time>='{0}' and start_time<='{1}'";
                    sql = string.Format(sql, start_time, end_time);
                }
                list = PostgreHelper.GetEntityList<error_log>(sql);
                return list;
            }
            catch (Exception ex)
            {
                return list;
            }
        }
    }
}
