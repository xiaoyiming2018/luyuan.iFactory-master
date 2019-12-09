using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tag_time_classService
    {
        public int Insert(Tag_time_class obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tag_time_class>("tag_time_class", obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update(Tag_time_class obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tag_time_class>("tag_time_class", obj.id, obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Tag_time_class> Select(string class_no)
        {
            try
            {
                string comm = string.Format("select * from '{0} where class_no='{1}' ");
                List<Tag_time_class> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_class>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
