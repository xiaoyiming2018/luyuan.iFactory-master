using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
  public  class AttendanceManager
    {
        AttendanceService AS = new AttendanceService();
       
        
        //public int Insert(Attendance obj)
        //{
        //    int count = AS.Insert(obj);
        //    return count;
        //}
        public int Del(int id)
        {
            int count = AS.Del(id);
            return count;
        }
        /// <summary>
        /// 向数据库中更新attendance对象信息
        /// </summary>
        /// <returns></returns>
        public int Update(Attendance obj)
        {
            return AS.Update(obj);
        }
        public List<Attendance> SelectAll()
        {
            List<Attendance> objList = AS.GetAllAttendance();
            return objList;
        }
        public Attendance SelectById(int id)
        {
            Attendance objList = AS.SelectById(id);
            return objList;
        }
    }
}
