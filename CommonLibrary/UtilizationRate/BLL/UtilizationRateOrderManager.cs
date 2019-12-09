using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateOrderManager
    {
        UtilizationRateOrderService service = new UtilizationRateOrderService();

        /// <summary>
        /// 通过编号抓取工单
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="line_id"></param>
        /// <returns></returns>
        public UtilizationRateOrder SelectByWorkOrder(string work_order, int line_id)
        {
            return service.SelectByWorkOrder(work_order, line_id);
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(UtilizationRateOrder obj)
        {
            return service.Insert(obj);
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(UtilizationRateOrder obj)
        {
            return service.Update(obj);
        }
    }
}
