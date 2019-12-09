using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.WorkOrderManage
{
    /// <summary>
    /// 生产力数据处理
    /// </summary>
    public class ProductivityHelper
    {
        private static ProductivityDailyManager productivityDailyManager = new ProductivityDailyManager();

        /// <summary>
        /// 计算生产力数据
        /// </summary>
        /// <param name="proScheduleList">订单队列</param>
        /// <param name="AddFlag">=1为新增，=0为更新</param>
        public static void CalProductivityData(List<pro_schedule> proScheduleList, int AddFlag)
        {
            try
            {
                Productivity_daily productivitydaily = new Productivity_daily();

                double real_workighour = 0;
                double product_workinghour = 0;
                double real_num = 0;
                foreach (pro_schedule obj in proScheduleList)
                {
                    //获取工单表中的生产力等信息
                    productivitydaily.line_id = obj.line_id;
                    productivitydaily.year = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).Year;
                    productivitydaily.month = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).Month;
                    productivitydaily.day = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).Day;
                    real_num += obj.actual_num;
                    //  productivitydaily.real_num = proScheduleManager.Getactual_num("0001-01-01");
                    TimeSpan TSpan = (TimeSpan)(Convert.ToDateTime(obj.end_time) - Convert.ToDateTime(obj.start_time));
                    real_workighour += (int)TSpan.TotalSeconds;
                    //产出工时 = 标准用时
                    product_workinghour += obj.standard_usetime;
                    productivitydaily.createtime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd HH:mm:ss");
                }
                productivitydaily.real_num = real_num;
                productivitydaily.real_workinghour = real_workighour;
                productivitydaily.product_workinghour = product_workinghour;
                //判断当天是否有数据，有则更新，无则插入
                if (AddFlag == 1)
                    productivityDailyManager.Insert(productivitydaily);
                else
                    productivityDailyManager.Update(productivitydaily);
            }
            catch (Exception ex)
            {
                Console.WriteLine("CalProductivityData error=" + ex.Message);
            }
        }
    }
}
