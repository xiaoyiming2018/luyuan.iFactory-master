using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class ErrorTypeService
    {
        /// <summary>
        /// 返回插入的id
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int InsertType(error_type Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<error_type>(Obj);
            return id;
        }

        /// <summary>
        /// 获取详细的异常信息
        /// </summary>
        /// <param name="id">异常描述id</param>
        /// <param name="Code">异常编码</param>
        /// <returns></returns>
        public error_type GetType(int id = -1, string Code = "")
        {
            error_type eDetails = new error_type();
            string command = string.Empty;
            if (id >= 0)
            {
                command = string.Format("Select * from andon.error_type where id='{0}'", id);
            }
            else if (Code.Length > 0)
            {
                command = string.Format("Select * from andon.error_type where code_no='{0}'", Code);
            }
            if (command != string.Empty)
            {
                eDetails = PostgreHelper.GetSingleEntity<error_type>(command);
                return eDetails;
            }
            return eDetails;
        }

        public List<error_type> SelectAll()
        {
            try
            {
                List<error_type> objList = new List<error_type>();
                string sql = null;
                sql = "select * from andon.error_type order by id";
                objList = PostgreHelper.GetEntityList<error_type>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Update(error_type Obj)
        {
            int count = PostgreHelper.UpdateSingleEntity<error_type>(Obj);
            return count;
        }

        public int Del(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_type where id={0}";
                sql = string.Format(sql, id);
                count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 返回插入的id
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        public int InsertTypeDetails(error_type_details Obj)
        {
            int id = PostgreHelper.InsertSingleEntity<error_type_details>(Obj);
            return id;
        }
        public List<error_type_details> SelectAllDetails()
        {
            try
            {
                List<error_type_details> objList = new List<error_type_details>();
                string sql = null;
                sql = "select * from andon.error_type_details order by id";
                objList = PostgreHelper.GetEntityList<error_type_details>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<error_type_details> SelectbyErrorid(int type_id)
        {
            try
            {
                List<error_type_details> objList = new List<error_type_details>();
                string sql = null;
                sql = "select * from andon.error_type_details where type_id={0} order by id";
                sql = string.Format(sql, type_id);
                objList = PostgreHelper.GetEntityList<error_type_details>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取详细的异常信息
        /// </summary>
        /// <param name="id">异常描述id</param>
        /// <param name="Code">异常编码</param>
        /// <returns></returns>
        public error_type_details GetTypeDetails(int id=-1,string Code="")
        {
            error_type_details eDetails = new error_type_details();
            string command=string.Empty;
            if (id >= 0)
            {
                command = string.Format("Select * from andon.error_type_details where id='{0}'", id);
            }
            else if (Code.Length > 0)
            {
                command = string.Format("Select * from andon.error_type_details where code_no='{0}'", Code);
            }
            else {
                command = string.Format("Select * from andon.error_type_details");
            }
            if(command !=string.Empty)
            {
                eDetails = PostgreHelper.GetSingleEntity<error_type_details>(command);
                return eDetails;
            }
            return eDetails;
        }
        public int UpdateTypeDetails(error_type_details Obj)
        {
            int count = PostgreHelper.UpdateSingleEntity<error_type_details>(Obj);
            return count;
        }

        public int DelTypeDetails(int id)
        {

            try
            {
                int count = 0;
                string sql = "delete from andon.error_type_details where id={0}";
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
