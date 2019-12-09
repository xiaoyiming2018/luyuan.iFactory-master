using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineErrorCodeLogManager
    {
        MachineErrorCodeLogService machineErrorCodeLogService = new MachineErrorCodeLogService();

        public int Insert(MachineErrorCodeLog obj)
        {
            return machineErrorCodeLogService.Insert(obj);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(MachineErrorCodeLog obj, Dictionary<string, object> Dic = null)
        {
            return machineErrorCodeLogService.Update(obj, Dic);
        }
        /// <summary>
        /// 根据设备编码查询未结束的异常
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrorCodeLog> SelectUnclosed(string machine_code)
        {
            return machineErrorCodeLogService.SelectUnclosed(machine_code);
        }
        /// <summary>
        /// 根据日期，设备编码查询异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrorCodeLog> SelectMachineCodeLog(DateTime start_time, DateTime end_time,string machine_code=null)
        {
            return machineErrorCodeLogService.SelectMachineCodeLog(start_time, end_time, machine_code);
        }
        /// <summary>
        /// 根据日期，设备编码查询异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public List<MachineErrCodeLogForRpt> SelectMachineCodeLogRpt(DateTime start_time, DateTime end_time, string machine_code = null)
        {
            return machineErrorCodeLogService.SelectMachineCodeLogRpt(start_time, end_time, machine_code);
        }
        /// <summary>
        /// 根据日期，设备编码删除异常记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public int Delete(DateTime start_time, DateTime end_time, string machine_code = null)
        {
            return machineErrorCodeLogService.Delete(start_time, end_time, machine_code);
        }
    }
}
