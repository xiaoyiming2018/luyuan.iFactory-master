using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagValueTrendService
    {
        /// <summary>
        /// 获取指定时间段内的标签值列表
        /// </summary>
        /// <returns></returns>
        public List<tag_value_trend> GetTagValueTrends(DateTime Start, DateTime End)
        {
            List<tag_value_trend> list = new List<tag_value_trend>();
            string command = string.Format("Select * from fimp.tag_value_trend where insert_time>='{0}' and insert_time<'{1}'", Start, End);
            list = PostgreHelper.GetEntityList<tag_value_trend>(command);
            return list;
        }
    }
}
