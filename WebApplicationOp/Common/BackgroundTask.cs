using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace iFactory.Op.Common
{
    public class BackgroundTask : BackgroundService
    {
        public BackgroundTask()
        {
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            pro_schedule deviceSchedule;
            ProScheduleService proScheduleService = (ProScheduleService)ServiceLocator.Instance.GetService<IDataService<pro_schedule>>();
            ProScheduleMachineService devScheduleService= (ProScheduleMachineService)ServiceLocator.Instance.GetService<IDataService<Pro_schedule_machine>>();
            CtAveragedService ctAveragedService = (CtAveragedService)ServiceLocator.Instance.GetService<IDataService<CtAveraged>>();
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    if (proScheduleService != null)
                    {
                        GlobalCfgData.UnFinishedSchedules = proScheduleService.SelectUnFinished();
                    }
                    if (devScheduleService != null)
                    {
                        GlobalCfgData.UnFinishedDevSchedules = devScheduleService.SelectUnFinishedSchedule(null);
                    }
                    if (ctAveragedService != null)
                    {
                        deviceSchedule = GlobalCfgData.UnFinishedSchedules.FirstOrDefault(x => x.order_status == (int)OrderStatusEnum.Excuting);
                        if (deviceSchedule != null)
                        {
                            GlobalCfgData.CTInfomation = ctAveragedService.SelectByOrder(deviceSchedule.work_order);
                        }
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                
                await Task.Delay(1000);//每隔1秒查询所有未完成的工单
            }
        }
    }
}
