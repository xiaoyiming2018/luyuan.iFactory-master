using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication
{
    [Route("api/[controller]/[action]")]
    public class IoTDataController : Controller
    {
        ProScheduleManager proScheduleManager = new ProScheduleManager();
        ProScheduleMachineManager proScheduleMachineManager = new ProScheduleMachineManager();
        LineInfoManager lineInfoManager = new LineInfoManager();
        StationManager stationManager = new StationManager();

        // 获取当前运行工单号接口
        //请求方式 https://localhost:44352/api/IoTData/GetCurrentOrder?device_code=Y10-S1
        [HttpGet]
        public string GetCurrentOrder(string device_code)
        {
            Pro_schedule_machine deviceSchedule= proScheduleMachineManager.SelectOnLineWorkorder(device_code);
            if (deviceSchedule != null)
            {
                return deviceSchedule.work_order;
            }
            return "";
        }

        //传递工单接口
        //请求方式 https://localhost:44352/api/IoTData/PostOrders
        [HttpPost]
        public async Task<IActionResult> PostOrders([FromBody]List<pro_schedule> Orders)
        {
            List<pro_schedule> list = Orders;
            int count = 0;
            
            if (list !=null && list.Count>0)
            {
                foreach (var item in list)
                {
                    if(proScheduleManager.Insert(item)>0)
                    {
                        ++count;
                    }
                }
                return Ok("success insert count="+count.ToString());
            }
            else
            {
                return NotFound();
            }
        }
    }
}
