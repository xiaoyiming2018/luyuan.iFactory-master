using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
   public class TagTimeDayManager
    {
        TagTimeDayService tagtimeday = new TagTimeDayService();

        public int Insert(Tag_time_day obj)
        {
            int flag = tagtimeday.Insert(obj);
            return flag;
        }

        public int Update(Tag_time_day obj)
        {
            int flag = tagtimeday.Update(obj);
            return flag;
        }
        public int Update(double duration, int id)
        {
            int count = tagtimeday.Update(duration, id);
            return count;
        }
        public List<Tag_time_day> SelectByDate(string date)
        {
            List<Tag_time_day> tagclass = tagtimeday.SelectByDate(date);
            return tagclass;
        }
        public List<Tag_time_day> SelectByDeviceAndDate(string device_code,string date)
        {
            List<Tag_time_day> tagclass = tagtimeday.SelectByDeviceAndDate(device_code, date);
            return tagclass;
        }
        public int GetId(string machine_code, string tag_code, string date)
        {
            int id = tagtimeday.GetId(machine_code, tag_code, date);
            return id;
        }
    }
}
