using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTagService
    {
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Tricolor_tag obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tricolor_tag>(obj);
                return flag;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询当前machine_code最新一笔数据
        /// </summary>
        /// <param name="machine_code">设备编码</param>
        /// <returns></returns>
        public Tricolor_tag SelectSingle(string machine_code, string tag_code,string date_time = null)
        {
            try
            {
                Tricolor_tag obj = new Tricolor_tag();
                string comm = null;
                if (string.IsNullOrEmpty(date_time))
                {
                    comm = "select  * from fimp.tricolor_tag where machine_code='{0}' and tag_code='{1}' order by id desc limit 1";
                    comm = string.Format(comm, machine_code, tag_code);
                  
                }
                else
                {
                    string insert_time = Convert.ToDateTime(date_time).ToString("yyyy-MM-dd HH:mm:ss.999");
                    comm = "select  * from fimp.tricolor_tag where machine_code='{0}' and tag_code='{1}' and insert_time>'{2}' order by id desc limit 1";
                    comm = string.Format(comm, machine_code, tag_code, insert_time);
                }
                obj = PostgreHelper.GetSingleEntity<Tricolor_tag>(comm);
                
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询当前machine_code最新一笔数据
        /// </summary>
        /// <param name="machine_code">设备编码</param>
        /// <returns></returns>
        public Tricolor_tag SelectLatestSingle(string machine_code, string tag_code)
        {
            try
            {
                Tricolor_tag obj = new Tricolor_tag();
                string comm = null;
               
                comm = "select  * from fimp.tricolor_tag where machine_code='{0}' and tag_code='{1}' and tag_status='1' order by id desc limit 1";
                comm = string.Format(comm, machine_code, tag_code);

                obj = PostgreHelper.GetSingleEntity<Tricolor_tag>(comm);
                
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除machine_code的数据
        /// </summary>
        /// <param name="station_code">设备编号</param>
        /// <param name="delTime">删除日期</param>
        /// <returns></returns>
        public int Del(string machine_code, string delTime)
        {
            try
            {
                string sql = "delete from fimp.tricolor_tag where machine_code='{0}' and insert_time<='{1}'";
                sql = string.Format(sql, machine_code, delTime);
     
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Tricolor_tag obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tricolor_tag>(obj);
                return flag;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }
}
