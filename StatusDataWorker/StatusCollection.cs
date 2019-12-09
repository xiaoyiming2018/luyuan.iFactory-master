using Advantech.IFactory.CommonLibrary;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Advantech.IFactory.MachineStatusCollect
{
    /// <summary>
    /// 当前状态缓存列表(暂未用到)
    /// </summary>
    public  class StatusCollection: List<Tricolor_tag_duration>
    {
        private Mutex mutex = new Mutex();
        private TricolorTagDurationManager tricolorTagDurationManager = new TricolorTagDurationManager();
        public StatusCollection()
        {
            
        }
        /// <summary>
        /// 增加新的记录
        /// </summary>
        /// <param name="DurationItem"></param>
        /// <returns></returns>
        public  bool AddDuration(Tricolor_tag_duration DurationItem)
        {
            if(DurationItem.tag_status==false)//非激活状态不加入
            {
                return false;
            }
            mutex.WaitOne();
            try
            {
                var list = this.Where(x => x.station_id == DurationItem.station_id).ToList();
                if (list != null && list.Count > 0)
                {
                    foreach (var item in list)
                    {
                        this.Remove(item);
                    }
                }
                this.Add(DurationItem);
            }
            catch(Exception ex) { }
            mutex.ReleaseMutex();
            return true;
        }
        /// <summary>
        /// 获取当前状态标签
        /// </summary>
        /// <param name="device_id"></param>
        /// <returns>返回状态的系统标签，未查找到则为空</returns>
        public string GetDuration(int device_id,string device_code)
        {
            string tag_code = string.Empty;
            mutex.WaitOne();
            try
            {
                if (this != null && this.Count > 0)
                {
                    var item = this.FirstOrDefault(x => x.station_id == device_id);
                    if (item != null)
                    {
                        tag_code= item.tag_code;
                    }
                }
                else
                {
                    var list = tricolorTagDurationManager.SelectLatestOnlineStatus(device_code);
                    if(list !=null && list.Count>0)
                    {
                        this.Add(list[0]);//新增进入
                        tag_code = list[0].tag_code;
                    }
                }
            }
            catch(Exception ex) { }
            mutex.ReleaseMutex();
            return tag_code;
        }
    }
}
