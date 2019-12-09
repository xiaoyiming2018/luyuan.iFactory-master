using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class CtAveragedManager
    {
        CtAveragedService ctAveragedService = new CtAveragedService();
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(CtAveraged obj)
        {
            return ctAveragedService.Insert(obj);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(CtAveraged obj, Dictionary<string, object> Dic = null)
        {
            return ctAveragedService.Update(obj, Dic);
        }
        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public List<CtAveraged> SelectAll(string device_code = null)
        {
            return ctAveragedService.SelectAll(device_code);
        }
        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public CtAveraged SelectLastes(string device_code,string tag_code,string work_order)
        {
            return ctAveragedService.SelectLastes(device_code, tag_code, work_order);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int DelByTime(DateTime datetime)
        {
            return ctAveragedService.DelByTime(datetime);
        }
    }
}
