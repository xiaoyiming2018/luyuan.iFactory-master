using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorConfigManager
    {
        ErrorConfigService errorConfigService = new ErrorConfigService();
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private StationManager stationManager = new StationManager();

        public int Insert(error_config Obj)
        {
            return errorConfigService.Insert(Obj);
        }
        public int Update(error_config Obj)
        {
            return errorConfigService.Update(Obj);
        }
        /// <summary>
        /// 根据id获取单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public error_config GetErrorConfigById(int id)
        {
            return errorConfigService.GetErrorConfigById(id);
        }
       
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程工序，不能为空</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public List<error_config> GetErrorConfig(string system_tag_code, String unit_no, int line_id = -1)
        {
            return errorConfigService.GetErrorConfig(system_tag_code, unit_no, line_id);
        }
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程工序，不能为空</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public error_config GetFirstErrorConfig(string system_tag_code, String unit_no, int line_id = -1)
        {
            return errorConfigService.GetFirstErrorConfig(system_tag_code, unit_no, line_id);
        }
        /// <summary>
        /// 根据装置id获取配置信息
        /// </summary>
        /// <returns></returns>
        public List<error_config> GetLogConfigList(string system_tag_code, int station_id = -1, string machine_code = "")
        {
            List<error_config> list = null;
            if (machine_code.Length > 0)//以machine_code查询
            {
                MachineInfo machine = machineInfoManager.SelectSingle(-1, machine_code);
                if (machine != null)
                {
                    list = errorConfigService.GetErrorConfig(system_tag_code, machine.unit_no, machine.line_id);
                }
            }
            else if (station_id > 0)
            {
                ///Station station = stationManager.SelectSingle(station_id);
                station_info station = stationManager.SelectSingle(station_id);
                if (station != null)
                {
                    list = errorConfigService.GetErrorConfig(system_tag_code, station.unit_no, -1);
                }
            }

            return list;
        }
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public error_config GetErrorConfigByCode(string system_tag_code, String unit_no, int line_id = -1)
        {
            return errorConfigService.GetErrorConfigByCode(system_tag_code, unit_no, line_id);
        }
        public int Del(int id)
        {
            int count = errorConfigService.Del(id);
            return count;
        }

        public List<error_config> SelectAll(int line_id, string unit_no = null, string part_num = null)
        {
            List<error_config> list = null;
            list = errorConfigService.SelectAll(line_id, unit_no, part_num);
            return list;
        }
    }
}
