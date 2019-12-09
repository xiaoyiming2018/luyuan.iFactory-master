using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Advantech.IFactory.CommonHelper;
using System.Data;
using System.Data.SqlClient;

namespace Advantech.IFactory.CommonLibrary
{
    public class CTLogService
    {
        /// <summary>
        /// 插入C/T日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(CT_Log obj)
        {
            try
            {
                int id = PostgreHelper.InsertSingleEntity<CT_Log>(obj);
                return id;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public CT_Log SelectNewsLog(string machine_code, string ts)
        {
            try
            {
                string comm = "select * from fimp.ct_log where machine_code='{0}' order by insert_time desc limit 1";
                CT_Log ctlog = PostgreHelper.GetSingleEntity<CT_Log>(comm);
                return ctlog;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查找最近的一笔记录
        /// </summary>
        /// <param name="device_code"></param>
        /// <param name="tag_code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CT_Log SelectLatestLog(string device_code, string tag_code,int value = -1, DateTime start_time = default(DateTime))
        {
            try
            {
                string comm;
                if(start_time > default(DateTime) || start_time>Convert.ToDateTime("2001-01-01"))
                {
                    if (value >= 0)
                    {
                        comm = "select * from fimp.ct_log where machine_code='{0}' and tag_code='{1}' and value='{2}' and insert_time>='{3}' order by insert_time desc limit 1";
                        comm = string.Format(comm, device_code, tag_code, value, start_time);
                    }
                    else
                    {
                        comm = "select * from fimp.ct_log where machine_code='{0}' and tag_code='{1}' and insert_time>='{2}' order by insert_time desc limit 1";
                        comm = string.Format(comm, device_code, tag_code, start_time);
                    }
                }
                else
                {
                    if (value >= 0)
                    {
                        comm = "select * from fimp.ct_log where machine_code='{0}' and tag_code='{1}' and value='{2}' order by insert_time desc limit 1";
                        comm = string.Format(comm, device_code, tag_code, value);
                    }
                    else
                    {
                        comm = "select * from fimp.ct_log where machine_code='{0}' and tag_code='{1}' order by insert_time desc limit 1";
                        comm = string.Format(comm, device_code, tag_code);
                    }
                }
                
                CT_Log ctlog = PostgreHelper.GetSingleEntity<CT_Log>(comm);
                return ctlog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查找最近的一笔记录
        /// </summary>
        /// <param name="device_code"></param>
        /// <param name="tag_code"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public CT_Log SelectLatestLog(string device_code, string tag_code, DateTime datetime, int value)
        {
            try
            {
                string comm;

                comm = "select * from fimp.ct_log where machine_code='{0}' and tag_code='{1}' and insert_time<'{2}' and value='{3}'  order by insert_time desc limit 1";
                comm = string.Format(comm, device_code, tag_code, datetime, value);

                CT_Log ctlog = PostgreHelper.GetSingleEntity<CT_Log>(comm);
                return ctlog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            try
            {
                string comm = "delete from fimp.ct_log where insert_time<='{0}'";
                comm = string.Format(comm, datetime);
                int count = PostgreHelper.ExecuteNonQuery(comm);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
