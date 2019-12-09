using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.MachineStatusCollect
{
   public class Tag_time_classManager
    {
        Tag_time_classService tagtimeclass = new Tag_time_classService();

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

        public List<Tag_time_class> Select(string class_no)
        {
            List<Tag_time_class> tagclass= tagtimeclass.Select(class_no);
            return tagclass;
        }

    }
}
