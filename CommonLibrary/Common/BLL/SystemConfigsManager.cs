using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SystemConfigsManager
    {
        SystemConfigsService SystemConfigs = new SystemConfigsService();
        /// <summary>
        /// 根据编码读取配置
        /// </summary>
        /// <param name="config_code"></param>
        /// <returns></returns>
        public SystemConfigs Get(string config_code)
        {
            return SystemConfigs.Get(config_code);
        }
    }
}
