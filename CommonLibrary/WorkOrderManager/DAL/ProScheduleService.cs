using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Advantech.IFactory.CommonLibrary
{
    public class ProScheduleService: IDataService<pro_schedule>
    {
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int Insert(pro_schedule Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<pro_schedule>(Obj);
            return id;
        }
        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(pro_schedule obj, Dictionary<string, object> dic=null)
        {
            int count = 0;
            try
            {
                if (dic == null)
                {
                    count = PostgreHelper.UpdateSingleEntity<pro_schedule>(obj);
                }
                else
                {
                    count = PostgreHelper.UpdateSingleEntity<pro_schedule>("fimp.pro_schedule", obj.id, dic);
                }
            }
            catch
            {
                count = 0;
            }
            return count;
        }
        /// <summary>
        /// 查询当前未完成的工单
        /// </summary>
        /// <returns></returns>
        public List<pro_schedule> SelectUnFinished()
        {
            List<pro_schedule> objList = new List<pro_schedule>();
            try
            {
                string sql = null;

                sql = "select * from fimp.pro_schedule where order_status<={0} order by order_status,id";
                sql = string.Format(sql, (int)OrderStatusEnum.Excuting);

                objList = PostgreHelper.GetEntityList<pro_schedule>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return objList;

        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<pro_schedule> SelectAll()
        {
            List<pro_schedule> objList = new List<pro_schedule>();
            try
            {
                string sql = null;
                sql = "select * from fimp.pro_schedule order by start_time";
                objList=PostgreHelper.GetEntityList<pro_schedule>(sql);
                return objList;
            }
            catch
            {
                
            }
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
            string command = "";
            List<pro_schedule> list = new List<pro_schedule>();
            try
            {
                command = "SELECT * FROM fimp.pro_schedule where unit_no = '{0}' and line_id = '{1}' and order_status={2} order by start_time";
                command = string.Format(command, unit_no, line_id, order_status);

                list = PostgreHelper.GetEntityList<pro_schedule>(command);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return list;
        }
        /// <summary>
        /// 通过id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public pro_schedule SelectByID(int id)
        {
            pro_schedule obj = null;
            try
            {
                string sql = "select * from fimp.pro_schedule where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<pro_schedule>(sql);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return obj;
        }
        
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = 0;
            try
            {
                string sql = "delete from  fimp.pro_schedule where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
        /// <summary>
        /// 根据工单号码获取工单对象
        /// </summary>
        /// <param name="work_order">工单</param>
        /// <returns></returns>
        public pro_schedule SelectByWorkOrder(string work_order)
        {
            pro_schedule obj = new pro_schedule();
            try
            {
                string sql = "select * from fimp.pro_schedule where work_order='{0}'";
                sql = string.Format(sql, work_order);
                obj = PostgreHelper.GetSingleEntity<pro_schedule>(sql);
            }
            catch
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 查找序号最大的工单
        /// </summary>
        /// <returns></returns>
        public pro_schedule GetMaxIndexSchedule()
        {
            pro_schedule obj = new pro_schedule();
            try
            {
                string sql = "SELECT * FROM fimp.pro_schedule where order_status<'{0}' ORDER BY order_index desc limit 1 ";
                sql = string.Format(sql,(int)OrderStatusEnum.Excuting);
                obj = PostgreHelper.GetSingleEntity<pro_schedule>(sql);
            }
            catch
            {
                obj = null;
            }
            return obj;
        }

        /// <summary>
        /// 获取单笔数据
        /// </summary>
        /// <param name="line_id">id</param>
        /// <returns></returns>
        public pro_schedule SelectFirstByLineInfo(int line_id, int order_status = 1)
        {
            pro_schedule obj = new pro_schedule();
            try
            {
                string sql = "select * from fimp.pro_schedule where line_id={0} and order_status={1} order by start_time limit 1";
                sql = string.Format(sql, line_id, order_status);
                obj = PostgreHelper.GetSingleEntity<pro_schedule>(sql);
            }
            catch
            {
                obj = null;
            }
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
            try
            {
                string sql = "select * from fimp.pro_schedule where line_id={0} and order_status={1} order by start_time";
                sql = string.Format(sql, line_id, order_status);
                list = PostgreHelper.GetEntityList<pro_schedule>(sql);
            }
            catch
            {
                list = null;
            }
            return list;
        }
        
        /// <summary>
        /// 通过时间段查询记录
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <returns></returns>
        public List<pro_schedule> SelectScheduleByTime(DateTime start_time, DateTime end_time)
        {
            List<pro_schedule> objlist = new List<pro_schedule>();
            try
            {
                string sql = "select * from fimp.pro_schedule where insert_time>='{0}' and insert_time<='{1}'";
                sql = string.Format(sql, start_time, end_time);
                objlist = PostgreHelper.GetEntityList<pro_schedule>(sql);
                return objlist;
            }
            catch
            {
                objlist = null;
            }
            return objlist;
        }
    }
}
