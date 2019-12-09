using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class MaterialInfoService
    {
        //查询所有
        public List<material_info> SelectAll(string material_name)
        {
            try
            {
                List<material_info> objList = new List<material_info>();
                string sql = null;
                if (string.IsNullOrEmpty(material_name))
                {
                    sql = "SELECT a.id,a.category_id,b.type_name,a.material_code,a.material_name,a.material_type,a.material_inventory,a.remark,a.createtime FROM fimp.material_info a,fimp.material_category b where a.category_id=b.id order by a.material_code, a.material_code";
                    sql = string.Format(sql);
                }
                else
                {
                    sql = "SELECT a.id,a.category_id,b.type_name,a.material_code,a.material_name,a.material_type,a.material_inventory,a.remark,a.createtime FROM fimp.material_info a,fimp.material_category b where a.category_id=b.id and a.material_name like '%{0}%' or a.material_code like '%{0}%' order by a.material_code, a.material_code";
                    sql = string.Format(sql, material_name);
                }

                objList = PostgreHelper.GetEntityList<material_info>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 查询用户信息列表
        /// </summary>
        /// <param name="user_name">根据用户名查询，可为空</param>
        /// <returns></returns>
        public List<material_info> SelectAllcol()
        {
            try
            {
                List<material_info> objList = new List<material_info>();
                string sql = null;
                sql = "select * from fimp.material_info order by id";
                objList = PostgreHelper.GetEntityList<material_info>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(material_info obj)
        {
            try
            {
                string sql = "insert into fimp.material_info(category_id,material_code,material_name,material_type,material_inventory,remark,createtime)values({0},'{1}','{2}','{3}',{4},'{5}','{6}')";
                sql = string.Format(sql, obj.category_id, obj.material_code, obj.material_name, obj.material_type, obj.material_inventory, obj.remark, DateTime.Now.AddHours(GlobalDefine.SysTimeZone));
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public material_info SelectSingleByID(int id)
        {
            try
            {
                material_info obj = new material_info();
                string sql = "select * from fimp.material_info where id={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<material_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(material_info obj)
        {
            try
            {
                int count = 0;
                string sql = "update fimp.material_info set category_id={0},material_code='{1}',material_name='{2}',material_type='{3}',material_inventory={4},remark='{5}',createtime='{6}' where id={7}";
                sql = string.Format(sql, obj.category_id, obj.material_code, obj.material_name, obj.material_type, obj.material_inventory, obj.remark, obj.createtime,obj.id );
                count = PostgreHelper.ExecuteNonQuery(sql);
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
        /// <param name="id"></param>
        /// <returns></returns>
        public int Del(int id)
        {
            try
            {
                string sql = "delete from  fimp.material_info where id={0}";
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
