using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iFactory.Op.Models;
using iFactory.Op.Common;
using Advantech.IFactory.Andon;

namespace WebApplicationForOp.Common
{
    public class ClientCfgHelper
    {
        /// <summary>
        /// 获取客户端配置信息
        /// </summary>
        /// <param name="Request"></param>
        /// <returns></returns>
        public static client_config_info GetLineAndStation(HttpRequest Request)
        {
            client_config_info cfgInfo = new client_config_info();
            try
            {
                cfgInfo.plant_id = Convert.ToInt32(Request.Cookies[CookiesEnum.plantId.ToString()]);
                cfgInfo.unit_no = Request.Cookies[CookiesEnum.UnitNo.ToString()];
                if (Request.Cookies[CookiesEnum.LineId.ToString()] == "")
                {
                    cfgInfo.line_id = 0;
                }
                else
                {
                    cfgInfo.line_id = Convert.ToInt32(Request.Cookies[CookiesEnum.LineId.ToString()]);
                }
                   
                cfgInfo.station_id = Convert.ToInt32(Request.Cookies[CookiesEnum.StationId.ToString()]);
                cfgInfo.machine_code = Request.Cookies[CookiesEnum.MachineCode.ToString()];
                
            }
            catch(Exception ex)
            {

            }
            return cfgInfo;
        }
    }
}
