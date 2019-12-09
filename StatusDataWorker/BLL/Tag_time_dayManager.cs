using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
   public class Tag_time_dayManager
    {
        Tag_time_dayService tagtimeday = new Tag_time_dayService();

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

        public List<Tag_time_day> Select(string class_no)
        {
            List<Tag_time_day> tagclass = tagtimeday.Select(class_no);
            return tagclass;
        }

    }
}
