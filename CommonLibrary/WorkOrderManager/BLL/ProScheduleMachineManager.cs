using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
  public  class ProScheduleMachineManager
    {
     
        ProScheduleMachineService schedule = new ProScheduleMachineService();
        public List<Pro_schedule_machine> SelectAll(string start_time = null, string end_time = null, string work_order = null)
        {
            List<Pro_schedule_machine> objList = schedule.SelectAll(start_time, end_time, work_order);
            return objList;
        }
        public Pro_schedule_machine SelectById(int id)
        {
            Pro_schedule_machine obj = schedule.SelectById(id);
            return obj;
        }
        public Pro_schedule_machine SelectOnLineWorkorder(string device_code)
        {
            Pro_schedule_machine obj = schedule.SelectOnLineWorkorder(device_code);
            return obj;
        }
        public int Insert(Pro_schedule_machine obj)
        {
            int count = schedule.Insert(obj);
            return count;
        }
        public int Update(Pro_schedule_machine obj, Dictionary<string, object> Dic=null)
        {
            int count = schedule.Update(obj, Dic);
            return count;
        }
        
        /// <summary>
        /// 筛选设备或者站位工单，不管状态
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="unit_no"></param>
        /// <param name="device_code">设备编码</param>
        /// <returns></returns>
        public Pro_schedule_machine SelectByWorkOrder(string work_order, string unit_no, string device_code)
        {
            Pro_schedule_machine obj = schedule.SelectByWorkOrder(work_order, unit_no, device_code);
            return obj;
        }
        /// <summary>
        /// 筛选设备或者站位工单，不管状态
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="unit_no"></param>
        /// <param name="device_code">设备编码</param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectByWorkOrder(string work_order, string unit_no)
        {
            List<Pro_schedule_machine> list = schedule.SelectByWorkOrder(work_order, unit_no);
            return list;
        }

        public Pro_schedule_machine SelectSingle(string work_order, string unit_no, string machine_code, int order_status = (int)OrderStatusEnum.Excuting)
        {
            Pro_schedule_machine obj = schedule.SelectSingle(work_order, unit_no, machine_code, order_status);
            return obj;
        }
        /// <summary>
        /// 查找设备所属的工单
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="device_code"></param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectSchedule(string device_code,string unit_no=null,int order_status = (int)OrderStatusEnum.Excuting)
        {
            List<Pro_schedule_machine> list = schedule.SelectSchedule(device_code, unit_no, order_status);
            return list;
        }
        /// <summary>
        /// 查找未完成的工单
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="device_code"></param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectUnFinishedSchedule(string device_code, string unit_no = null)
        {
            List<Pro_schedule_machine> list = schedule.SelectUnFinishedSchedule(device_code, unit_no);
            return list;
        }
        /// <summary>
        /// 删除工单
        /// </summary>
        /// <param name="work_order"></param>
        /// <returns></returns>
        public int DeleteByWorkOrder(string work_order)
        {
            return schedule.DeleteByWorkOrder(work_order);
        }
    }
}
