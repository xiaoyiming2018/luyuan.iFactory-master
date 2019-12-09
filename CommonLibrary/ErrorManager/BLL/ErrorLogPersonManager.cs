using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorLogPersonManager
    {
        ErrorLogPersonService errorLogPersonService = new ErrorLogPersonService();

        public int Insert(error_log_person Obj)
        {
            return errorLogPersonService.Insert(Obj);
        }

        /// <summary>
        /// 记录通知的人员信息
        /// </summary>
        /// <param name="LogId"></param>
        /// <param name="PersonId"></param>
        /// <param name="Level"></param>
        public error_log_person InsertMessagePerson(int LogId, int PersonId, int Level, EventHandleFlowEnum eventHandleFlowEnum)
        {
            int id = -1;
            error_log_person ePerson = new error_log_person();
            ePerson.error_log_id = LogId;
            ePerson.message_level = Level;
            ePerson.person_id = PersonId;
            ePerson.insert_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            ePerson.message_flow = eventHandleFlowEnum.ToString();
            id = errorLogPersonService.Insert(ePerson);
            if (id >= 0)
            {
                ePerson.id = id;
            }
            return ePerson;
        }
        public int Update(error_log_person Obj)
        {
            return errorLogPersonService.Update(Obj);
        }

        public int Del(int id)
        {
            int count = errorLogPersonService.Del(id);
            return count;
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public int Del(DateTime start_time, DateTime end_time)
        {
            int count = errorLogPersonService.Del(start_time, end_time);
            return count;
        }
        /// <summary>
        /// 获取已通知人员记录
        /// </summary>
        /// <param name="LogId"></param>
        /// <returns></returns>
        public List<error_log_person> GetErrorLogPersonByLogId(int LogId)
        {
            return errorLogPersonService.GetErrorLogPersonByLogId(LogId);
        }
    }
}
