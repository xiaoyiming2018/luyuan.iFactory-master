using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tag_durationService
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(Tricolor_tag_duration obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tricolor_tag_duration>("tricolor_tag_duration", obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(Tricolor_tag_duration obj)
        {
            try
            {
                int flag = PostgreHelper.UpdateSingleEntity<Tricolor_tag_duration>("tricolor_tag_duration", obj.id, obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 查询设备的持续时间
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int SelectDuration(string machinecode)
        {
            try
            {
                Tricolor_tag_duration durationinfo = new Tricolor_tag_duration();
                durationinfo = PostgreHelper.GetSingleEntity<Tricolor_tag_duration>(machinecode);
                return durationinfo.duration;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
