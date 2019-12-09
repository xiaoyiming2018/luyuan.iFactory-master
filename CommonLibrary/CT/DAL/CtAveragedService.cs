using Advantech.IFactory.CommonHelper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class CtAveragedService : IDataService<CtAveraged>
    {
        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(CtAveraged obj)
        {
            try
            {
                int id = PostgreHelper.InsertSingleEntity<CtAveraged>(obj);
                return id;
            }
            catch (Exception ex)
            {
            }
            return -1;
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(CtAveraged obj, Dictionary<string, object> Dic  = null)
        {
            try
            {
                int count = 0;
                if (Dic == null)
                {
                    count = PostgreHelper.UpdateSingleEntity<CtAveraged>(obj);
                }
                else
                {
                    count = PostgreHelper.UpdateSingleEntity<CtAveraged>(CtAveraged.TableName, obj.id, Dic);
                }

                return count;
            }
            catch (Exception ex)
            {
              
            }
            return -1;
        }
        /// <summary>
        /// 查询所有C/T，或者指定设备的
        /// </summary>
        /// <returns></returns>
        public List<CtAveraged> SelectAll(string device_code = null)
        {
            try
            {
                List<CtAveraged> objList = new List<CtAveraged>();
                string comm;
                if (string.IsNullOrEmpty(device_code))
                {
                    comm = string.Format("SELECT * FROM {0} order by insert_time desc", CtAveraged.TableName);
                }
                else
                {
                    comm = string.Format("SELECT * FROM {0} where device_code='{1}' order by insert_time desc", CtAveraged.TableName, device_code);
                }
                objList = PostgreHelper.GetEntityList<CtAveraged>(comm);
                return objList;
            }
            catch (Exception ex)
            {
            }
            return null;
        }
        /// <summary>
        /// 查询指定设备的
        /// </summary>
        /// <returns></returns>
        public CtAveraged SelectLastes(string device_code, string tag_code, string work_order)
        {
            try
            {
                CtAveraged obj = new CtAveraged();
                string comm;

                comm = "SELECT * FROM {0} where device_code='{1}' and tag_code='{2}' and work_order='{3}' order by insert_time desc  limit 1";
                comm = string.Format(comm, CtAveraged.TableName, device_code, tag_code, work_order);

                obj = PostgreHelper.GetSingleEntity<CtAveraged>(comm);
                return obj;
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }
        /// <summary>
        /// 按工单查询
        /// </summary>
        /// <returns></returns>
        public List<CtAveraged> SelectByOrder(string work_order)
        {
            try
            {
                List<CtAveraged> objList = new List<CtAveraged>();
                string comm;

                comm = "SELECT * FROM {0} where work_order='{1}' order by insert_time desc";
                comm = string.Format(comm, CtAveraged.TableName, work_order);

                objList = PostgreHelper.GetEntityList<CtAveraged>(comm);
                return objList;
            }
            catch (Exception ex)
            {

            }
            return null;
        }
        /// <summary>
        /// 按照日期删除
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int DelByTime(DateTime datetime)
        {
            try
            {
                int count = 0;
                string comm;

                comm = "Delete FROM {0} where insert_time<='{1}'";
                comm = string.Format(comm, CtAveraged.TableName, datetime);

                count =PostgreHelper.ExecuteNonQuery(comm);

                return count;
            }
            catch (Exception ex)
            {

            }
            return -1;
        }

        public List<CtAveraged> SelectAll()
        {
            throw new NotImplementedException();
        }

        public CtAveraged SelectByID(int id)
        {
            throw new NotImplementedException();
        }

        public int Del(int id)
        {
            throw new NotImplementedException();
        }
    }
}
