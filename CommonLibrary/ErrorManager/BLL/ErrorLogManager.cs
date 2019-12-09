using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorLogManager
    {
        ErrorLogService errorLogService = new ErrorLogService();
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private StationManager stationManager = new StationManager();

        /// <summary>
        /// 返回插入的id
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Insert(error_log Obj)
        {
            return errorLogService.Insert(Obj);
        }
        /// <summary>
        /// 加载指定的异常记录
        /// </summary>
        /// <param name="id">记录id</param>
        /// <returns></returns>
        public error_log GetErrorLogById(int id)
        {
            return errorLogService.GetErrorLogById(id);
        }
        /// <summary>
        /// 加载所有未完成的异常记录
        /// </summary>
        /// <param name="device_code">设备编码</param>
        /// <returns></returns>
        public List<error_log> GetAllUnfinishedByDeviceCode(string device_code)
        {
            return errorLogService.GetAllUnfinishedByDeviceCode(device_code);
        }
        /// <summary>
        /// 加载所有未维护完成的异常记录
        /// </summary>
        /// <param name="MachineCode">设备编码</param>
        /// <returns></returns>
        public List<error_log> GetUnMaintainByDeviceCode(string device_code = null)
        {
            return errorLogService.GetUnMaintainByDeviceCode(device_code);
        }
        /// <summary>
        /// 加载所有未完成的异常记录
        /// </summary>
        /// <param name="StationId">站位id</param>
        /// <returns></returns>
        public List<error_log> GetAllUnfinishedByDeviceId(int device_id)
        {
            return errorLogService.GetAllUnfinishedByDeviceId(device_id);
        }
        /// <summary>
        /// 加载未完成的异常记录
        /// </summary>
        /// <param name="StationId">站位id</param>
        /// <returns></returns>
        public error_log GetUnAckLogByDeviceId(int device_id,string system_tag_code)
        {
            return errorLogService.GetUnAckLogByDeviceId(device_id, system_tag_code);
        }
        /// <summary>
        /// 更新error_log对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Update(error_log Obj)
        {
            return errorLogService.Update(Obj);
        }
        /// <summary>
        /// 更新error_log对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <param name="UpdateDic">更新的键值对信息</param>
        /// <returns></returns>
        public int Update(error_log Obj, Dictionary<string, object> UpdateDic)
        {
            return errorLogService.Update(Obj, UpdateDic);
        }
        public int Del(int id)
        {
            int count = errorLogService.Del(id);
            return count;
        }
        /// <summary>
        /// 检查是否已经存在异常
        /// </summary>
        /// <param name="SysTagCode"></param>
        /// <param name="MachineCode"></param>
        /// <param name="StationId"></param>
        /// <returns>存在返回True，不存在返回False</returns>
        public bool CheckExist(String SysTagCode, string MachineCode = "", int StationId = -1)
        {
            return errorLogService.CheckExist(SysTagCode, MachineCode, StationId);
        }
        /// <summary>
        /// 检查物料呼叫是否已经存在
        /// </summary>
        /// <param name="SysTagCode"></param>
        /// <param name="MachineCode"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public bool CheckMaterialRequireExist(String SysTagCode, int MaterialId, string MachineCode = "", int StationId = -1)
        {
            return errorLogService.CheckMaterialRequireExist(SysTagCode, MaterialId, MachineCode, StationId);
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="device_code">装置编号</param>
        /// <returns></returns>
        public int Del(DateTime start_time, DateTime end_time, string device_code = null)
        {
            int count = errorLogService.Del(start_time, end_time, device_code);
            return count;
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
            return errorLogService.GetAllLogByTime(start_time, end_time, device_code);
        }
    }
}
