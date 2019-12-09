using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class AttendanceService
    {
        public int Update(Attendance obj)
        {
            try
            {
                int count = 0;
                if (obj.id == 0)
                {
                    DateTime time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    Decimal achievement = Convert.ToDecimal(obj.real_num) / Convert.ToDecimal(obj.standard_num);

                    string sql = "insert into fimp.attendance(standard_num,real_num,attendance_rate,createtime,line_id,unit_no,class_no)values('{0}','{1}','{2}','{3}','{4}','{5}','{6}')";
                    sql = string.Format(sql, obj.standard_num, obj.real_num, achievement, time, obj.line_id,obj.unit_no,obj.class_no);
                    count = PostgreHelper.ExecuteNonQuery(sql);
                    return count;
                }
                else
                {
                    DateTime time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    Decimal achievement = Convert.ToDecimal(obj.real_num) / Convert.ToDecimal(obj.standard_num);
                    string sql = "update fimp.attendance set standard_num='{0}',real_num='{1}',attendance_rate='{2}',createtime='{3}',line_id='{4}', unit_no='{5}',class_no='{6}' where id={7}";
                    sql = string.Format(sql, obj.standard_num, obj.real_num, achievement, time, obj.line_id,obj.unit_no,obj.class_no, obj.id);
                    count = PostgreHelper.ExecuteNonQuery(sql);
                    return count;
                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
        /// <summary>
        /// 加载记录
        /// </summary>
        /// <returns></returns>
        public List<Attendance> GetAllAttendance() 
        {
            try
            {
                List<Attendance> objList = new List<Attendance>();
                string sql = null;
                sql = "select * from fimp.attendance ";
                objList = PostgreHelper.GetEntityList<Attendance>(sql);
               
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Attendance SelectById(int id)
        {
            Attendance attItem = new Attendance();
            string command = string.Format("Select * from fimp.attendance where id= {0}", id);
            attItem = PostgreHelper.GetSingleEntity<Attendance>(command);
            return attItem;
        }
        /// <summary>
        /// 更新attendance对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        //public int Update(attendance Obj)
        //{
        //    int id = PostgreHelper.UpdateSingleEntity<attendance>("attendance", Obj.id, Obj);
        //    return id;
        //}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.attendance where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
