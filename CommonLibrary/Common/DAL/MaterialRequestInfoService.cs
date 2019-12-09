using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class MaterialRequestInfoService
    {
        //查询所有material_request_info
        public List<material_request_info> SelectAll()
        {
            try
            {
                List<material_request_info> objList = new List<material_request_info>();
                string sql = null;
                sql = "SELECT * FROM fimp.material_request_info order by createtime";
                sql = string.Format(sql);
                objList = PostgreHelper.GetEntityList<material_request_info>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //查询所有material_request_info
        public material_request_info SelectById(int id)
        {
            try
            {
                material_request_info obj = new material_request_info();
                string sql = "SELECT id, material_id,station_id,request_person_id,work_order,part_num,request_count,take_person_id,take_time,remark,createtime FROM fimp.material_request_info where id ={0}";
                sql = string.Format(sql, id);
                obj = PostgreHelper.GetSingleEntity<material_request_info>(sql);
                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //插入数据

        public int Insert(material_request_info obj)
        {
            try
            {
                string sql = "Insert into fimp.material_request_info(material_id,station_id,request_person_id,work_order,part_num,request_count,take_person_id,take_time,remark,createtime FROM fimp.material_request_info)values({0},{1},{2},N'{3}',N'{4}',{5},{6},'{7}','{8}','{9}')";
                sql = string.Format(sql, obj.material_id, obj.station_id, obj.request_person_id, obj.work_order, obj.part_num, obj.request_count, obj.take_person_id, obj.take_time, obj.remark, obj.createtime);
                //int count = PostgreHelper.ExecuteNonQuery(sql);
                int count = PostgreHelper.InsertSingleEntity<material_request_info>(obj);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //更新数据
        public int Update(material_request_info obj)
        {
            try
            {
                int count = 0;
                if (obj.depot_ack_time < Convert.ToDateTime("2000-01-01 00:00:00"))
                {
                    string sql = "Update fimp.material_request_info set take_person_id={0}, take_time='{1}' where id={2}";
                    sql = string.Format(sql, obj.take_person_id,obj.take_time, obj.id);
                    count = PostgreHelper.ExecuteNonQuery(sql);
                }
                else
                {
                    string sql = "Update fimp.material_request_info set depot_ack_time='{0}' where id={1}";
                    sql = string.Format(sql, obj.depot_ack_time, obj.id);
                    count = PostgreHelper.ExecuteNonQuery(sql);
                }
                
                
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //查询所有RequestAndInfo
        public List<RequestAndInfo> SelectRequestAndInfoAll(string start_time = null, string end_time = null, string material_name = null)
        {
            try
            {
                List<RequestAndInfo> objList = new List<RequestAndInfo>();
                string sql = null;
                if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time,end_time,material_name全为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql);
                }
                else if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //material_name不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and (b.material_code like '%{0}%' or b.material_name like '%{0}%') and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, material_name);
                }
                else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //end_time不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime<'{0}' and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, end_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime>'{0}' and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time,end_time不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime>'{0}' and a.createtime<'{1}' and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, end_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //start_time,material_name不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime>'{0}' and (b.material_code like '%{1}%' or b.material_name like '%{1}%') and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, material_name);
                }
                else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //end_time,material_name不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime<'{0}' and (b.material_code like '%{1}%' or b.material_name like '%{1}%') and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, end_time, material_name);
                }
                else
                {
                    //全不为空
                    sql = " SELECT a.id,a.take_person_id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                          " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.createtime>'{0}' and a.createtime<'{1}' and (b.material_code like '%{2}%' or b.material_name like '%{2}%') and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, end_time, material_name);
                }
                
                objList = PostgreHelper.GetEntityList<RequestAndInfo>(sql);

                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //查询所有RequestAndInfo
        public List<RequestAndInfo> SelectRequestAndInfoSatation(int station_id)
        {
            try
            {
                List<RequestAndInfo> objList = new List<RequestAndInfo>();
                string sql = null;
                sql = " SELECT a.id, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime " +
                      " FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c where a.material_id =b.id and a.station_id=c.station_id and a.station_id ={0} and a.depot_ack_time<'1979-01-01 00:00:00' order by a.createtime desc";
                sql = string.Format(sql, station_id);
                objList = PostgreHelper.GetEntityList<RequestAndInfo>(sql);
                
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //查询所有配送完成的
        public List<RequestAndInfo> SelectFinishAll(string start_time = null, string end_time = null, string material_name = null)
        {
            try
            {
                List<RequestAndInfo> objList = new List<RequestAndInfo>();
                string sql = null;
                if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time,end_time,material_name全为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.depot_ack_time>'1979-01-01 00:00:00' null order by a.createtime desc";
                    sql = string.Format(sql);
                }
                else if (string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //material_name不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and (b.material_code like '%{0}%' or b.material_name like '%{0}%') and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, material_name);
                }
                else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //end_time不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime<'{0}' and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, end_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime>'{0}' and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && string.IsNullOrEmpty(material_name))
                {
                    //start_time,end_time不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime>'{0}' and a.createtime<'{1}' and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, end_time);
                }
                else if (!string.IsNullOrEmpty(start_time) && string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //start_time,material_name不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime>'{0}'and (b.material_code like '%{1}%' or b.material_name like '%{1}%') and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, material_name);
                }
                else if (string.IsNullOrEmpty(start_time) && !string.IsNullOrEmpty(end_time) && !string.IsNullOrEmpty(material_name))
                {
                    //end_time,material_name不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime<'{0}' and (b.material_code like '%{1}%' or b.material_name like '%{1}%') and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, end_time, material_name);
                }
                else
                {
                    //全不为空
                    sql = "SELECT a.id, a.take_person_id, d.user_name, c.station_name_cn,b.material_code,b.material_name,b.material_type,a.request_count,a.work_order,a.createtime,a.depot_ack_time " +
                                        "FROM fimp.material_request_info a, fimp.material_info b, fimp.station_info c, fimp.person d where a.material_id =b.id and a.station_id=c.station_id and d.id=a.take_person_id and a.createtime>'{0}' and a.createtime<'{1}' and (b.material_code like '%{2}%' or b.material_name like '%{2}%') and a.depot_ack_time>'1979-01-01 00:00:00' order by a.createtime desc";
                    sql = string.Format(sql, start_time, end_time, material_name);
                }
                objList = PostgreHelper.GetEntityList<RequestAndInfo>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //删除数据
        public int Del(int id)
        {
            try
            {
                string sql = "delete from fimp.material_request_info where id={0}";
                sql = string.Format(sql, id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查找未完成的物料请求
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public List<material_request_info> SelectUnfinishedRequestInfo(int MaterialId, int StationId)
        {
            List<material_request_info> list = new List<material_request_info>();
            string command = string.Format(" Select * from fimp.material_request_info where material_id='{0}' and station_id='{1}' and (take_time is null or take_time<='2001-01-01 00:00:00')", MaterialId, StationId);
            list = PostgreHelper.GetEntityList<material_request_info>(command);
            return list;
        }
        /// <summary>
        /// 物料呼叫增加
        /// </summary>
        /// <param name="materialID">物料id</param>
        /// <param name="count">数量</param>
        /// <param name="stationId">站位id</param>
        /// <param name="part_num">机种</param>
        /// <param name="work_order">工单</param>
        public bool AddMaterialRequest(int materialID, int count, int stationId, string part_num="",string work_order="")
        {
            material_request_info request_Info = new material_request_info();

            var list = SelectUnfinishedRequestInfo(materialID, stationId);
            if (list == null || list.Count == 0)
            {
                request_Info.station_id = stationId;
                request_Info.createtime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                request_Info.request_count = count;
                request_Info.material_id = materialID;
                
                request_Info.part_num = part_num;//机种
                request_Info.work_order = work_order;//工单

                Insert(request_Info);
                return true;
            }
            return false;
        }
    }
}
