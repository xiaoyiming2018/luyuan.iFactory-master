using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorConfigPnService
    {
        public int Insert(error_config_pn Obj, bool RetId = false)
        {
            int count = PostgreHelper.InsertSingleEntity<error_config_pn>(Obj);
            return count;
        }
        /// <summary>
        /// 查询异常配置所关联的机种信息
        /// </summary>
        /// <param name="config_id">配置id</param>
        /// <param name="class_no">班别</param>
        /// <param name="part_num">机种编号</param>
        /// <returns></returns>
        public List<error_config_pn> GetCfgPartNum(int config_id, string class_no = null, string part_num = null)
        {
            List<error_config_pn> list = new List<error_config_pn>();
            string command = string.Empty;
            if (config_id > 0 && class_no != null && part_num != null)
            {
                command = string.Format("Select * from andon.error_config_pn where error_config_id='{0}' and class_no='{1}' and part_num='{2}'", config_id, class_no, part_num);
            }
            else if (config_id > 0 && class_no != null)
            {
                command = string.Format("Select * from andon.error_config_pn where error_config_id='{0}' and class_no='{1}'", config_id, class_no);
            }
            else if (config_id > 0 && part_num != null)
            {
                command = string.Format("Select * from andon.error_config_pn where error_config_id='{0}'and part_num='{1}'", config_id, part_num);
            }
            else if (config_id >0) {
                command = string.Format("Select * from andon.error_config_pn where error_config_id='{0}'", config_id);
            }
            else
            {
                command = string.Format("Select * from andon.error_config_pn ");
            }
            if (command != string.Empty)
            {
                list = PostgreHelper.GetEntityList<error_config_pn>(command);
            }

            return list;
        }

        //public error_config_pn SelectSingle(string class_no, int id)
        //{

        //    try
        //    {
        //        error_config_pn obj = new error_config_pn();
        //        string sql = null;
        //        if (!string.IsNullOrEmpty(class_no))
        //        {
        //            sql = "select * from error_config_pn where class_no='{0}' order by class_no";
        //            sql = string.Format(sql, class_no);
        //        }
        //        else
        //        {
        //            sql = "select * from error_config_pn where id={0} order by class_no";
        //            sql = string.Format(sql, id);
        //        }
        //        using (PostgreHelper dr = PostgreHelper.GetReader(sql))
        //        {
        //            if (dr.Read())
        //            {
        //                obj.id = Convert.ToInt32(dr["id"]);
        //                obj.id = Convert.ToInt32(dr["id"]);
        //                obj.error_config_id = Convert.ToInt32(dr["code_no"]);
        //                obj.class_no = dr["class_no"].ToString();
        //                obj.person_level = Convert.ToInt32(dr["person_level"]);
        //                obj.person_id = Convert.ToInt32(dr["person_id"]);
        //            }
        //            else
        //            {
        //                obj = null;
        //            }
        //        }
        //        return obj;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        public int Del(int id)
        {
            try
            {
                string sql = "delete from  andon.error_config_pn where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int DelErrorConfigId(int id)
        {
            try
            {
                string sql = "delete from  andon.error_config_pn where error_config_id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
