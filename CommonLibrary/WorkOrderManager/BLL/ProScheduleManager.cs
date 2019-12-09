using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ProScheduleManager
    {

        ProScheduleService schedule = new ProScheduleService();

        public List<pro_schedule> SelectUnFinished()
        {
            List<pro_schedule> objList = schedule.SelectUnFinished();
            return objList;
        }
      
        /// <summary>
        /// 筛选指定状态的工单
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="line_id"></param>
        /// <param name="order_status">工单状态</param>
        /// <returns></returns>
        public List<pro_schedule> SelectSchedules(string unit_no, int line_id, int order_status)
        {
            List<pro_schedule> objList = schedule.SelectSchedules(unit_no, line_id, order_status);
            return objList;
        }
        public pro_schedule SelectByWorkOrder(string work_order)
        {
            pro_schedule obj = schedule.SelectByWorkOrder(work_order);
            return obj;
        }
        /// <summary>
        /// 查找未执行的工单里面序号最大的工单
        /// </summary>
        /// <returns></returns>
        public pro_schedule GetMaxIndexSchedule()
        {
            pro_schedule obj = schedule.GetMaxIndexSchedule();
            return obj;
        }
        /// <summary>
        /// 通过id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public pro_schedule SelectByID(int id)
        {
            pro_schedule obj = schedule.SelectByID(id);
            return obj;
        }
        public int Insert(pro_schedule obj)
        {
            int count = schedule.Insert(obj);
            return count;
        }
        public int Update(pro_schedule obj, Dictionary<string, object> dic=null)
        {
            int count = schedule.Update(obj, dic);
            return count;
        }
        public List<pro_schedule> SelectScheduleByTime(DateTime start_time, DateTime end_time)
        {
            List<pro_schedule> obj = schedule.SelectScheduleByTime(start_time, end_time);
            return obj;
        }
       
        /// <summary>
        /// 查找当前线别下指定状态下第一条记录的工单
        /// </summary>
        /// <param name="line_id"></param>
        /// <param name="order_status"></param>
        /// <returns></returns>
        public pro_schedule SelectFirstByLineInfo(int line_id, int order_status = (int)OrderStatusEnum.Excuting)
        {
            pro_schedule obj = schedule.SelectFirstByLineInfo(line_id, order_status);
            return obj;
        }
        /// <summary>
        /// 按照线别获取指定记录的工单
        /// </summary>
        /// <param name="line_id">id</param>
        /// <returns></returns>
        public List<pro_schedule> SelectByLineInfo(int line_id, int order_status = 1)
        {
            List<pro_schedule> list = new List<pro_schedule>();
            list= schedule.SelectByLineInfo(line_id, order_status);
            return list;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            return schedule.Del(id);
        }
    }
}
