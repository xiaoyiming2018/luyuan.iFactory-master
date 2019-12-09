using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TagValueTrendManager
    {
        private TagValueTrendService tagValueTrendService = new TagValueTrendService();
        
        /// <summary>
        /// 获取指定时间段内的标签值列表
        /// </summary>
        /// <returns></returns>
        public List<tag_value_trend> GetTagValueTrends(DateTime Start, DateTime End)
        {
           return tagValueTrendService.GetTagValueTrends(Start, End);
        }
    }
}
