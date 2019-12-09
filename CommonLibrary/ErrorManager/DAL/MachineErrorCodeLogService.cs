using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineErrorCodeLogService
    {
        public int Insert(MachineErrorCodeLog obj)
        {
            try
            {
                int count = 0;
                count = PostgreHelper.InsertSingleEntity<MachineErrorCodeLog>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(MachineErrorCodeLog obj,Dictionary<string,object>Dic=null)
        {
            try
            {
                int count = 0;
                if(Dic==null)
                {
                    count = PostgreHelper.UpdateSingleEntity<MachineErrorCodeLog>( obj);
                }
                else
                {
                    count = PostgreHelper.UpdateSingleEntity<MachineErrorCodeLog>(MachineErrorCodeLog.TableName, obj.id, Dic);
                }
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据设备编码查询
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrorCodeLog> SelectUnclosed(string machine_code)
        {
            List<MachineErrorCodeLog> list = new List<MachineErrorCodeLog>();
            try
            {
                string sql;
                sql = "select * from andon.machine_error_code_log where machine_code='{0}' and (away_time<='2001-01-01 00:00:00' OR away_time is null)";
                sql = string.Format(sql, machine_code);
                list = PostgreHelper.GetEntityList<MachineErrorCodeLog>(sql);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据日期，设备编码查询异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrorCodeLog> SelectMachineCodeLog(DateTime start_time, DateTime end_time, string machine_code = null)
        {
            List<MachineErrorCodeLog> list = new List<MachineErrorCodeLog>();
            try
            {
                string sql;
                if (string.IsNullOrEmpty(machine_code) == false)
                {
                    sql = "select * from andon.machine_error_code_log where insert_time>='{0}' and insert_time<='{1}' and  machine_code='{2}'";
                    sql = string.Format(sql, start_time, end_time,machine_code);
                }
                else
                {
                    sql = "select * from andon.machine_error_code_log where insert_time>='{0}' and insert_time<='{1}'";
                    sql = string.Format(sql, start_time, end_time);
                }
                
                list = PostgreHelper.GetEntityList<MachineErrorCodeLog>(sql);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 根据日期，设备编码查询异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrCodeLogForRpt> SelectMachineCodeLogRpt(DateTime start_time, DateTime end_time, string machine_code = null)
        {
            List<MachineErrCodeLogForRpt> list = new List<MachineErrCodeLogForRpt>();
            try
            {
                string sql;
                if (string.IsNullOrEmpty(machine_code) == false)
                {
                    sql = " SELECT code.code_no,code.code_name_en,code.code_name_cn,code.desciption," +
                          " log.id,log.machine_code,log.error_code_id,log.insert_time,log.away_time,log.value" +
                          " from andon.machine_error_code_log log left join andon.machine_error_code code" +
                          " on log.error_code_id = code.id" +
                          " where log.insert_time >='{0}' and log.insert_time <='{1}' and log.machine_code = '{2}'";
                    sql = string.Format(sql, start_time, end_time, machine_code);
                }
                else
                {
                    sql = " SELECT code.code_no,code.code_name_en,code.code_name_cn,code.desciption," +
                           " log.id,log.machine_code,log.error_code_id,log.insert_time,log.away_time,log.value" +
                           " from andon.machine_error_code_log log left join andon.machine_error_code code" +
                           " on log.error_code_id = code.id" +
                           " where log.insert_time >='{0}' and log.insert_time <='{1}'";
                    sql = string.Format(sql, start_time, end_time);
                }

                list = PostgreHelper.GetEntityList<MachineErrCodeLogForRpt>(sql);
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 根据日期，设备编码删除异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public int Delete(DateTime start_time, DateTime end_time, string machine_code = null)
        {
            int count = 0;
            try
            {
                string sql;
                if (string.IsNullOrEmpty(machine_code) == false)
                {
                    sql = "delete from andon.machine_error_code_log where insert_time>='{0}' and insert_time<='{1}' and  machine_code='{2}'";
                    sql = string.Format(sql, start_time, end_time, machine_code);
                }
                else
                {
                    sql = "delete  from andon.machine_error_code_log where insert_time>='{0}' and insert_time<='{1}'";
                    sql = string.Format(sql, start_time, end_time, machine_code);
                }

                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}
