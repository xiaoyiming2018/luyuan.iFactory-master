using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTagDurationService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Tricolor_tag_duration obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tricolor_tag_duration>(obj);
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Tricolor_tag_duration obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tricolor_tag_duration>(obj);
                return flag;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }

        /// <summary>
        /// 查询设备的持续时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int SelectDuration(string machinecode)
        {
            try
            {
                Tricolor_tag_duration durationinfo = new Tricolor_tag_duration();
                durationinfo = PostgreHelper.GetSingleEntity<Tricolor_tag_duration>(machinecode);
                return durationinfo.duration_second;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// 查询最近在线的状态
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectAllLatestOnlineStatus(string device_code=null)
        {
            try
            {
                List<Tricolor_tag_duration> list = new List<Tricolor_tag_duration>();
                string comm = null;
                if(string.IsNullOrEmpty(device_code))
                {
                    comm = "select * from fimp.tricolor_tag_duration where tag_status='1' order by tag_code";
                }
                else
                {
                    comm = "select  * from fimp.tricolor_tag_duration where machine_code='{0}' and tag_status='1' order by id";
                    comm = string.Format(comm, device_code);
                }
               
                list = PostgreHelper.GetEntityList<Tricolor_tag_duration>(comm);

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 查询当前持续的状态，并且超过了某一个时间节点
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectInterStatus(DateTime query_time,string tag_code)
        {
            try
            {
                List<Tricolor_tag_duration> list = new List<Tricolor_tag_duration>();
                string comm = null;
                
                comm = "select  * from fimp.tricolor_tag_duration where ((insert_time<'{0}' and tag_status='1') or " +
                      " (insert_time<'{1}' and away_time>'{2}' and tag_status='0')) and tag_code='{3}' order by id";
                comm = string.Format(comm, query_time, query_time, query_time, tag_code);
                

                list = PostgreHelper.GetEntityList<Tricolor_tag_duration>(comm);

                return list;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int Del(string machine_code, string delTime)
        {
            try
            {
                string sql = "delete from fimp.tricolor_tag_duration where machine_code='{0}' and insert_time<='{1}'";
                sql = string.Format(sql, machine_code, delTime);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// 按照日期删除数据
        /// </summary>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime delTime)
        {
            try
            {
                string sql = "delete from fimp.tricolor_tag_duration where insert_time<='{0}'";
                sql = string.Format(sql, delTime);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return 0;
        }
        /// <summary>
        /// 获取指定时间段内的状态时间表。时间为空则查询所有状态.时间不传输则抓取当前status=true的记录
        /// </summary>
        /// <param name="machine_code">设备代码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <param name="tag_code">标签</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectTagDurationList(string machine_code, string start_time, string end_time,
                                                                 string tag_code = null,bool status= false)
        {
            try
            {
                List<Tricolor_tag_duration> objList = new List<Tricolor_tag_duration>();
                string sql;
                if(!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(tag_code))
                {
                    sql = "select * from fimp.tricolor_tag_duration where machine_code='{0}' and insert_time>='{1}' and insert_time<'{2}' and tag_code='{3}' order by tag_code";
                    sql = string.Format(sql, machine_code, start_time, end_time, tag_code);
                }
                else
                {
                    sql = "select * from fimp.tricolor_tag_duration where machine_code='{0}' and tag_status='1' order by tag_code";
                    sql = string.Format(sql, machine_code);
                }

                objList=PostgreHelper.GetEntityList<Tricolor_tag_duration>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
        /// <summary>
        /// 获取指定时间段内的状态时间表。时间为空则查询所有状态.时间不传输则抓取当前status=true的记录
        /// </summary>
        /// <param name="machine_code">设备代码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间,结束时间小于开始时间，则按照开始时间查询</param>
        /// <param name="tag_code">标签</param>
        /// <param name="status">状态</param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectTagDurationListByTime(DateTime start_time, DateTime end_time,
                                                                       string tag_code = null)
        {
            try
            {
                List<Tricolor_tag_duration> objList = new List<Tricolor_tag_duration>();
                string sql=string.Empty;
                if (!string.IsNullOrEmpty(tag_code))
                {
                    if(start_time> end_time)//开始时间大于结束时间，则结束时间为默认时间，还未结束
                    {
                        sql = "select * from fimp.tricolor_tag_duration where  insert_time>='{0}' and tag_code='{1}' order by tag_code";
                        sql = string.Format(sql, start_time, tag_code);
                    }
                    else
                    {
                        sql = "select * from fimp.tricolor_tag_duration where  insert_time>='{0}' and insert_time<'{1}' and tag_code='{2}' order by tag_code";
                        sql = string.Format(sql, start_time, end_time, tag_code);
                    }
                }
               
                objList = PostgreHelper.GetEntityList<Tricolor_tag_duration>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }
    }
}
