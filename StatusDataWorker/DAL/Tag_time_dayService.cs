using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.MachineStatusCollect
{
   public class Tag_time_dayService
    {
        public int Insert(Tag_time_day obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tag_time_day>("tag_time_day", obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public int Update(Tag_time_day obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tag_time_day>("tag_time_day", obj.id, obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<Tag_time_day> Select(string class_no)
        {
            try
            {
                string comm = string.Format("select * from '{0} where class_no='{1}' ");
                List<Tag_time_day> tag_Time_Classes_info = PostgreHelper.GetEntityList<Tag_time_day>(comm);
                return tag_Time_Classes_info;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
