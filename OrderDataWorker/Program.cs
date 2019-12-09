using Advantech.IFactory.CommonHelper;
using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.WorkOrderManage
{
    class Program
    {
        static void Main(string[] args)
        {
            ParameterInitail();
            WorkOrderTask.Start_Task(10000);
        }
        /// <summary>
        /// 配置参数初始化
        /// </summary>
        private static void ParameterInitail()
        {
            string connString = "Server=42.159.117.209;Port=5432;Database=9f82c982-67b6-4d3e-8597-3532c9a2c655;User Id=aa7de2d5-fe8a-471f-8c1d-dd1e1a29b690;Password=1atb7c01lu789kg1im0jn6f4j;";
            //string connString = "Server=172.21.128.136;Port=5432;Database=postgres;User Id=postgres;Password=postgres;";
            PostgreBase.connString = connString;

            //mongoDB数据库配置
            string MongoDBConnString = "mongodb://3a16ec06-007a-4944-a4ef-63155cd013ce:zj4ug1ncSldpQaoFXFFeyxr02@42.159.80.108:27017/5eae523e-589d-414b-9ea8-9c3e76f00248";
            MongoDBHelper.connectionstring = MongoDBConnString;
            string MongoDBDataBase = "5eae523e-589d-414b-9ea8-9c3e76f00248";
            MongoDBHelper.databaseName = MongoDBDataBase;

            GlobalDefine.IsLocalMode = false;
            GlobalDefine.MongoDBLogKeepDays = 3;
            GlobalDefine.LogTableKeepDays = 3;
            GlobalDefine.LineBalanceRateKeepDays = 30;

            //拉取系统所在的时区
            AreaInfoManager areaInfoManager = new AreaInfoManager();
            var list = areaInfoManager.SelectAll();
            if (list != null && list.Count > 0)
            {
                GlobalDefine.SysTimeZone = list[0].time_zone;//获取系统设定的时区
            }
        }
    }
}
