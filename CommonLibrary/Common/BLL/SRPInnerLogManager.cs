using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SRPInnerLogManager
    {
        SRPInnerLogService sRPInnerLogService = new SRPInnerLogService();

        public int Insert(srp_inner_log Obj)
        {
            return sRPInnerLogService.Insert(Obj);
        }
        public int Update(srp_inner_log Obj)
        {
            return sRPInnerLogService.Update(Obj);
        }
        /// <summary>
        /// 根据code获取单个对象
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public srp_inner_log GetInnerLogByCode(string code)
        {
            return sRPInnerLogService.GetInnerLogByCode(code);
        }
        /// <summary>
        /// 获取上一次的记录时间
        /// </summary>
        /// <param name="SrpCode"></param>
        /// <returns></returns>
        public string GetLastTimeByCode(string SrpCode)
        {
            srp_inner_log srpLog = GetInnerLogByCode(SrpCode);
            if (srpLog == null)
            {
                srpLog = new srp_inner_log();
                srpLog.srp_code = SrpCode;
                srpLog.last_time = "1990-01-01 00:00:00";//默认的最小值
                srpLog.create_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToUniversalTime();
                if(Insert(srpLog)>0)
                {
                    return srpLog.last_time;
                }
                else
                {
                    return DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString();
                }
            }
            else
            {
                return srpLog.last_time;
            }
        }
        /// <summary>
        /// 更新srp时间
        /// </summary>
        /// <param name="SrpCode"></param>
        /// <param name="UpdateTime">需要更新的时间，最小值则不更新，以现在的时间为主</param>
        public void UpdateSrpTimeByCode(string SrpCode,DateTime UpdateTime)
        {
            srp_inner_log srpLog = GetInnerLogByCode(SrpCode);
            if (srpLog != null)
            {
                if(UpdateTime!= DateTime.MinValue)
                {
                    srpLog.last_time = UpdateTime.ToString("O");//2019-07-10T06:59:33.686Z
                }
                else
                {
                    srpLog.last_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("O");
                }
               
                Update(srpLog);
            }
        }
    }
}
