using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorConfigPersonService
    {
        public int Insert(error_config_person Obj, bool RetId = false)
        {
            int count = PostgreHelper.InsertSingleEntity<error_config_person>(Obj);
            return count;
        }
        /// <summary>
        /// 查询异常配置所关联的人员
        /// </summary>
        /// <param name="config_id">配置id</param>
        /// <returns></returns>
        public List<error_config_person> GetCfgPersons(int config_id)
        {
            List<error_config_person> list = new List<error_config_person>();
            string command=string.Empty;
          
            command = string.Format("Select * from andon.error_config_person where error_config_id='{0}'", config_id);
            
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<error_config_person>(command);
            }
            
            return list;
        }

        /// <summary>
        /// 查询异常配置所关联的人员
        /// </summary>
        /// <param name="config_id">配置id</param>
        /// <returns></returns>
        public List<ErrorCfgPersonView> GetCfgPersonsView(int config_id)
        {
            List<ErrorCfgPersonView> list = new List<ErrorCfgPersonView>();
            string command = string.Empty;
            if (config_id > 0)
            {
                command = string.Format("SELECT * from andon.error_config_person_view where error_config_id='{0}' ", config_id);
            }
            
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<ErrorCfgPersonView>(command);
            }

            return list;
        }
        public List<error_config_person> SelectAll(string class_no)
        {
            try
            {
                List<error_config_person> objList = new List<error_config_person>();
                string sql = null;
                if (!string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from andon.error_config_person where class_no='{0}' order by class_no";
                    sql = string.Format(sql, class_no);
                }
                else
                {
                    sql = "select * from andon.error_config_person order by class_no";
                }
                objList = PostgreHelper.GetEntityList<error_config_person>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public error_config_person SelectSingle(string class_no, int id)
        {
            try
            {
                error_config_person obj = new error_config_person();
                string sql = null;
                if (!string.IsNullOrEmpty(class_no))
                {
                    sql = "select * from andon.error_config_person where class_no='{0}' order by class_no";
                    sql = string.Format(sql, class_no);
                }
                else
                {
                    sql = "select * from andon.error_config_person where id={0} order by class_no";
                    sql = string.Format(sql, id);
                }
                obj = PostgreHelper.GetSingleEntity<error_config_person>(sql);
                
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Update(error_config_person obj)
        {
            try
            {
                int count = 0;
                string sql = "Update andon.error_config_person set class_no='{0}',error_config_id={1},person_level={2},person_id={3} where id={4}";
                sql = string.Format(sql, obj.class_no, obj.error_config_id, obj.person_level, obj.person_id, obj.id);
                count = PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = "delete from andon.error_config_person where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DelErrorConfigid(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_config_person where error_config_id={0}";
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
