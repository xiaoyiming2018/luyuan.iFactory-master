using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class TagTimeManager
    {
        private TagTimeDayManager tagTimeDayManager = new TagTimeDayManager();
        private TagTimeClassManager tagTimeClassManager = new TagTimeClassManager();
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private SRPLogManager srpLogManager = new SRPLogManager();

        /// <summary>
        /// tag_time_class班表
        /// </summary>
        /// <param name="device_code">装置的编码，可以为设备编码，也可以为站位编码</param>
        /// <param name="device_id">装置的id</param>
        /// <param name="system_tag_code">系统编码</param>
        /// <param name="class_no">班次代码</param>
        /// <param name="insert_time">插入时间，时间为已经计算过时区后的时间</param>
        /// <param name="duration">持续时间</param>
        /// <param name="AreaAttributeEnum"></param>
        /// <returns></returns>
        public void InserTagTimeClassTable(string device_code, int device_id, 
                                           string system_tag_code, string class_no,
                                           DaysData daysData, double duration,
                                           TagAreaAttributeEnum AreaAttributeEnum)
        {
            try
            {
                int id = tagTimeClassManager.GetId(device_code, system_tag_code, class_no, daysData.TodayDate);

                if (id > 0)
                {
                    tagTimeClassManager.Update(duration, id);//更新班次时间
                }
                else                                         //插入新的班次
                {
                    Tag_time_class objTagTimeClass = new Tag_time_class();
                    objTagTimeClass.machine_code = device_code;
                    objTagTimeClass.tag_code = system_tag_code;
                    objTagTimeClass.year = daysData.RawDate.Year;
                    objTagTimeClass.month = daysData.RawDate.Month;
                    objTagTimeClass.day = daysData.RawDate.Day;
                    objTagTimeClass.date = daysData.RawDate;
                    objTagTimeClass.class_no = class_no;
                    objTagTimeClass.duration = duration;
                    if (AreaAttributeEnum == TagAreaAttributeEnum.machine_info)
                    {
                        MachineInfo machine = machineInfoManager.SelectSingleById(device_id);
                        if (machine != null)
                        {
                            objTagTimeClass.station_id = machine.station_id;
                        }
                    }
                    else if (AreaAttributeEnum == TagAreaAttributeEnum.station_info)
                    {
                        objTagTimeClass.station_id = device_id;
                    }
                    tagTimeClassManager.Insert(objTagTimeClass);
                }

            }
            catch (Exception ex)
            {
                string date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // SM.WriteLog(date, ex);
            }
        }
        
        /// <summary>
        /// 插入tag_time_day日表
        /// </summary>
        /// <param name="device_code">装置的编码，可以为设备编码，也可以为站位编码</param>
        /// <param name="system_tag_code"></param>
        /// <param name="insert_time"></param>
        /// <param name="duration"></param>
        /// <returns></returns>    
        public void InsertTagTimeDayTable(string device_code, int device_id, string system_tag_code,
                                          DaysData daysData, double duration,
                                          TagAreaAttributeEnum AreaAttributeEnum)
        {
            try
            {
                int id = tagTimeDayManager.GetId(device_code, system_tag_code, daysData.TodayDate);

                if (id > 0)
                {
                    tagTimeDayManager.Update(duration, id);
                }
                else
                {
                    Tag_time_day objTagTimeDay = new Tag_time_day();
                    objTagTimeDay.machine_code = device_code;
                    objTagTimeDay.tag_code = system_tag_code;
                    objTagTimeDay.year = daysData.RawDate.Year;
                    objTagTimeDay.month = daysData.RawDate.Month;
                    objTagTimeDay.day = daysData.RawDate.Day;
                    objTagTimeDay.date = daysData.RawDate;
                    objTagTimeDay.duration = duration;

                    if (AreaAttributeEnum == TagAreaAttributeEnum.machine_info)
                    {
                        MachineInfo machine = machineInfoManager.SelectSingleById(device_id);
                        if (machine != null)
                        {
                            objTagTimeDay.station_id = machine.station_id;
                        }
                    }
                    else if (AreaAttributeEnum == TagAreaAttributeEnum.station_info)
                    {
                        objTagTimeDay.station_id = device_id;
                    }
                    tagTimeDayManager.Insert(objTagTimeDay);
                }
            }
            catch (Exception ex)
            {
                string date = System.DateTime.Now.ToString("yyyy-MM-dd");
                // SM.WriteLog(date, ex);
                srpLogManager.Insert("InsertTagTimeDayTable error="+ex.Message);
            }
        }
        /// <summary>
        /// 从Tag_time_class队列转换为Tag_time_day队列
        /// </summary>
        /// <param name="TimeClassList"></param>
        /// <returns></returns>
        public List<Tag_time_day>GetListFromClass(List<Tag_time_class> TimeClassList)
        {
            List<Tag_time_day>  tagTimeDayList = new List<Tag_time_day>();
            foreach(var item in TimeClassList)
            {
                Tag_time_day tagTimeDay = new Tag_time_day();
                tagTimeDay.date = item.date;
                tagTimeDay.day = item.day;
                tagTimeDay.duration = item.duration;
                tagTimeDay.machine_code = item.machine_code;
                tagTimeDay.month = item.month;
                tagTimeDay.id = item.id;
                tagTimeDay.station_id = item.station_id;
                tagTimeDay.tag_code = item.tag_code;
                tagTimeDay.year = item.year;
                tagTimeDayList.Add(tagTimeDay);
            }
            return tagTimeDayList;
        }
    }
}
