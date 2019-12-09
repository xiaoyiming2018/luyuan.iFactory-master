using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using iFactory.Op.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace WebApplicationOp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class IotDataController
    {
        /// <summary>
        /// 获取工单信息
        /// </summary>
        /// <param name="StationCode">工位编码</param>
        /// <returns></returns>
        [HttpGet]
        public string GetOrdersInfo(string StationCode)
        {
            string jsonstr;
            JObject jsonMain = new JObject();
            JArray jArray = new JArray();
            try
            {
                CtAveraged ctAveraged = GlobalCfgData.CTInfomation.FirstOrDefault(x => x.device_code == StationCode && x.tag_code == (string)SystemTagCodeEnum.cycle_time.ToString());
                Pro_schedule_machine machine = GlobalCfgData.UnFinishedDevSchedules.FirstOrDefault(x => x.work_order == ctAveraged.work_order && x.machine_code == StationCode);
                double remainNum = machine.standard_num - machine.actual_num;
                double remainTime = Math.Round(remainNum * ctAveraged.averaged_min,1);
                jsonMain.Add("response", "success");
                jArray.Add(new JObject()
                {
                    { "WorkOrder", ctAveraged.work_order },
                    { "PartNumber",  ctAveraged.part_number},
                    { "CycleTime", Math.Round(ctAveraged.averaged_min, 2) },
                    { "RemainNum", remainNum },
                    { "RemainTime", remainTime }
                });
            }
            catch (Exception ex)
            {
                jsonMain = new JObject();
                jArray = new JArray();
                jsonMain.Add("response", "fail");
                jArray.Add(new JObject()
                {
                    { "WorkOrder", "" },
                    { "PartNumber",  ""},
                    { "CycleTime", 0 },
                    { "RemainNum", 0 },
                    { "RemainTime", 0 }
                });
            }

            jsonMain.Add("data", jArray);
            jsonstr = jsonMain.ToString(Newtonsoft.Json.Formatting.None, null);
            return jsonstr;
        }
    }
}
