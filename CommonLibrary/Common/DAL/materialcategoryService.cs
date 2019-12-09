using System;
using System.Collections.Generic;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class materialcategoryService
    {
        
        /// <summary>
        /// 执行插入人员信息方法
        /// </summary> 
        /// <param name="obj"></param>
        /// <returns>影响行数</returns>
        public int Insert(material_category obj)
        { 
            try
            {
                string sql = "insert into fimp.material_category(type_code,type_name,unit_no,line_id,remark,createtime)values('{0}','{1}','{2}',{3},'{4}','{5}')";
                sql = string.Format(sql, obj.type_code, obj.type_name, obj.unit_no, obj.line_id, obj.remark, DateTime.Now.AddHours(GlobalDefine.SysTimeZone));
                int count = PostgreHelper.ExecuteNonQuery(sql);
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
        public List<material_category> SelectAll(int line_id, string unit_no )
        {
            try
            {
                List<material_category> objList = new List<material_category>();
                string sql = null;
                if (line_id>0&& !string.IsNullOrEmpty(unit_no))
                {
                    sql = "select * from fimp.material_category where unit_no='{0}' and line_id={1}";
                    sql = string.Format(sql,unit_no,line_id);
                }
                else
                {
                    sql = "select * from fimp.material_category order by id";
                }
                objList = PostgreHelper.GetEntityList<material_category>(sql);
                return objList;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
      

        public material_category SelectSingleID(int area_id)
        {
            try
            {
                material_category obj = new material_category();
                string sql = "select * from fimp.material_category where id={0}";
                sql = string.Format(sql, area_id);
                obj = PostgreHelper.GetSingleEntity<material_category>(sql);
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
        public int Update(material_category obj)
        {
            try
            {
                int count = 0;
                string sql = "update fimp.material_category set type_code='{0}',type_name='{1}',unit_no='{2}',line_id={3},remark='{4}',createtime='{5}' where id={6}";
                sql = string.Format(sql, obj.type_code, obj.type_name, obj.unit_no, obj.line_id, obj.remark, DateTime.Now.AddHours(GlobalDefine.SysTimeZone), obj.id);
                count= PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = "delete from fimp.material_category where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
       
      
    }
}
