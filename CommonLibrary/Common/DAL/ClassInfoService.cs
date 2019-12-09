using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class ClassInfoService
    {
        //查询多笔数据,可以同过制程编码搜索
        public List<class_info> SelectAll(string class_no = null)
        {
            try
            {
                List<class_info> objList = new List<class_info>();
                string sql = null;
                if(!string.IsNullOrEmpty(class_no))
                {
                    sql = "SELECT * FROM fimp.class_info where class_no='{0}' order by start_time";
                    sql = string.Format(sql, class_no);
                }
                else
                {
                    sql = "SELECT * FROM fimp.class_info order by start_time";
                }
                objList = PostgreHelper.GetEntityList<class_info>(sql);
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //查询单笔数据，通过制程编码搜索
        public class_info SelectSingle(string class_no = null,int id=0)
        {
            try
            {
                class_info obj = new class_info();
                string sql = null;
                if (!string.IsNullOrEmpty(class_no))
                {
                    sql = "SELECT  * FROM fimp.class_info where class_no='{0}' order by start_time";
                    sql = string.Format(sql, class_no);
                }
                else if (id>0)
                {
                    sql = "SELECT  * FROM fimp.class_info where class_id={0} order by start_time";
                    sql = string.Format(sql, id);
                }
                obj = PostgreHelper.GetSingleEntity<class_info>(sql);
                return obj;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
        //插入数据
        public int Insert(class_info obj)
        {
            try
            {
                string sql = "insert into fimp.class_info(class_no,class_name_en,class_name_tw,class_name_cn,start_time,end_time)values('{0}','{1}','{2}','{3}','{4}','{5}')";
                sql = string.Format(sql, obj.class_no, obj.class_name_en, obj.class_name_tw, obj.class_name_cn,obj.start_time,obj.end_time);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(class_info obj)
        {
            try
            {
                string sql = "Update fimp.class_info set class_no='{0}',class_name_en='{1}',class_name_tw='{2}',class_name_cn='{3}',start_time='{4}',end_time='{5}' where class_id={6}";
                sql = string.Format(sql, obj.class_no, obj.class_name_en, obj.class_name_tw, obj.class_name_cn, obj.start_time, obj.end_time,obj.class_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.class_info where class_id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        //判断当前时间是否在班别中，属于哪里班别，班别的开始时间与结束时间
        public class_info SelectClass(string date_time, string date)
        {
            try
            {
                class_info obj = null;
                //转换为时间
                string time = Convert.ToDateTime(date_time).ToString("HH:mm:ss");
                //查询
                string sql = "SELECT * FROM fimp.class_info order by start_time";
                List<class_info> objList = PostgreHelper.GetEntityList<class_info>(sql);

                foreach (var classItem in objList)
                {
                    string start_time = classItem.start_time;
                    string end_time = classItem.end_time;
                    if (Convert.ToDateTime(start_time) > Convert.ToDateTime(end_time))
                    {
                        DateTime start_time1 = Convert.ToDateTime("23:59:59");
                        DateTime end_time1 = Convert.ToDateTime("00:00:00");

                        if (Convert.ToDateTime(start_time) <= Convert.ToDateTime(time) && Convert.ToDateTime(time) <= Convert.ToDateTime(start_time1))
                        {
                            classItem.start_time = date + " " + classItem.start_time;
                            classItem.end_time = Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd") + " " + classItem.end_time;
                            obj = classItem;
                            break;
                        }
                        else if (Convert.ToDateTime(end_time1) <= Convert.ToDateTime(time) && Convert.ToDateTime(time) <= Convert.ToDateTime(end_time))
                        {
                            classItem.start_time = date + " " + classItem.start_time;
                            classItem.end_time = Convert.ToDateTime(date).AddDays(1).ToString("yyyy-MM-dd") + " " + classItem.end_time;
                            obj = classItem;
                            break;
                        }
                    }
                    else
                    {
                        //获取当前时间对应的班别及时间
                        if (Convert.ToDateTime(start_time) <= Convert.ToDateTime(time) && Convert.ToDateTime(time) <= Convert.ToDateTime(end_time))
                        {
                            classItem.start_time = date + " " + classItem.start_time;
                            classItem.end_time = date + " " + classItem.end_time;
                            obj = classItem;
                            break;
                        }
                    }
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
