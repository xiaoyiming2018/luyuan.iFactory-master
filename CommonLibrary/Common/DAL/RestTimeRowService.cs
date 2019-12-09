using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Advantech.IFactory.CommonHelper;
using System.Data.Common;

namespace Advantech.IFactory.CommonLibrary
{
    public class RestTimeRowService
    {
        public List<rest_time_row> SelectAll(string start_time,string end_time)
        {
            try
            {
                List<rest_time_row> objList = new List<rest_time_row>();
                string sql = null;
                if(!string.IsNullOrEmpty(start_time)&&!string.IsNullOrEmpty(end_time))
                {
                    sql = "select * from fimp.rest_time_row where start_time>='{0}' and end_time<='{1}' order by start_time desc,machine_code";
                    sql = string.Format(sql, start_time, end_time);
                }
                else
                {
                    string sys_time = System.DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd HH:mm:ss");
                    sql = "select * from fimp.rest_time_row where end_time>='{0}' order by start_time desc,machine_code";
                    sql = string.Format(sql, sys_time);
                }
                objList = PostgreHelper.GetEntityList<rest_time_row>(sql);
                
                return objList;
                //string sql="select * from rest_time_row where "
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(rest_time_row obj)
        {
            try
            {
                string sql = "insert into fimp.rest_time_row(machine_code,start_time,end_time,tag_code,remarks)values('{0}','{1}','{2}','{3}',N'{4}')";
                sql = string.Format(sql, obj.machine_code, obj.start_time, obj.end_time, obj.tag_code,obj.remarks);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(rest_time_row obj)
        {
            try
            {
                string sql = "Update fimp.rest_time_row set machine_code='{0}',start_time='{1}',end_time='{2}',tag_code='{3}',remarks=N'{4}' where id={5}";
                sql = string.Format(sql, obj.machine_code, obj.start_time, obj.end_time, obj.tag_code, obj.remarks, obj.id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public int Del(int id,string machine_code)
        {
            try
            {
                string sql = null;
                if(string.IsNullOrEmpty(machine_code))
                {
                    sql = "delete from fimp.rest_time_row where id={0}";
                    sql = string.Format(sql, id);
                    
                }
                else
                {
                    sql = "delete from fimp.rest_time_row where machine_code='{0}'";
                    sql = string.Format(sql, machine_code);
                }

                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public rest_time_row SelectSingle(string machine_code,string ts)
        {
            try
            {
                rest_time_row obj = new rest_time_row();
                string sql = "select * from fimp.rest_time_row where machine_code='{0}' and start_time<='{1}' and end_time>='{2}'";
                sql = string.Format(sql, machine_code, ts, ts);
                obj = PostgreHelper.GetSingleEntity<rest_time_row>(sql);
                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public rest_time_row IsUpdate(string machine_code,string start_time,string end_time)
        {
            try
            {
                rest_time_row obj = new rest_time_row();
                string sql = "select * from fimp.rest_time_row where machine_code='{0}' and start_time<='{1}' and end_time>='{2}'";
                sql = string.Format(sql, machine_code, start_time, start_time);
                obj = PostgreHelper.GetSingleEntity<rest_time_row>(sql);

                if (obj!= null)
                {
                }
                else
                {
                    sql = "select * from fimp.rest_time_row where machine_code='{0}' and start_time<='{1}' and end_time>='{2}'";
                    sql = string.Format(sql, machine_code, end_time, end_time);
                    obj = PostgreHelper.GetSingleEntity<rest_time_row>(sql);
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
