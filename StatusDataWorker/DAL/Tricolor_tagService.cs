using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.MachineStatusCollect
{
    public class Tricolor_tagService
    {
        public int Update(string dbname, int id, Tricolor_tag tritag)
        {
            try
            {
                int count = CommonHelper.PostgreHelper.UpdateSingleEntity<Tricolor_tag>("tricolor_tag", id, tritag);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        #region 获取指定设备号的当前状态
        /// <summary>
        /// 获取指定设备号的mqtt协议码（状态）
        /// </summary>
        /// <param name="machine_code"设备编码</param>
        /// <returns></returns>
        public Tricolor_tag GetStatus(string machine_code)
        {
            try
            {
                string comm = string.Format("select * from tricolor_tag where machine_code='{1}'", machine_code);
                Tricolor_tag tricoloer = PostgreHelper.GetSingleEntity<Tricolor_tag>(comm);
                return tricoloer;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public int Insert(Tricolor_tag obj)
        {
            try
            {
                int flag = PostgreHelper.InsertSingleEntity<Tricolor_tag>("tricolor_tag", obj);
                return flag;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

    }
}
