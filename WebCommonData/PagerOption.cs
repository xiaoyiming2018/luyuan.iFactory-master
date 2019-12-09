using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.WebCommonLibrary
{
    public class PagerOption
    {
        //当前页
        public int PageIndex { get; set; }
        //每页数据条数
        public int PageSize { get; set; }

        //显示的页面个数
        public int CountNum { get; set; }
        //总数据条数
        public int ItemCount { get; set; }
        //总页数
        public int TotalPage
        {
            get
            {
                return ItemCount % PageSize > 0 ? (ItemCount / PageSize + 1) : ItemCount / PageSize;
            }
        }
        //分页请求的地址
        public string Url { get; set; }

        //接收url参数
        public IQueryCollection Query { get; set; }
    }
}
