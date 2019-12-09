using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class UserService
    {
        SharedService SS = new SharedService();
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(user obj)
        {
            try
            {
                user newUser = new user();
                newUser.user_name = obj.user_name;
                newUser.user_level = obj.user_level;
                newUser.pass_word = SS.Base64Encryption(Encoding.UTF8, obj.pass_word);
                newUser.create_time = obj.create_time;
                newUser.dept_id = obj.dept_id;
                int count = PostgreHelper.InsertSingleEntity<user>(newUser);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<user> SelectAll(string user_name)
        {
            try
            {
                List<user> objList = new List<user>();
                string sql = null;
                if (string.IsNullOrEmpty(user_name))
                {
                    sql = "select * from fimp.user order by user_level,user_name";
                }
                else
                {
                    sql = "select * fimp.user where user_name like'{0}%' order by user_level,user_name";
                    sql = string.Format(sql, user_name);
                }
                objList=PostgreHelper.GetEntityList<user>(sql);
                
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<UserDept> GetUserDept(string user_name)
        {
            List<UserDept> objList = new List<UserDept>();
            string sql = null;
            try
            {
                if (string.IsNullOrEmpty(user_name))
                {
                    sql = " select * from fimp.user_dept_view order by user_level,user_name";
                }
                else
                {
                    sql = " select * from fimp.user_dept_view where user_name like'{0}%' order by user_level,user_name";
                    sql = string.Format(sql, user_name);
                }
                objList = PostgreHelper.GetEntityList<UserDept>(sql);
            }
            catch (Exception ex)
            {
                
            }
            return objList;
        }
        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <returns></returns>
        public user SelectSingle(string user_name)
        {
            try
            {
                user obj = new user();
                string sql = "select * from fimp.user where user_name='{0}'";
                sql = string.Format(sql, user_name);
                obj = PostgreHelper.GetSingleEntity<user>(sql);
                if(obj !=null)
                {
                    obj.pass_word= SS.Base64Decrypt(Encoding.UTF8, obj.pass_word);
                }
                return obj;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(user obj)
        {
            try
            {
                int count = 0;
                string sql = "update fimp.user set user_name='{0}',user_level='{1}' where id={2}";
                sql = string.Format(sql, obj.user_name, obj.user_level, obj.id);
                count= PostgreHelper.ExecuteNonQuery(sql);
                return count;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user_name">账户</param>
        /// <param name="pass_word">密码</param>
        /// <returns></returns>
        public int UpdatePassWord(string user_name,string pass_word)
        {
            try
            {
                pass_word = SS.Base64Encryption(Encoding.UTF8, pass_word);
                string sql = "update fimp.user set pass_word='{0}' where user_name='{1}'";
                sql = string.Format(sql, pass_word, user_name);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public int ResetPassWord(string user_name)
        {
            try
            {
                string pass_word = SS.Base64Encryption(Encoding.UTF8,"111111");
                string sql = "update  fimp.user set pass_word='{0}' where user_name='{1}'";
                sql = string.Format(sql, pass_word, user_name);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from  fimp.user where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user_name">用户名称</param>
        /// <param name="pass_word">用户密码</param>
        /// <returns></returns>
        public bool Login(string user_name, string pass_word, ref int user_level)
        {
            try
            {
                bool result=false;
                string sql = "select * from fimp.user where user_name='{0}' and pass_word='{1}'";
                sql = string.Format(sql, user_name, SS.Base64Encryption(Encoding.UTF8,pass_word));
                user u= PostgreHelper.GetSingleEntity<user>(sql);
                if(u !=null)
                {
                    result = true;
                    user_level = u.user_level;
                }
                
                return result;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}
