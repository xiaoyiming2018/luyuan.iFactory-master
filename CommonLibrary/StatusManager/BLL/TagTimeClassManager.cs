using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
   public class TagTimeClassManager
    {
        TagTimeClassService tagtimeclass = new TagTimeClassService();

        public int Insert(Tag_time_class obj)
        {
            int flag = tagtimeclass.Insert(obj);
            return flag;
        }

        public int Update(Tag_time_class obj)
        {
            int flag = tagtimeclass.Update(obj);
            return flag;
        }

        public int Update(double duration, int id)
        {
            int count = tagtimeclass.Update(duration, id);
            return count;
        }

        public List<Tag_time_class> Select(string class_no)
        {
            List<Tag_time_class> tagclass= tagtimeclass.Select(class_no);
            return tagclass;
        }

        public List<Tag_time_class> SelectByDateAndClass(string class_no, string date)
        {
            List<Tag_time_class> tagclass = tagtimeclass.SelectByDateAndClass(class_no,date);
            return tagclass;
        }

        public List<Tag_time_class> SelectByDateAndClass(string device_code,string class_no, string date)
        {
            List<Tag_time_class> tagclass = tagtimeclass.SelectByDateAndClass(device_code,class_no, date);
            return tagclass;
        }

        public List<Tag_time_class> SelectUnAckLog(string tag_code, string machine_code = "", int station_id = -1)
        {
            List<Tag_time_class> tagclass = tagtimeclass.SelectUnAckLog(tag_code, machine_code, station_id);
            return tagclass;
        }
        public int GetId(string machine_code, string tag_code, string class_no, string date)
        {
            int id = tagtimeclass.GetId(machine_code, tag_code, class_no, date);
            return id;
        }
    }
}
