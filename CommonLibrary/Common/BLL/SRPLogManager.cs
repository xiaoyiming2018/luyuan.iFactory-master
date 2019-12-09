using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SRPLogManager
    {
        private SRPLogService sRPLogService = new SRPLogService();
        /// <summary>
        /// 执行新的日志记录
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(srp_log obj)
        {
            return sRPLogService.Insert(obj);
        }
        /// <summary>
        /// 执行新的日志记录
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(string Content)
        {
            srp_log srp_Log = new srp_log();
            srp_Log.content = Content;
            srp_Log.create_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
            return sRPLogService.Insert(srp_Log);
        }

        /// <summary>
        /// 删除该日期之前的日志记录
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int DeleteLog(DateTime datetime)
        {
            return sRPLogService.Delete(datetime);
        }
    }
}
