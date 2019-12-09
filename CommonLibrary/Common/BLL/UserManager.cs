using System.Collections.Generic;

namespace Advantech.IFactory.CommonLibrary
{
    public class UserManager
    {
        public UserService US = new UserService();

        public UserManager()//新的不含参数的类
        {
        }
        
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(user obj)
        {
            int count = US.Insert(obj);
            return count;
        }

        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<user> SelectAll(string user_name = null)
        {
            List<user> objList = US.SelectAll(user_name);
            return objList;
        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<UserDept> GetUserDept(string user_name = null)
        {
            List<UserDept> objList = US.GetUserDept(user_name);
            return objList;
        }

        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <returns></returns>
        public user SelectSingle(string user_name)
        {
            user obj = US.SelectSingle(user_name);
            return obj;
        }

        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(user obj)
        {
            int count = US.Update(obj);
            return count;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="user_name">账户</param>
        /// <param name="pass_word">密码</param>
        /// <returns></returns>
        public int UpdatePassWord(string user_name,string pass_word)
        {
            int count = US.UpdatePassWord(user_name, pass_word);
            return count;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="user_name"></param>
        /// <returns></returns>
        public int ResetPassWord(string user_name)
        {
            int count = US.ResetPassWord(user_name);
            return count;
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            int count = US.Del(id);
            return count;
        }
    }
}
