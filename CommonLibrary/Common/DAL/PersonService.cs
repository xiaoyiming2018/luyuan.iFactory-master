using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class PersonService
    {
        /// <summary>
        /// 根据id查询人员
        /// </summary>
        /// <param name="Id">人员id</param>
        /// <returns></returns>
        public person GetPersonById(int Id)
        {
            string command = string.Format("Select * from fimp.Person where id='{0}'", Id);

            person Person = PostgreHelper.GetSingleEntity<person>(command); 
            return Person;
        }
        /// <summary>
        /// 根据id查询人员
        /// </summary>
        /// <param name="card_num">员工卡号</param>
        /// <returns></returns>
        public person GetPersonByCardId(string card_num)
        {
            string command = string.Format("Select * from fimp.Person where card_num='{0}'", card_num);

            person Person = PostgreHelper.GetSingleEntity<person>(command);
            return Person;
        }
        SharedService SS = new SharedService();
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary> 
        /// <param user_name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(person obj)
        {
            try
            {
                int count = PostgreHelper.InsertSingleEntity<person>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param user_name="user_user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<person> SelectAll(int dept_id)
        {
            try
            {
                List<person> objList = new List<person>();
                string sql = null;
                if (dept_id == 0)
                {
                    sql = "select * from fimp.person order by dept_id";
                }
                else
                {
                    sql = "select * from fimp.person where dept_id = {0} order by dept_id";
                    sql = string.Format(sql, dept_id);
                }
                objList = PostgreHelper.GetEntityList<person>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param user_name="user_user_name">用户名</param>
        /// <returns></returns>
        public person SelectSingle(string user_name)
        {
            try
            {
                person obj = new person();
                string sql = "select * from fimp.person where user_name=N'{0}'";
                sql = string.Format(sql, user_name);
                obj = PostgreHelper.GetSingleEntity<person>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据id_num   查询出等级
        /// </summary>
        /// <param user_name="user_name"></param>
        /// <returns></returns>
        public person SelectSinglenum(string id_num)
        {
            try
            {
                person obj = new person();
                string sql = "select * from fimp.person where id_num=N'{0}'";
                sql = string.Format(sql, id_num);
                obj = PostgreHelper.GetSingleEntity<person>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据id_num   查询信息
        /// </summary>
        /// <param user_name="user_name"></param>
        /// <returns></returns>
        public person Selectnum(string card_num)
        {
            try
            {
                person obj = new person();
                string sql = "select * from fimp.person where card_num=N'{0}'";
                sql = string.Format(sql, card_num);
                obj = PostgreHelper.GetSingleEntity<person>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public person SelectSingleByID(int id)
        {
            try
            {
                person obj = new person();
                string sql = "select * from fimp.person where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<person>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 通过id返回视图里面的人员及部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PersonDept SelectPersonByID(int id)
        {
            try
            {
                PersonDept obj = new PersonDept();
                string sql = "select * from fimp.person_dept_view where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<PersonDept>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param user_name="obj"></param>
        /// <returns></returns>
        public int Update(person obj)
        {
            try
            {
                int count = 0;
                count = PostgreHelper.UpdateSingleEntity<person>(obj);
                return count;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param user_name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from  fimp.Person where id={0}";
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
