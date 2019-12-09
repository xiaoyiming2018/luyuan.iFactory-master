using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.Andon
{
    public class ErrorPersonManager
    {
        private ErrorConfigPersonManager ErrorConfigPersonManager = new ErrorConfigPersonManager();
        private PersonManager personManager = new PersonManager();
        private ErrorLogPersonManager errorLogPersonManager = new ErrorLogPersonManager();
        /// <summary>
        /// 获取配置的人员关系及具体的人员对象
        /// </summary>
        /// <param name="ConfigId"></param>
        /// <returns></returns>
        public List<ErrorCfgPerson> GetCfgPersonList(int ConfigId)
        {
            List<ErrorCfgPerson> list = new List<ErrorCfgPerson>();
            List<error_config_person> eCfgPersons = ErrorConfigPersonManager.GetCfgPersons(ConfigId);
            foreach (var item in eCfgPersons)
            {
                person p = personManager.SelectSingleByID(item.person_id);
                ErrorCfgPerson errorCfgPerson = new ErrorCfgPerson();
                errorCfgPerson.eCfgPerson = item;
                errorCfgPerson.ePerson = p;
                list.Add(errorCfgPerson);
            }
            return list;
        }
        /// <summary>
        /// 获取已通知的人员信息
        /// </summary>
        /// <param name="LogId"></param>
        /// <returns></returns>
        public List<ErrorLogPerson> GetErrorLogPersonList(int LogId)
        {
            List<ErrorLogPerson> list = new List<ErrorLogPerson>();
            List<error_log_person> logList = errorLogPersonManager.GetErrorLogPersonByLogId(LogId);
            foreach (var item in logList)
            {
                person p = personManager.SelectSingleByID(item.person_id);
                if (p != null)
                {
                    ErrorLogPerson LogPersonObj = new ErrorLogPerson();
                    LogPersonObj.eLogPerson = item;
                    LogPersonObj.ePerson = p;
                    list.Add(LogPersonObj);
                }
            }
            return list;
        }
        /// <summary>
        /// 向数据库插入已通知的人员信息
        /// </summary>
        /// <param name="LogPersonList"></param>
        public bool InsertMessagePerson(List<ErrorLogPerson>LogPersonList)
        {
            foreach(var item in LogPersonList)
            {
                errorLogPersonManager.Insert(item.eLogPerson);
            }
            return true;
        }
    }
}
