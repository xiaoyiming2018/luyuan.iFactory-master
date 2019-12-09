using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagTimeClassService
    {
        public int Insert(Tag_time_class obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tag_time_class>(obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update(Tag_time_class obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tag_time_class>(obj);
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
                string sql = "Update fimp.tag_time_class set duration={0} where id={1}";
                sql = string.Format(sql, duration, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tag_time_class> Select(string class_no)
        {
            try
            {
                string comm = string.Format("select * from fimp.tag_time_class where class_no='{0}' ", class_no);
                List<Tag_time_class> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_class>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<Tag_time_class> SelectByDateAndClass(string class_no, string date)
        {
            try
            {
                string comm = string.Format("select * from fimp.tag_time_class where class_no='{0}' and date='{1}'", class_no, date);
                List<Tag_time_class> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_class>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tag_time_class> SelectByDateAndClass(string device_code, string class_no, string date)
        {
            try
            {
                string comm = "select * from fimp.tag_time_class where machine_code='{0}' and class_no='{1}' and date='{2}'";
                comm = string.Format(comm, device_code, class_no, date);
                List<Tag_time_class> list = PostgreHelper.GetEntityList<Tag_time_class>(comm);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Tag_time_class> SelectUnAckLog(string tag_code, string machine_code = "", int station_id = -1)
        {
            string comm;
            try
            {
                if (machine_code.Length > 0)
                {
                    comm = string.Format("select * from fimp.tag_time_day where tag_code='{0}' and machine_code='{1}' and details_id=null", tag_code, machine_code);
                }
                else
                {
                    comm = string.Format("select * from fimp.tag_time_day where tag_code='{0}' and station_id='{1}' and details_id=null", tag_code, station_id);
                }
                List<Tag_time_class> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_class>(comm);
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
        /// <param name="class_no">班别代码</param>
        /// <param name="date">日期</param>
        /// <returns></returns>
        public int GetId(string machine_code, string tag_code, string class_no, string date)
        {
            try
            {
                int id = 0;
                string sql = "select * from fimp.tag_time_class where machine_code='{0}' and tag_code='{1}' and class_no='{2}' and date='{3}'";
                sql = string.Format(sql, machine_code, tag_code, class_no, date);
                //using (SqlDataReader dr = Helper.SqlHelper.GetReader(sql))
                //{
                //    if (dr.Read())
                //    {
                //        id = Convert.ToInt32(dr["id"]);
                //    }
                //    else
                //    {
                //        id = 0;
                //    }
                //}
                Tag_time_class tagClass= PostgreHelper.GetSingleEntity<Tag_time_class>(sql);
                if(tagClass !=null)
                {
                    return tagClass.id;
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
