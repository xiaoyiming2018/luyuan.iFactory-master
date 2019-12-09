using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
   public class TagTimeDayService
    {
        public int Insert(Tag_time_day obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tag_time_day>(obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update(Tag_time_day obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tag_time_day>(obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 更新持续时间
        /// </summary>
        /// <param name="duration">持续时间</param>
        /// <param name="id">id</param>
        /// <returns>影响的行数</returns>
        public int Update(double duration, int id)
        {
            try
            {
                string sql = "Update fimp.tag_time_day set duration={0} where id={1}";
                sql = string.Format(sql, duration, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tag_time_day> SelectByDate(string date)
        {
            try
            {
                string comm = string.Format("select * from fimp.tag_time_day where date='{0}' ", date);
                List<Tag_time_day> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_day>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Tag_time_day> SelectByDeviceAndDate(string device_code, string date)
        {
            try
            {
                string comm = string.Format("select * from fimp.tag_time_day where machine_code='{0}' and  date='{1}' ", device_code,date);
                List<Tag_time_day> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_day>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 判断数据是否存在
        /// </summary>
        /// <param name="machine_code">设备编码</param>
        /// <param name="tag_code">mqtt协议码</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public int GetId(string machine_code, string tag_code, string date)
        {
            try
            {
                int id = 0;
                string sql = "select * from fimp.tag_time_day where machine_code='{0}' and tag_code='{1}' and date='{2}'";
                sql = string.Format(sql, machine_code, tag_code, date);
                List<Tag_time_day> list= PostgreHelper.GetEntityList<Tag_time_day>(sql);
                
                if(list != null && list.Count>0)
                {
                    id = list[0].id;
                }
                return id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
