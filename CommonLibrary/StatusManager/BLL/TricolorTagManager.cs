using System;
using System.Collections.Generic;
using System.Text;


namespace Advantech.IFactory.CommonLibrary
{
    public class TricolorTagManager
    {
        TricolorTagService tricolorservice = new TricolorTagService();

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Tricolor_tag obj)
        {
            int count = tricolorservice.Insert(obj);
            return count;
        }

        /// <summary>
        /// 查询当前machine_code最新一笔数据
        /// </summary>
        /// <param name="station_code">设备编码</param>
        /// <returns></returns>
        public Tricolor_tag SelectSingle(string machine_code,string tag_code,string date_time = null)
        {
            Tricolor_tag obj = tricolorservice.SelectSingle(machine_code,tag_code, date_time: date_time);
            return obj;
        }
        /// <summary>
        /// 查询当前machine_code最新一笔数据
        /// </summary>
        /// <param name="station_code">设备编码</param>
        /// <returns></returns>
        public Tricolor_tag SelectLatestSingle(string machine_code, string tag_code)
        {
            Tricolor_tag obj = tricolorservice.SelectLatestSingle(machine_code, tag_code);
            return obj;
        }

        /// <summary>
        /// 删除machine_code的数据
        /// </summary>
        /// <param name="station_code">设备编码编号</param>
        /// <param name="delTime">删除日期</param>
        /// <returns></returns>
        public int Del(string machine_code, string delTime)
        {
            int count = tricolorservice.Del(machine_code, delTime);
            return count;
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Tricolor_tag obj)
        {
            int count = tricolorservice.Update(obj);
            return count;
        }

    }
}
