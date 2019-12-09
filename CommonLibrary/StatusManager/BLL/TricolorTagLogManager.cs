using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTagLogManager
    {
        TricolorTaglogService triColorTagLogService = new TricolorTaglogService();

        public int Insert(Tricolor_tag_log TricolorTagLogObject)
        {
            int flag = triColorTagLogService.Insert(TricolorTagLogObject);
            return flag;
        }

        public Tricolor_tag_log SelectNewsLog(string machine_code, string ts)
        {
            Tricolor_tag_log tricolorlog = triColorTagLogService.SelectNewsLog(machine_code, ts);
            return tricolorlog;
        }

        public List<Tricolor_tag_log> SelectDayLog(string datetime)
        {
            List<Tricolor_tag_log> tricolorlogs = triColorTagLogService.SelectDayLog(datetime);
            return tricolorlogs;
        }
        /// <summary>
        /// 获取最近的一条记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public string GetLatestTagCode(string machine_code)
        {
            string tag_code = triColorTagLogService.GetLatestTagCode(machine_code);
            return tag_code;
        }

        /// <summary>
        /// 按照日期删除记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            int count= triColorTagLogService.DeleteByTime(datetime);
            return count;
        }
    }
}
