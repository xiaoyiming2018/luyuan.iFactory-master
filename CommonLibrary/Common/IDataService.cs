using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    /// <summary>
    /// 数据层统一访问继承接口
    /// </summary>
    public interface IDataService<T>
    {
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="Obj"></param>
        /// <returns></returns>
        int Insert(T Obj);
        /// <summary>
        /// 更新对象或者根据字段更新
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        int Update(T obj, Dictionary<string, object> dic = null);
        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        List<T> SelectAll();
        /// <summary>
        /// 根据id进行查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T SelectByID(int id);
        /// <summary>
        /// 根据id进行删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        int Del(int id);
    }
}
