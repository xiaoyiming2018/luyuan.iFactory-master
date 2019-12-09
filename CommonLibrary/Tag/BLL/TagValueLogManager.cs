using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagValueLogManager
    {
        TagValueLogService tagValueLogService = new TagValueLogService();
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(tag_value_log obj)
        {
            int count = tagValueLogService.Insert(obj);
            return count;
        }
        /// <summary>
        /// 按照日期删除记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime datetime)
        {
            int count = tagValueLogService.DeleteByTime(datetime);
            return count;
        }
    }
}
