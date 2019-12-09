using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;
using System.Data;
using System.Data.SqlClient;
using Advantech.IFactory.CommonHelper;

namespace Advantech.IFactory.CommonLibrary
{
    public class MachineInfoService
    {
        /// <summary>
        /// 获取站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<MachineInfo> SelectAll(MachineInfo obj, string unit_no = null)
        {
            try
            {
                List<MachineInfo> objList = new List<MachineInfo>();
                string sql = null;
                if (string.IsNullOrEmpty(unit_no) && obj == null)
                {
                    sql = "SELECT  * FROM fimp.machine_info order by area_id,city_id,plant_id,unit_no,line_id";
                    sql = string.Format(sql, unit_no);
                }
                else if (!string.IsNullOrEmpty(unit_no))
                {
                    sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where unit_no='{0}'  order by area_id,city_id,plant_id,unit_no,line_id";
                    sql = string.Format(sql, unit_no);
                }
                else
                {
                    if (obj != null)
                    {
                        if (obj.area_id > 0 && obj.city_id <= 0 && obj.plant_id <= 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where area_id={0} order by area_id,city_id,plant_id,unit_no,line_id";
                            sql = string.Format(sql, obj.area_id);
                        }
                        else if (obj.area_id > 0 && obj.city_id > 0 && obj.plant_id <= 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where area_id={0} and city_id={1} order by area_id,city_id,plant_id,unit_no,line_id";
                            sql = string.Format(sql, obj.area_id, obj.city_id);
                        }
                        else if (obj.area_id > 0 && obj.city_id > 0 && obj.plant_id > 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where area_id={0} and city_id={1} and plant_id={2} order by area_id,city_id,plant_id,unit_no,line_id";
                            sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id);
                        }
                        else if (obj.area_id > 0 && obj.city_id > 0 && obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0)
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where area_id={0} and city_id={1} and plant_id={2} and unit_no='{3}' order by area_id,city_id,plant_id,unit_no,line_id";
                            sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no);
                        }
                        else if (obj.area_id > 0 && obj.city_id > 0 && obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id > 0)
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where area_id={0} and city_id={1} and plant_id={2} and unit_no='{3}' and line_id={4} order by area_id,city_id,plant_id,unit_no,line_id";
                            sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id);
                        }
                        else
                        {
                            sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info  order by area_id,city_id,plant_id,unit_no,line_id";
                        }
                    }
                    else
                    {
                        sql = "SELECT  mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info  order by area_id,city_id,plant_id,unit_no,line_id";
                    }
                }

                objList=PostgreHelper.GetEntityList<MachineInfo>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<MachineInfo> Select( int machine_id = 0)
        {
            try
            {
                List<MachineInfo> objList = new List<MachineInfo>();
                string sql = null;
                if (machine_id > 0)
                {
                    sql = "SELECT * FROM fimp.machine_info where machine_id={0}  ";
                    sql = string.Format(sql, machine_id);
                }
                else
                {
                    sql = "SELECT  *  FROM fimp.machine_info ";
                }
                objList = PostgreHelper.GetEntityList<MachineInfo>(sql);
                return objList;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<MachineInfo> SelectByLine(int line_id)
        {
            try
            {
                List<MachineInfo> objList = new List<MachineInfo>();
                string sql = null;
               
                sql = "SELECT * FROM fimp.machine_info where line_id={0}  ";
                sql = string.Format(sql, line_id);
                objList = PostgreHelper.GetEntityList<MachineInfo>(sql);
                return objList;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 将id全部替换为name
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<MachineInfoName> SelectAllName(MachineInfoName obj)
        {
            try
            {
                List<MachineInfoName> objList = new List<MachineInfoName>();
                string sql = null;

                if (obj.city_id > 0 && obj.plant_id <= 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0 && obj.station_id <= 0)
                {
                    //根据城市获取城市对应的所有厂别
                    sql ="SELECT  a.plant_id,c.plant_name_en,c.plant_name_cn " +
                         " FROM fimp.machine_info a,fimp.city_info b, fimp.plant_info c " +
                         " where a.city_id=b.city_id and a.city_id={0} and a.plant_id=c.plant_id and b.city_id=c.city_id " +
                         " group by a.plant_id,c.plant_name_en,c.plant_name_cn " +
                         " order by a.plant_id";
                    sql = string.Format(sql, obj.city_id);

                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);
                    
                    return objList;
                    
                }

                else if (obj.city_id > 0 && obj.plant_id > 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0 && obj.station_id <= 0)
                {
                    //根据厂别获取厂别对应的所有制程
                    sql =" SELECT a.unit_no " +
                         " FROM fimp.machine_info a,fimp.city_info b, fimp.plant_info c " +
                         " where a.city_id=b.city_id and a.city_id={0} and a.plant_id=c.plant_id and b.city_id=c.city_id and a.plant_id={1} " +
                         " group by a.unit_no " +
                         " order by a.unit_no";
                    sql = string.Format(sql, obj.city_id, obj.plant_id);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;
                }
                else if (obj.city_id > 0 && obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0 && obj.station_id <= 0)
                {
                    //根据制程获取制程对应的所有线别
                    sql =" SELECT a.line_id,d.line_name_cn,d.line_name_en " +
                         " FROM fimp.machine_info a,fimp.city_info b, fimp.plant_info c, fimp.line_info d " +
                         " where a.city_id=b.city_id and a.city_id={0} and a.plant_id=c.plant_id and b.city_id=c.city_id and a.plant_id={1} and a.unit_no='{2}' and a.line_id=d.line_id " +
                         " group by a.line_id,d.line_name_cn,d.line_name_en " +
                         " order by line_id";
                    sql = string.Format(sql, obj.city_id, obj.plant_id, obj.unit_no);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;
                }
                else if (obj.city_id > 0 && obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id > 0 && obj.station_id <= 0)
                {
                    //根据线别获取线别对应的所有站位
                    sql =" SELECT a.station_id, e.station_name_cn, e.station_name_en " +
                         " FROM fimp.machine_info a,fimp.city_info b, fimp.plant_info c, fimp.line_info d, fimp.station_info e " +
                         " where a.city_id=b.city_id and a.city_id={0} and a.plant_id=c.plant_id and b.city_id=c.city_id and a.plant_id={1} and a.unit_no='{2}' and a.line_id=d.line_id and a.line_id={3} and c.plant_id=d.plant_id and d.unit_no=a.unit_no and e.station_id = a.station_id and e.unit_no=a.unit_no " +
                         " group by a.station_id, e.station_name_cn, e.station_name_en " +
                         " order by a.station_id";
                    sql = string.Format(sql, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;


                }
                else if (obj.city_id > 0 && obj.plant_id > 0 && !string.IsNullOrEmpty(obj.unit_no) && obj.line_id > 0 && obj.station_id > 0)
                {
                    //根据站位获取站位对应的所有机台
                    sql =" SELECT a.machine_code,a.machine_name_en,a.machine_name_tw,a.machine_name_cn " +
                         " FROM fimp.machine_info a,fimp.city_info b, fimp.plant_info c, fimp.line_info d, fimp.station_info e " +
                         " where a.city_id=b.city_id and a.city_id={0} and a.plant_id=c.plant_id and b.city_id=c.city_id and a.plant_id={1} and a.unit_no='{2}' and a.line_id=d.line_id and a.line_id={3} and c.plant_id=d.plant_id and d.unit_no=a.unit_no and a.station_id={4} and e.station_id = a.station_id and e.unit_no=a.unit_no " +
                         " group by a.machine_code,a.machine_name_en,a.machine_name_tw,a.machine_name_cn " +
                         " order by a.machine_code";
                    sql = string.Format(sql, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id, obj.station_id);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;
                }
                else if (obj.city_id <= 0 && obj.plant_id > 0 && string.IsNullOrEmpty(obj.unit_no) && obj.line_id <= 0 && obj.station_id <= 0)
                {
                    sql = " SELECT a.city_id,b.city_name_en,b.city_name_cn FROM fimp.machine_info a, fimp.city_info b where a.city_id=b.city_id and a.plant_id={0} group by a.city_id,b.city_name_en,b.city_name_cn order by a.city_id ";
                    sql = string.Format(sql, obj.plant_id);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;
                }
                else
                {
                    sql = "SELECT  a.city_id,b.city_name_en,b.city_name_cn FROM fimp.machine_info a, fimp.city_info b where a.city_id=b.city_id group by a.city_id,b.city_name_en,b.city_name_cn order by a.city_id ";
                    sql = string.Format(sql);
                    objList = PostgreHelper.GetEntityList<MachineInfoName>(sql);

                    return objList;
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// 获取站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public List<MachineInfo> SelectMachineNameByStaion(int id)
        {
            try
            {
                List<MachineInfo> objList = new List<MachineInfo>();
                string sql = null;              
                sql = "SELECT mqtt_no,machine_id,area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,status_no,station_id,set_up FROM fimp.machine_info where station_id={0}  order by machine_name_cn";
                sql = string.Format(sql, id);
                objList = PostgreHelper.GetEntityList<MachineInfo>(sql);
                return objList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数据，通过id或者machine_code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MachineInfo SelectSingle(int id = 0, string machine_code = null)
        {
            try
            {
                MachineInfo obj = new MachineInfo();
                string sql = null;
                if (id > 0)
                {
                    sql = "SELECT * FROM fimp.machine_info where machine_id={0}  order by area_id,city_id,plant_id,unit_no,line_id";
                    sql = string.Format(sql, id);
                }
                else if (!string.IsNullOrEmpty(machine_code))
                {
                    sql = "SELECT  * FROM fimp.machine_info where machine_code='{0}'  order by area_id,city_id,plant_id,unit_no,line_id";
                    sql = string.Format(sql, machine_code);
                }
                obj = PostgreHelper.GetSingleEntity<MachineInfo>(sql);
                return obj;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询单笔数据，通过id或者machine_code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public MachineInfo SelectSingleById(int id = 0)
        {
            try
            {
                MachineInfo obj = new MachineInfo();
                string sql = null;
                if (id > 0)
                {
                    sql = "SELECT  * FROM fimp.machine_info where machine_id={0}  order by area_id,city_id,plant_id,unit_no,line_id";
                    sql = string.Format(sql, id);
                    obj = PostgreHelper.GetSingleEntity<MachineInfo>(sql);
                }
            
                return obj;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 插入站别信息数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Insert(MachineInfo obj, int machine_id=0)
        {
            try
            {
                string mqtt_no = obj.mqtt_no;
                if(string.IsNullOrEmpty(mqtt_no))
                {
                    mqtt_no = "A";
                }
                string sql = null;
                if (machine_id <= 0)
                {
                    sql = "Insert into fimp.machine_info(area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,station_id,status_no,mqtt_no,set_up)values({0},{1},{2},'{3}',{4},'{5}','{6}',N'{7}',N'{8}',{9},'{10}','{11}',{12})";
                    sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id, obj.machine_code, obj.machine_name_en, obj.machine_name_tw, obj.machine_name_cn, obj.station_id, obj.status_no, mqtt_no,obj.set_up);
                }
                else
                {
                    sql = "set IDENTITY_INSERT fimp.machine_info ON Insert into fimp.machine_info(area_id,city_id,plant_id,unit_no,line_id,machine_code,machine_name_en,machine_name_tw,machine_name_cn,station_id,status_no,mqtt_no,machine_id,set_up)values({0},{1},{2},'{3}',{4},'{5}','{6}',N'{7}',N'{8}',{9},'{10}','{11}',{12},{13}) set IDENTITY_INSERT fimp.machine_info OFF";
                    sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id, obj.machine_code, obj.machine_name_en, obj.machine_name_tw, obj.machine_name_cn, obj.station_id, obj.status_no, mqtt_no, machine_id,obj.set_up);
                }
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新站别信息
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int Update(MachineInfo obj)
        {
            try
            {
                string sql = "Update fimp.machine_info set area_id={0},city_id={1},plant_id='{2}',unit_no='{3}',line_id={4},machine_code='{5}',machine_name_en='{6}',machine_name_tw=N'{7}',machine_name_cn=N'{8}',status_no='{9}',station_id={10},set_up={11} where machine_id={12}";
                sql = string.Format(sql, obj.area_id, obj.city_id, obj.plant_id, obj.unit_no, obj.line_id, obj.machine_code, obj.machine_name_en, obj.machine_name_tw, obj.machine_name_cn, obj.status_no, obj.station_id, obj.set_up,obj.machine_id);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新站别状态
        /// </summary>
        /// <param name="station_code"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int UpdateState(string machine_code, string status_no)
        {
            try
            {
                string sql = "Update fimp.machine_info set status_no='{0}' where machine_code='{1}'";
                sql = string.Format(sql, status_no, machine_code);
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 更新MQTT状态
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="mqtt_no"></param>
        /// <returns></returns>
        public int UpdateMqttState(string machine_code, string mqtt_no)
        {
            try
            {
                string sql = "Update fimp.machine_info set mqtt_no='{0}' where machine_code='{1}'";
                sql = string.Format(sql, mqtt_no, machine_code);
                int count = PostgreHelper.ExecuteNonQuery(sql);
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
                string sql = null;
                if (id == -1)
                {
                    sql = "TRUNCATE TABLE fimp.machine_info";
                }
                else
                {
                    sql = "delete from fimp.machine_info where machine_id={0}";
                    sql = string.Format(sql, id);
                }
                int count = PostgreHelper.ExecuteNonQuery(sql);
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 更新unit_no
        /// </summary>
        /// <param name="old_unit_no">旧</param>
        /// <param name="unit_no">新</param>
        /// <returns></returns>
        public int UpdateUnitNo(string old_unit_no, string unit_no)
        {
            try
            {
                string sql = "Update fimp.machine_info set unit_no='{0}' where unit_no='{1}'";
                sql = string.Format(sql, unit_no, old_unit_no);
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
