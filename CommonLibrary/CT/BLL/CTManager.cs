using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class CTManager
    {
        CTService ct = new CTService();
        public int Insert(CT obj)
        {
            int count = ct.Insert(obj);
            return count;
        }
        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public List<CT> SelectAll(string device_code = null)
        {
            List<CT> objlist = ct.SelectAll(device_code);
            return objlist;
        }
        /// <summary>
        /// 查询所有工单的数据
        /// </summary>
        /// <param name="device_code">设备信息</param>
        /// <param name="work_order">工单编号</param>
        /// <param name="pn">机种信息</param>
        /// <returns></returns>
        public List<CT> SelectAllByWorkOrder(string device_code,string work_order, string pn,string tag_code)
        {
            List<CT> objlist = ct.SelectAllByWorkOrder(device_code, work_order, pn,tag_code);
            return objlist;
        }
        public CT SelectSingle(string machine_code, string date_time = null)
        {
            CT obj = ct.SelectSingle(machine_code, date_time: date_time);
            return obj;
        }
        public CT SelectSingle(int station_id, string date_time = null)
        {
            CT obj = ct.SelectSingle(station_id, date_time: date_time);
            return obj;
        }
        /// <summary>
        /// 定期删除记录
        /// </summary>
        /// <param name="datetime"></param>
        /// <returns></returns>
        public int DelByTime(DateTime datetime)
        {
            return ct.DelByTime(datetime);
        }
        /// <summary>
        /// ct值发生更新事件委托
        /// </summary>
        public delegate void CtValueChangedDelegate(CT e);
        /// <summary>
        /// ct值发生更新事件
        /// </summary>
        public static event CtValueChangedDelegate CtValueChangedEvent = delegate { };
        /// <summary>
        /// 触发Ct值更新事件
        /// </summary>
        /// <param name="e"></param>
        public static void Raise_CtValueChangedEvent(CT e)
        {
            if (CtValueChangedEvent != null)
            {
                CtValueChangedEvent(e);
            }
        }
    }
}
