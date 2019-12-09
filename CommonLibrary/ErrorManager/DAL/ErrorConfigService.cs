using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 异常配置
    /// </summary>
    public class ErrorConfigService
    {
        public int Insert(error_config Obj)
        {
            int count = PostgreHelper.InsertSingleEntity<error_config>(Obj);
            return count;
        }
        public int Update(error_config Obj)
        {
            int count = PostgreHelper.UpdateSingleEntity<error_config>(Obj);
            return count;
        }
        /// <summary>
        /// 根据id获取单个对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public error_config GetErrorConfigById(int id)
        {
            string command = string.Empty;

            command = string.Format("Select * from andon.error_config where id='{0}'", id);

            return PostgreHelper.GetSingleEntity<error_config>(command);
        }
      
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程工序，不能为空</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public List<error_config> GetErrorConfig(string system_tag_code,String unit_no, int line_id = -1)
        {

            List<error_config> list = new List<error_config>();
            string command = string.Empty;

            if (line_id >= 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}' and line_id='{2}'", system_tag_code, unit_no, line_id);
            }
            else if (line_id < 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}'", system_tag_code, unit_no);
            }
            
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<error_config>(command);

                return list;
            }
            return null;
        }
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程工序，不能为空</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public error_config GetFirstErrorConfig(string system_tag_code, String unit_no, int line_id = -1)
        {
            error_config obj = new error_config();
            string command = string.Empty;

            if (line_id > 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}' and line_id='{2}' limit 1", system_tag_code, unit_no, line_id);
            }
            else if (line_id < 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}' limit 1", system_tag_code, unit_no);
            }

            if (command != string.Empty)
            {
                obj = PostgreHelper.GetSingleEntity<error_config>(command);

                return obj;
            }
            return null;
        }
        /// <summary>
        /// 获取指定配置
        /// </summary>
        /// <param name="system_tag_code">系统标签类型</param>
        /// <param name="unit_no">制程</param>
        /// <param name="line_id">线别，=-1则不启用</param>
        /// <returns>配置列表</returns>
        public error_config GetErrorConfigByCode(string system_tag_code, String unit_no, int line_id = -1)
        {

            error_config obj = new error_config();
            string command = string.Empty;

            if (system_tag_code != null && line_id >= 0 && unit_no != null && unit_no.Length > 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}' and line_id='{2}'", system_tag_code, unit_no, line_id);
                obj = PostgreHelper.GetSingleEntity<error_config>(command);
                if(obj==null)//查找不到，再向上一级查找
                {
                    command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}'", system_tag_code, unit_no);
                }
            }
            else if (system_tag_code != null && unit_no != null && unit_no.Length > 0)
            {
                command = string.Format("Select * from andon.error_config where system_tag_code='{0}' and unit_no='{1}'", system_tag_code, unit_no);
            }
            
            if (command != string.Empty)
            {
                obj = PostgreHelper.GetSingleEntity<error_config>(command);

                return obj;
            }
            return null;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_config where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<error_config> SelectAll(int line_id, string unit_no = null, string part_num = null)
        {
            List<error_config> list = new List<error_config>();
            string command = string.Empty;
            if (line_id > 0 && unit_no != null && part_num != null)
            {
                command = string.Format("Select * from andon.error_config where line_id='{0}' and unit_no='{1}' and part_num='{2}' order by id", line_id, unit_no, part_num);
            }
            else if (line_id > 0 && unit_no != null)
            {
                command = string.Format("Select * from andon.error_config where line_id='{0}' and unit_no='{1}'  order by id", line_id, unit_no);
            }
            else if (line_id > 0)
            {
                command = string.Format("Select * from andon.error_config where line_id='{0}'  order by id", line_id);
            }
            else if (unit_no != null)
            {
                command = string.Format("Select * from andon.error_config where line_id='{0}' and unit_no='{1}' order by id", line_id, unit_no);
            }
            else
            {
                command = string.Format("Select * from andon.error_config  order by id");
            }

            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<error_config>(command);
            }

            return list;
        }
    }
}
