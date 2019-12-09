using Advantech.IFactory.CommonLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.Andon
{
    public class AndonMaterialRequestManager
    {
        private MachineInfoManager machineInfoManager = new MachineInfoManager();
        private StationManager stationManager = new StationManager();
        private MaterialRequestInfoManager materialRequestInfoManager = new MaterialRequestInfoManager();
        private CTManager ctManager = new CTManager();
        //private MaterialInfoManager materialInfoManager = new MaterialInfoManager();

        /// <summary>
        /// 物料呼叫增加
        /// </summary>
        /// <param name="materialID">物料id</param>
        /// <param name="count">数量</param>
        /// <param name="deviceTagValueInfo"></param>
        /// <param name="tagAreaAttributeEnum"></param>
        public void AddMaterialRequest(int materialID, int count, DeviceTagValueInfo deviceTagValueInfo, TagAreaAttributeEnum tagAreaAttributeEnum)
        {
            material_request_info request_Info = new material_request_info();
            MachineInfo machine = null;
            station_info station = null;
            bool CheckOk = false;
            CT ct = null;
            //material_info material= materialInfoManager.SelectSingle(materialID);
            if (tagAreaAttributeEnum==TagAreaAttributeEnum.machine_info)
            {
                machine = machineInfoManager.SelectSingle(deviceTagValueInfo.device_id);
                if(machine !=null)
                {
                    var list = materialRequestInfoManager.SelectUnfinishedRequestInfo(materialID, machine.station_id);
                    if (list == null || list.Count == 0)
                    {
                        CheckOk = true;
                        request_Info.station_id = machine.station_id;
                        ct = ctManager.SelectSingle(machine.machine_code);
                    }
                }
            }
            else if(tagAreaAttributeEnum == TagAreaAttributeEnum.station_info)
            {
                station = stationManager.SelectSingle(deviceTagValueInfo.device_id);
                if(station!=null)
                {
                    var list = materialRequestInfoManager.SelectUnfinishedRequestInfo(materialID, station.station_id);
                    if(list==null ||list.Count==0)
                    {
                        CheckOk = true;
                        request_Info.station_id = station.station_id;
                        ct = ctManager.SelectSingle(station.station_id);
                    }
                }
            }
           
           
            if (CheckOk)//队列不存在
            {
                request_Info.createtime = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                request_Info.request_count = count;
                request_Info.material_id = materialID;
                if(ct !=null)
                {
                    request_Info.part_num = ct.wo;//机种
                    request_Info.work_order = ct.pn;//工单
                }
                
                materialRequestInfoManager.Insert(request_Info);
            }
        }
    }
}
