using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class MaterialRequestInfoManager
    {
        MaterialRequestInfoService MRIS = new MaterialRequestInfoService();
        private CTManager ctManager = new CTManager();

        public List<RequestAndInfo> SelectRequestAndInfoAll(string start_time = null, string end_time = null, string material_name = null)
        {
            List<RequestAndInfo> objList = MRIS.SelectRequestAndInfoAll(start_time, end_time, material_name);
            return objList;
        }

        public List<RequestAndInfo> SelectRequestAndInfoSatation(int station_id)
        {
            List<RequestAndInfo> objList = MRIS.SelectRequestAndInfoSatation(station_id);
            return objList;
        }

        public List<RequestAndInfo> SelectFinishAll(string start_time = null, string end_time = null, string material_name = null)
        {
            List<RequestAndInfo> objList = MRIS.SelectFinishAll(start_time, end_time, material_name);
            return objList;
        }

        public List<material_request_info> SelectAll()
        {
            List<material_request_info> objList = MRIS.SelectAll();
            return objList;
        }

        public material_request_info SelectById(int id)
        {
            material_request_info objList = MRIS.SelectById(id);
            return objList;
        }


        public int Insert(material_request_info obj)
        {
            int count = MRIS.Insert(obj);
            return count;
        }

        public int Update(material_request_info obj)
        {
            int count = MRIS.Update(obj);
            return count;
        }

        public int Del(int id)
        {
            int count = MRIS.Del(id);
            return count;
        }
        /// <summary>
        /// 查找未完成的物料请求
        /// </summary>
        /// <param name="MaterialId"></param>
        /// <param name="StationId"></param>
        /// <returns></returns>
        public List<material_request_info> SelectUnfinishedRequestInfo(int MaterialId, int StationId)
        {
            return MRIS.SelectUnfinishedRequestInfo(MaterialId, StationId);
        }

        /// <summary>
        /// 物料呼叫增加
        /// </summary>
        /// <param name="materialID">物料id</param>
        /// <param name="count">数量</param>
        /// <param name="stationId">站位id</param>
        /// <param name="part_num">机种</param>
        /// <param name="work_order">工单</param>
        public bool AddMaterialRequest(int materialID, int count, int stationId, string part_num = "", string work_order = "")
        {
            if(part_num=="")
            {
                CT ct = ctManager.SelectSingle(stationId);
                if(ct != null)
                {
                    return MRIS.AddMaterialRequest(materialID, count, stationId, ct.pn, ct.wo);
                }
                else
                {
                    return MRIS.AddMaterialRequest(materialID, count, stationId);
                }
            }
            return MRIS.AddMaterialRequest(materialID, count, stationId, part_num, work_order);
        }
    }
}
