
using System.Collections.Generic;


namespace Advantech.IFactory.CommonLibrary
{
    public class PersonManager
    {
        PersonService US = new PersonService();

        /// <summary>
        /// 根据id查询人员
        /// </summary>
        /// <param name="card_num">员工卡号</param>
        /// <returns></returns>
        public person GetPersonByCardId(string card_num)
        {
            return US.GetPersonByCardId(card_num);
        }
        /// <summary> 
        /// 执行插入人员信息方法
        /// </summary>
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(person obj)
        {
            int count = US.Insert(obj);
            return count;
        }

        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<person> SelectAll(int dept_id=0)
        {
            List<person> objList = US.SelectAll(dept_id);
            return objList;
        }

        /// <summary>
        /// 查询单笔数
        /// </summary>
        /// <param name="user_name">用户名</param>
        /// <returns></returns>
        public  person SelectSingle(string user_name)
        {
            person obj = US.SelectSingle(user_name);
            return obj;
        }
        
        /// <summary>
        /// 根据idnum   查询出等级
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public person SelectSinglenum(string idnum)
        {
            person obj = US.SelectSinglenum(idnum);
            return obj;
        }
        /// <summary>
        /// 根据idnum   查询信息
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public person Selectnum(string cars_num)
        {
            person obj = US.Selectnum(cars_num);
            return obj;
        }
        public person SelectSingleByID(int id = 0)
        {
            person obj = new person();
            if (id > 0)
            {
                obj = US.SelectSingleByID(id);
            }
            else
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 通过id返回视图里面的人员及部门对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PersonDept SelectPersonByID(int id)
        {
            PersonDept obj = new PersonDept();
            if (id > 0)
            {
                obj = US.SelectPersonByID(id);
            }
            else
            {
                obj = null;
            }
            return obj;
        }
        /// <summary>
        /// 更新人员信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(person obj)
        {
            int count = US.Update(obj);
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
