using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineErrorCodeService
    {
        /// <summary>
        /// 查询所有error_code
        /// </summary>
        /// <returns></returns>
        public List<MachineErrorCode> GetAllErrorCode()
        {
            string command = "Select * from "+ MachineErrorCode.TableName+" order by id";
            List<MachineErrorCode> list = PostgreHelper.GetEntityList<MachineErrorCode>(command);
            return list;
        }

        /// <summary>
        /// 查询指定编码的error_code
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeByNo(string code_no)
        {
            string command = string.Format("Select * from andon.machine_error_code where code_no='{0}'", code_no);
            MachineErrorCode detail = PostgreHelper.GetSingleEntity<MachineErrorCode>(command);
            return detail;
        }
        /// <summary>
        /// 查询指定编码的error_code
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeByValue(string tag_value)
        {
            string command = string.Format("Select * from andon.machine_error_code where tag_value='{0}'", tag_value);
            MachineErrorCode detail = PostgreHelper.GetSingleEntity<MachineErrorCode>(command);
            return detail;
        }
        /// <summary>
        /// 查询指定编码的error_code
        /// </summary>
        /// <returns></returns>
        public MachineErrorCode GetErrorCodeById(int id)
        {
            string command = string.Format("Select * from andon.machine_error_code where id='{0}'", id);
            MachineErrorCode detail = PostgreHelper.GetSingleEntity<MachineErrorCode>(command);
            return detail;
        }

        public List<MachineErrorCode> SelectAll(string code_no)
        {
            try
            {
                List<MachineErrorCode> objList = new List<MachineErrorCode>();
                string sql = null;
                if (!string.IsNullOrEmpty(code_no))
                {
                    sql = "select * from andon.machine_error_code where code_no='{0}' order by code_no";
                    sql = string.Format(sql, code_no);
                }
                else
                {
                    sql = "select * from andon.machine_error_code order by code_no";
                }
                objList = PostgreHelper.GetEntityList<MachineErrorCode>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public MachineErrorCode SelectSingle(string code_no, int id)
        {
            try
            {
                MachineErrorCode obj = new MachineErrorCode();
                string sql = null;
                if (!string.IsNullOrEmpty(code_no))
                {
                    sql = "select * from andon.machine_error_code where code_no='{0}' order by code_no,machine_type";
                    sql = string.Format(sql, code_no);
                }
                else
                {
                    sql = "select * from andon.machine_error_code where id={0} order by code_no";
                    sql = string.Format(sql, id);
                }
                obj = PostgreHelper.GetSingleEntity<MachineErrorCode>(sql);

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Insert(MachineErrorCode obj)
        {
            try
            {
                int count = 0;
                count = PostgreHelper.InsertSingleEntity<MachineErrorCode>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(MachineErrorCode obj)
        {
            try
            {
                int count = 0;
                count = PostgreHelper.UpdateSingleEntity<MachineErrorCode>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Del(int id)
        {
            try
            {
                int count = 0;
                string sql = "delete from andon.machine_error_code where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
