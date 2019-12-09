using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class CTLogManager
    {
        CTLogService ctlog = new CTLogService();
        public int Insert(CT_Log obj)
        {
            int count = ctlog.Insert(obj);
            return count;
        }

        public CT_Log SelectNewsLog(string machine_code, string ts)
        {
            CT_Log ct = ctlog.SelectNewsLog(machine_code,ts);
            return ct;
        }
        /// <summary>
        /// 查找最近一笔日志记录
        /// </summary>
        /// <param name="device_code"></param>
        /// <param name="tag_code"></param>
        /// <param name="value">=-1的时候不按照值查询</param>
        /// <param name="start_time">开始查询的时间范围</param>
        /// <returns></returns>
        public CT_Log SelectLatestLog(string device_code, string tag_code,int value=-1,DateTime start_time = default(DateTime))
        {
            CT_Log ct = ctlog.SelectLatestLog(device_code, tag_code, value, start_time);
            return ct;
        }
        /// <summary>
        /// 查找最近一笔日志记录
        /// </summary>
        /// <param name="device_code"></param>
        /// <param name="tag_code"></param>
        /// <param name="value">=-1的时候不按照值查询</param>
        /// <returns></returns>
        public CT_Log SelectLatestLog(string device_code, string tag_code,DateTime datetime, int value)
        {
            CT_Log ct = ctlog.SelectLatestLog(device_code, tag_code, datetime, value);
            return ct;
        }
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            int count = ctlog.DeleteByTime(datetime);
            return count;
        }
    }
}
