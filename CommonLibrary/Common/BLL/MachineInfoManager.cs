using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineInfoManager
    {
        MachineInfoService MIS = new MachineInfoService();

        /// <summary>
        /// 获取站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<MachineInfo> SelectAll(MachineInfo obj, string unit_no = null)
        {
            List<MachineInfo> objList = MIS.SelectAll(obj, unit_no);
            return objList;
        }
        public List<MachineInfo> Select(int machine_id = 0)
        {
            List<MachineInfo> objList = MIS.Select(machine_id);
            return objList;
        }
        /// <summary>
        /// 获取站别信息,id全为name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<MachineInfoName> SelectAllName(MachineInfoName obj)
        {
            List<MachineInfoName> objList = MIS.SelectAllName(obj);
            return objList;
        }

        /// <summary>
        /// 通过站别id获取machine_name
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="unit_no"></param>
        /// <returns></returns>
        public List<MachineInfo> SelectMachineNameByStaion(int id)
        {
            List<MachineInfo> objList = MIS.SelectMachineNameByStaion(id);
            return objList;
        }

        /// <summary>
        /// 插入站别信息数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(MachineInfo obj, int machine_id=0)
        {
            int count = MIS.Insert(obj, machine_id);
            return count;
        }

        /// <summary>
        /// 更新站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(MachineInfo obj)
        {
            int count = MIS.Update(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = MIS.Del(id);
            return count;
        }

        /// <summary>
        /// 查询单笔数据，通过id或machine
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MachineInfo SelectSingle(int id = 0,string machine_code = null)
        {
            MachineInfo obj = new MachineInfo();
            obj = MIS.SelectSingle(id: id, machine_code: machine_code);
            return obj;
        }
        /// <summary>
        /// 查询单笔数据，通过id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MachineInfo SelectSingleById(int id = 0)
        {
            MachineInfo obj = new MachineInfo();
            obj = MIS.SelectSingleById(id: id);
            return obj;
        }
        /// <summary>
        /// 更新站别状态
        /// </summary>
        /// <param name="station_code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int UpdateState(string machine_code, string status_no)
        {
            int count = MIS.UpdateState(machine_code, status_no);
            return count;
        }

        public int UpdateMqttState(string machine_code, string mqtt_no)
        {
            int count = MIS.UpdateMqttState(machine_code, mqtt_no);
            return count;
        }

         /// <summary>
        /// 更新unit_no
        /// </summary>
        /// <param name="old_unit_no">旧</param>
        /// <param name="unit_no">新</param>
        /// <returns></returns>
        public int UpdateUnitNo(string old_unit_no, string unit_no)
        {
            int count = MIS.UpdateUnitNo(old_unit_no, unit_no);
            return count;
        }
    }
}
