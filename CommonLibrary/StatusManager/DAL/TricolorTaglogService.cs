using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTaglogService
    {
        /// <summary>
        /// 插入日志表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Tricolor_tag_log obj)
        {
            int flag = PostgreHelper.InsertSingleEntity<Tricolor_tag_log>(obj);
            return flag;
        }

        /// <summary>
        /// 查询设备号最新一笔记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public Tricolor_tag_log SelectNewsLog(string machine_code, string ts)
        {
            try
            {
                string comm = string.Format("select  * from '{0}' where machine_code='{1}' order by insert_time desc limit 1", "fimp.tricolor_tag_log", machine_code);
                Tricolor_tag_log tricolorlog = PostgreHelper.GetSingleEntity<Tricolor_tag_log>(comm);
                return tricolorlog;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询当天记录
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>tricolors<returns>
        public List<Tricolor_tag_log> SelectDayLog(string datetime)
        {
            try
            {
                string comm = string.Format("select  * from '{0}' where insert_time like '{1}%' ", "fimp.tricolor_tag_log", datetime);
                List<Tricolor_tag_log> tricolorlogs = PostgreHelper.GetEntityList<Tricolor_tag_log>(comm);
                return tricolorlogs;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>tricolors<returns>
        public List<Tricolor_tag_log> SelectAllLog(string machine_code)
        {
            try
            {
                string comm = string.Format("select  * from '{0}' where machine_code = '{1}' order by insert_time desc ", "fimp.tricolor_tag_log", machine_code);
                List<Tricolor_tag_log> tricolorlogs = PostgreHelper.GetEntityList<Tricolor_tag_log>(comm);
                return tricolorlogs;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string GetLatestTagCode(string machine_code)
        {
            string tag_code = null;
            string sql = "select * from fimp.tricolor_tag_log where machine_code='{0}' order by insert_time desc LIMIT 1";
            sql = string.Format(sql, machine_code);
            Tricolor_tag_log obj = PostgreHelper.GetSingleEntity<Tricolor_tag_log>(sql);

            if (obj != null)
            {
                tag_code = obj.tag_code;
            }
            else
            {
                tag_code = null;
            }

            return tag_code;
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns>tricolors<returns>
        public int DeleteByTime(DateTime datetime)
        {
            try
            {
                string comm = string.Format("delete from fimp.tricolor_tag_log where insert_time <= '{0}' ", datetime);
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
