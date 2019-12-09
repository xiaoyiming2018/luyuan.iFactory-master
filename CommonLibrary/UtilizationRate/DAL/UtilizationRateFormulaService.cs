using System;
using System.Collections.Generic;
using System.Text;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class UtilizationRateFormulaService
    {
        public UtilizationRateFormula SelectSingle()
        {
            try
            {
                string sql = "select * from oee.utilization_rate_formula order by id";
                UtilizationRateFormula obj = new UtilizationRateFormula();
                
                obj = PostgreHelper.GetSingleEntity<UtilizationRateFormula>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入公式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(UtilizationRateFormula obj)
        {
            try
            {
                string sql = "insert into oee.utilization_rate_formula(run_time_formula,error_time_formula,others_time_formula,boot_time_formula)values('{0}','{1}','{2}','{3}')";
                sql = string.Format(sql, obj.run_time_formula, obj.error_time_formula, obj.others_time_formula, obj.boot_time_formula);
                //int count = Helper.SqlHelper.ExecuteNonQuery(sql);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新公式表
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(UtilizationRateFormula obj)
        {
            try
            {
                string sql = "update oee.utilization_rate_formula set run_time_formula='{0}',error_time_formula='{1}',others_time_formula='{2}',boot_time_formula='{3}' where id={4}";
                sql = string.Format(sql, obj.run_time_formula, obj.error_time_formula, obj.others_time_formula, obj.boot_time_formula, obj.id);
                //int count = Helper.SqlHelper.ExecuteNonQuery(sql);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 将状态转为run,error,others
        /// </summary>
        /// <param name="value"></param>
        /// <returns>状态</returns>
        public string ReturnState(string value)
        {
            string result = null;
            UtilizationRateFormula obj = SelectSingle();
            if (obj != null)
            {
                if (obj.run_time_formula.Contains(value))
                {
                    result = "run";
                }
                else if (obj.error_time_formula.Contains(value))
                {
                    result = "error";
                }
                else if (obj.others_time_formula.Contains(value))
                {
                    result = "others";
                }
                else
                {
                    result = null;
                }
            }
            else
            {
                result = null;
            }
            return result;
        }
    }
}
