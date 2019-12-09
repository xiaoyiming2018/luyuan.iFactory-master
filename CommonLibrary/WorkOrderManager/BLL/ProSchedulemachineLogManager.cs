using System;
using System.Collections.Generic;
using System.Text;


namespace Advantech.IFactory.CommonLibrary
{
    public class ProSchedulemachineLogManager
    {
     
        ProScheduleMachineLogService PSMLS = new ProScheduleMachineLogService();
        /// <summary>
        /// 插入Log日志
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(pro_schedule_machine_log obj)
        {
            int count = PSMLS.Insert(obj);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int Del(string machine_code, string delTime)
        {
            int count = PSMLS.Del(machine_code, delTime);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int DelByTime(DateTime delTime)
        {
            int count = PSMLS.DelByTime(delTime);
            return count;
        }
    }
}
