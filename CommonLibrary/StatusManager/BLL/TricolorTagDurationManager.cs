using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTagDurationManager
    {
        TricolorTagDurationService duration = new TricolorTagDurationService();

        public int Insert(Tricolor_tag_duration obj)
        {
            int flag = duration.Insert(obj);
            return flag;
        }

        public int Update(Tricolor_tag_duration obj)
        {
            int flag = duration.Update(obj);
            return flag;
        }

        public int SelectDuration(string machinecode)
        {
            int flag = duration.SelectDuration(machinecode);
            return flag;
        }
        /// <summary>
        /// 查找最近在线的所有记录
        /// </summary>
        /// <param name="device_code">设定值为空则查询所有</param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectLatestOnlineStatus(string device_code=null)
        {
            List<Tricolor_tag_duration> obj = duration.SelectAllLatestOnlineStatus(device_code);
            return obj;
        }
        /// <summary>
        /// 查询跨某一时刻点的状态
        /// 比如运行状态从08:00开始，持续到12:00，工单从09:00开始，则计算工单的运行时间则从09:00-12:00
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectInterStatus(DateTime query_time, string tag_code)
        {
            List<Tricolor_tag_duration> obj = duration.SelectInterStatus(query_time, tag_code);
            return obj;
        }
        public int Del(string machine_code, string delTime)
        {
            int count = duration.Del(machine_code, delTime);
            return count;
        }
        /// <summary>
        /// 按照日期删除数据
        /// </summary>
        /// <param name="delTime"></param>
        /// <returns></returns>
        public int DeleteByTime(DateTime delTime)
        {
            int count = duration.DeleteByTime(delTime);
            return count;
        }
        /// <summary>
        /// 获取指定时间段内的状态时间表.时间不传输则抓取当前status=true的记录
        /// </summary>
        /// <param name="machine_code">设备代码</param>
        /// <param name="start_time">开始时间</param>
        /// <param name="end_time">结束时间</param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectTagDurationList(string machine_code, string start_time, string end_time,
                                                                 string tag_code=null, bool status = false)
        {
            return duration.SelectTagDurationList(machine_code, start_time, end_time,tag_code, status);
        }
        /// <summary>
        /// 根据时间抓取指定的记录
        /// </summary>
        /// <param name="start_time"></param>
        /// <param name="end_time"></param>
        /// <param name="tag_code"></param>
        /// <returns></returns>
        public List<Tricolor_tag_duration> SelectTagDurationListByTime(DateTime start_time, DateTime end_time,
                                                                       string tag_code = null)
        {
            return duration.SelectTagDurationListByTime(start_time, end_time, tag_code);
        }
    }
}
