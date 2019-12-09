using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SystemConfigsService
    {
        public SystemConfigs Get(string config_code)
        {
            SystemConfigs obj = new SystemConfigs();
            string sql = " select * from {0} where config_code='{1}' ";
            sql = string.Format(sql, SystemConfigs.TableName, config_code);
            obj = PostgreHelper.GetSingleEntity<SystemConfigs>(sql);
            return obj;
        }
    }
}
