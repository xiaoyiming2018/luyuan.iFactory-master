using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
   public class ProScheduleMachineService : IDataService<Pro_schedule_machine>
    {

        /// <summary>
        /// 更新Pro_schedule_machine对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Update(Pro_schedule_machine Obj, Dictionary<string, object> Dic = null)
        {
            int count = 0;
            if(Dic!=null)
            {
                count = PostgreHelper.UpdateSingleEntity<Pro_schedule_machine>("fimp.Pro_schedule_machine", Obj.id, Dic);
            }
            else
            {
                count = PostgreHelper.UpdateSingleEntity<Pro_schedule_machine>(Obj);
            }
            
            return count;
        }
        public List<Pro_schedule_machine> SelectAll(string start_time, string end_time, string work_order)
        {
            List<Pro_schedule_machine> objList = new List<Pro_schedule_machine>();
            try
            {
                string sql = null;
                if (!string.IsNullOrEmpty(work_order) && !string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
                {
                    sql = "select * from fimp.pro_schedule_machine where work_order='{0}' and start_time>='{1}' and  start_time<='{2}' order by start_time desc";
                    sql = string.Format(sql, work_order, start_time, end_time);
                }
                else if (!string.IsNullOrEmpty(work_order) && string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time))
                {
                    sql = "select * from fimp.pro_schedule_machine where work_order='{0}' order by start_time desc";
                    sql = string.Format(sql, work_order);
                }
                else if (string.IsNullOrEmpty(work_order) && !string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time))
                {
                    sql = "select * from fimp.pro_schedule_machine where start_time>='{0}' and  start_time<='{1}' order by start_time desc";
                    sql = string.Format(sql, start_time, end_time);
                }
                else
                {
                    sql = "select * from fimp.pro_schedule_machine order by start_time desc";
                }
               
                objList = PostgreHelper.GetEntityList<Pro_schedule_machine>(sql);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objList;
        }
        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="machine_code">设备编号</param>
        /// <returns></returns>
        public Pro_schedule_machine SelectById(int id)
        {
            Pro_schedule_machine obj = new Pro_schedule_machine();
            try
            {
                string sql = "select  * from fimp.pro_schedule_machine where id='{0}'";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<Pro_schedule_machine>(sql);
            }
            catch (Exception ex)
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 获取当前设备的最新的在制工单信息
        /// </summary>
        /// <param name="machine_code">设备编号</param>
        /// <returns></returns>
        public Pro_schedule_machine SelectOnLineWorkorder(string device_code)
        {
            Pro_schedule_machine obj = new Pro_schedule_machine();
            try
            {
                string sql = "select  * from fimp.pro_schedule_machine where machine_code='{0}' and order_status='{1}' order by last_time desc limit 1";
                sql = string.Format(sql, device_code,(int)OrderStatusEnum.Excuting);
                obj=PostgreHelper.GetSingleEntity<Pro_schedule_machine>(sql);
            }
            catch(Exception ex)
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// 插入工单信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Pro_schedule_machine obj)
        {
            int count = 0;
            try
            {
                count = PostgreHelper.InsertSingleEntity<Pro_schedule_machine>(obj);
            }
            catch (Exception ex)
            {
                count = 0;
            }
            return count;
        }
        public Pro_schedule_machine SelectSingle(string work_order, string unit_no, string machine_code, int order_status = (int)OrderStatusEnum.Excuting)
        {
            Pro_schedule_machine obj = new Pro_schedule_machine();
            try
            {
                string sql = "select * from fimp.pro_schedule_machine where work_order='{0}' and unit_no='{1}' and machine_code='{2}' and order_status='{3}'";
                sql = string.Format(sql, work_order, unit_no, machine_code, order_status);
                obj = PostgreHelper.GetSingleEntity<Pro_schedule_machine>(sql);
            }
            catch (Exception ex)
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 筛选工单，不管状态
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="unit_no"></param>
        /// <param name="device_code">设备编码，可为空。为空则抓取当前线别的工单</param>
        /// <returns></returns>
        public Pro_schedule_machine SelectByWorkOrder(string work_order, string unit_no, string device_code)
        {
            Pro_schedule_machine obj = new Pro_schedule_machine();
            try
            {
                string sql;

                sql = "select * from fimp.pro_schedule_machine where work_order='{0}' and unit_no='{1}' and machine_code='{2}'";
                sql = string.Format(sql, work_order, unit_no, device_code);

                obj = PostgreHelper.GetSingleEntity<Pro_schedule_machine>(sql);
            }
            catch (Exception ex)
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 筛选工单，不管状态
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="unit_no"></param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectByWorkOrder(string work_order, string unit_no)
        {
            List<Pro_schedule_machine> list = new List<Pro_schedule_machine>();
            try
            {
                string sql;

                sql = "select * from fimp.pro_schedule_machine where work_order='{0}' and unit_no='{1}'";
                sql = string.Format(sql, work_order, unit_no);

                list = PostgreHelper.GetEntityList<Pro_schedule_machine>(sql);
            }
            catch (Exception ex)
            {
                list = null;
            }
            return list;
        }
        /// <summary>
        /// 查找设备所属的工单
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="device_code"></param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectSchedule(string device_code, string unit_no=null,int order_status = (int)OrderStatusEnum.Excuting)
        {
            try
            {
                string sql;
                if(string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from fimp.pro_schedule_machine where machine_code='{0}' and order_status='{1}' order by start_time desc";
                    sql = string.Format(sql, device_code, order_status);
                }
                else
                {
                    sql = "select * from fimp.pro_schedule_machine where machine_code='{0}' and unit_no='{1}' and order_status='{2}' order by start_time desc";
                    sql = string.Format(sql, device_code, unit_no, order_status);
                }
                return PostgreHelper.GetEntityList<Pro_schedule_machine>(sql);
                
            }
            catch(Exception ex)
            {
                string s = ex.Message;
            }
            return null;
        }
        /// <summary>
        /// 查找设备所属的工单.参数均为空情况返回所有未执行完成的，正常情况返回排产的
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="device_code"></param>
        /// <returns></returns>
        public List<Pro_schedule_machine> SelectUnFinishedSchedule(string device_code, string unit_no = null)
        {
            try
            {
                string sql;
                
                if (string.IsNullOrEmpty(device_code) && string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from fimp.pro_schedule_machine where order_status<='{0}' order by insert_time";
                    sql = string.Format(sql, (int)OrderStatusEnum.Excuting);
                }
                else if (string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from fimp.pro_schedule_machine where machine_code='{0}' and order_status='{1}' order by insert_time";
                    sql = string.Format(sql, device_code, (int)OrderStatusEnum.Scheduled);
                }
                else
                {
                    sql = "select * from fimp.pro_schedule_machine where machine_code='{0}' and unit_no='{1}' and order_status='{2}' order by insert_time";
                    sql = string.Format(sql, device_code, unit_no, (int)OrderStatusEnum.Scheduled);
                }
                return PostgreHelper.GetEntityList<Pro_schedule_machine>(sql);

            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return null;
        }
        /// <summary>
        /// 按照工单号删除设备工单
        /// </summary>
        /// <param name="unit_no"></param>
        /// <param name="device_code"></param>
        /// <returns></returns>
        public int DeleteByWorkOrder(string work_order)
        {
            try
            {
                string sql;

                sql = "delete from fimp.pro_schedule_machine where work_order='{0}'";
                sql = string.Format(sql, work_order);

                return PostgreHelper.ExecuteNonQuery(sql);
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
            return 0;
        }

        public List<Pro_schedule_machine> SelectAll()
        {
            throw new NotImplementedException();
        }

        public Pro_schedule_machine SelectByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Del(int id)
        {
            throw new NotImplementedException();
        }
    }
}
