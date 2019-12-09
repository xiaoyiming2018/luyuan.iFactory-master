using System;
using System.Collections.Generic;
using Advantech.IFactory.CommonHelper;


namespace Advantech.IFactory.CommonLibrary
{
    public class CTService
    {
        /// <summary>
        /// 插入C/T
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(CT obj)
        {
            try
            {
                int id = PostgreHelper.InsertSingleEntity<CT>(obj);
                return id;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return -1;
        }

        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public List<CT> SelectAll(string device_code = null)
        {
            try
            {
                List<CT> objList = new List<CT>();
                string comm;
                if(string.IsNullOrEmpty(device_code))
                {
                    comm = string.Format("SELECT * FROM {0} order by end_time desc", "fimp.ct");
                }
                else
                {
                    comm = string.Format("SELECT * FROM {0} where machine_code='{1}' order by end_time desc", "fimp.ct", device_code);
                }
                objList =PostgreHelper.GetEntityList<CT>(comm);
                return objList;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public List<CT> SelectAllByWorkOrder(string device_code, string work_order, string pn,string tag_code)
        {
            try
            {
                List<CT> objList = new List<CT>();
                string comm;
                if(string.IsNullOrEmpty(tag_code)==false)
                {
                    comm = "SELECT * FROM fimp.ct where machine_code='{0}' and wo='{1}' and pn='{2}' and tag_code='{3}' order by end_time desc";
                    comm = string.Format(comm, device_code, work_order, pn, tag_code);
                }
                else
                {
                    comm = "SELECT * FROM fimp.ct where machine_code='{0}' and wo='{1}' and pn='{2}' order by end_time desc";
                    comm = string.Format(comm, device_code, work_order, pn);
                }

                objList = PostgreHelper.GetEntityList<CT>(comm);
                return objList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 获取最后一笔数据
        /// </summary>
        /// <param name="machine_code">设备编号</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public CT SelectSingle(string machine_code, string date_time)
        {
            try
            {
                CT obj = new CT();
                string comm = null;
                if (string.IsNullOrEmpty(date_time))
                {
                    comm = "select  id,machine_code,station_id,pn,wo,start_time,end_time,value,tag_code from fimp.ct where machine_code='{0}' order by end_time desc limit 1";
                    comm = string.Format(comm, machine_code);
                }
                else
                {
                    string end_time = Convert.ToDateTime(date_time).ToString("yyyy-MM-dd HH:mm:ss.999");
                    comm = "select  id,machine_code,station_id,pn,wo,start_time,end_time,value,tag_code from fimp.ct where machine_code='{0}' and end_time>'{1}' order by end_time desc limit 1";
                    comm = string.Format(comm, machine_code, end_time);
                }
                obj = PostgreHelper.GetSingleEntity<CT>(comm);
         
                return obj;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 获取最后一笔数据
        /// </summary>
        /// <param name="station_id">站位编号</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public CT SelectSingle(int station_id, string date_time)
        {
            try
            {
                CT obj = new CT();
                string comm = null;
                if (string.IsNullOrEmpty(date_time))
                {
                    comm = "select id,machine_code,station_id,pn,wo,start_time,end_time,value,tag_code from fimp.ct where station_id='{0}' order by end_time desc limit 1";
                    comm = string.Format(comm, station_id);
                }
                else
                {
                    string end_time = Convert.ToDateTime(date_time).ToString("yyyy-MM-dd HH:mm:ss.999");
                    comm = "select id,machine_code,station_id,pn,wo,start_time,end_time,value,tag_code from fimp.ct where station_id='{0}' and end_time>'{1}' order by end_time desc limit 1";
                    comm = string.Format(comm, station_id, end_time);
                }
                obj = PostgreHelper.GetSingleEntity<CT>(comm);

                return obj;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }

        /// <summary>
        /// 按照时间定期删除记录
        /// </summary>
        /// <param name="station_id">站位编号</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public int DelByTime(DateTime datetime)
        {
            int count = 0;
            try
            {
                string comm;
                comm = string.Format("delete FROM {0} where start_time<='{1}'", "fimp.ct", datetime);
                count=PostgreHelper.ExecuteNonQuery(comm);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return count;
        }
    }
} 
