using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class GlobalDefine
    {
        /// <summary>
        /// 系统标签类别列表
        /// </summary>
        public static List<System_tag_type> SysTagType = new List<System_tag_type>();
        /// <summary>
        /// 系统标签详细信息
        /// </summary>
        public static List<SysTagCodeAndType> SysTagCode = new List<SysTagCodeAndType>();
        /// <summary>
        /// 当前运行是否为本地版本，否则为云端版本
        /// </summary>
        public static bool IsLocalMode = true;
        /// <summary>
        /// 系统当前的时区
        /// </summary>
        public static int SysTimeZone = 8;
        /// <summary>
        /// mongodb数据库tag临时数据保留天数
        /// </summary>
        public static int MongoDBLogKeepDays = 3;
        /// <summary>
        /// postgre数据库log之类的数据表保留天数
        /// </summary>
        public static int LogTableKeepDays = 3;
        /// <summary>
        /// postgre数据库中间计算数据表保留天数，如ct表
        /// </summary>
        public static int MiddleTableKeepDays = 30;
        /// <summary>
        /// 线平衡率数据保留天数
        /// </summary>
        public static int LineBalanceRateKeepDays = 7;

        static GlobalDefine()
        {
            TagService tagService = new TagService();
            TagTypeManager tagTypeManager = new TagTypeManager();

            SysTagType = tagTypeManager.GetALL();   //获取所有标签系统类别

            SysTagCode= tagService.GetAllSysTagCode();      //获取所有系统标签

            SystemConfigsManager sysConfigsManager = new SystemConfigsManager();
            SystemConfigs cfg = sysConfigsManager.Get("log_table_keep_days");
            if (cfg != null)
            {
                LogTableKeepDays = cfg.GetIntValue();
            }
            cfg = sysConfigsManager.Get("middle_table_keep_days");
            if (cfg != null)
            {
                MiddleTableKeepDays = cfg.GetIntValue();
            }
            cfg = sysConfigsManager.Get("mongo_table_keep_days");
            if (cfg != null)
            {
                MongoDBLogKeepDays = cfg.GetIntValue();
            }
        }
    }
}
