using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateOrderService
    {
        /// <summary>
        /// 通过编号抓取工单
        /// </summary>
        /// <param name="work_order"></param>
        /// <param name="insert_time"></param>
        /// <returns></returns>
        public UtilizationRateOrder SelectByWorkOrder(string work_order, int line_id)
        {
            try
            {
                string sql;
                UtilizationRateOrder obj = new UtilizationRateOrder();
                sql = "select * from {0} where work_order='{1}' and line_id='{2}' ";
                sql = string.Format(sql, UtilizationRateOrder.TableName, work_order, line_id);
                obj = PostgreHelper.GetSingleEntity<UtilizationRateOrder>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(UtilizationRateOrder obj)
        {
            try
            {
                int count = PostgreHelper.InsertSingleEntity<UtilizationRateOrder>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(UtilizationRateOrder obj)
        {
            try
            {
                int count = PostgreHelper.UpdateSingleEntity<UtilizationRateOrder>(obj);

                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
