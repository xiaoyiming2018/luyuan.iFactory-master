using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using iFactory.Op.Models;
using iFactory.Op.Common;
using Advantech.IFactory.Andon;
using WebApplicationForOp.Common;

namespace iFactory.Op.Controllers
{
    public class AndonController : BaseController
    {
        ErrorLogManager errorLogManager = new ErrorLogManager();
        StationManager stationManager = new StationManager();
        ErrorConfigManager errorConfigManager = new ErrorConfigManager();
        LineInfoManager lineInfoManager = new LineInfoManager();
        PersonManager personManager = new PersonManager();
        ErrorTypeDetailsManager errorTypeDetailsManager = new ErrorTypeDetailsManager();
        SRPLogManager srpLogManager = new SRPLogManager();

        public ActionResult Index()
        {
            ViewBag.AndonWebCall = GlobalCfgData.AndonWebCall;

            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            GlobalCfgData.LoadInitialData(clientCfg.line_id);
            station_info station = GlobalCfgData.StationsList.FirstOrDefault(x => x.station_id == clientCfg.station_id);
            List<error_config> errCfgList = new List<error_config>();
            string line_name = "";

            if (clientCfg.line_id > 0)
            {
                if(clientCfg.line_id == 0)
                {                       
                    errCfgList = errorConfigManager.SelectAll(-1, clientCfg.unit_no);
                }
                else
                {
                    errCfgList = errorConfigManager.SelectAll(clientCfg.line_id);
                    line_name = lineInfoManager.SelectSingle(clientCfg.line_id).line_name_en;
                }
               
                if (errCfgList.Count == 0)                           //如果line下面没有error 则查询unit下的error
                {
                    ViewBag.errCfgList = errorConfigManager.SelectAll(-1, clientCfg.unit_no);
                }
                else
                {
                    ViewBag.errCfgList = errCfgList;
                }
                ViewBag.station_name = station.station_name_en;
                ViewBag.station = station;
                ViewBag.line = line_name;

                return View();
            }
            else
            {
                //Response.ContentType = "charset=utf-8";//不写的话会导致中文乱码
                //Response.WriteAsync("<script type='text/javascript'>alert('请先配置站位信息');location.href='/Configuration/Index';</script>"); //如果没设定站位 则需要先设定站位
                //return View();
                return View("Views/Configuration/alert.cshtml");
            }
        }
        /// <summary>
        /// 周期刷新
        /// </summary>
        /// <returns></returns>
        public ActionResult Refresh()
        {
            ViewBag.AndonWebCall = GlobalCfgData.AndonWebCall;
            List<error_log> errLogList = new List<error_log>();
            List<ErrLogAndCfg> errList = new List<ErrLogAndCfg>();
            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);

            try
            {
                if (GlobalCfgData.OperateMode == 0)
                {
                    ViewBag.station = stationManager.SelectSingle(clientCfg.station_id);
                    errLogList = errorLogManager.GetAllUnfinishedByDeviceId(clientCfg.station_id);
                }
                else
                {
                    List<station_info> stations = new List<station_info>();
                    string line_name = lineInfoManager.SelectSingle(clientCfg.line_id).line_name_en;
                    stations = GlobalCfgData.StationsList.Where(x => x.line_id == clientCfg.line_id).ToList();
                    ViewBag.stationList = stations;
                    errLogList = errorLogManager.GetAllUnfinishedByDeviceId(stations[0].station_id);
                }

                foreach (var item in errLogList)
                {
                    ErrLogAndCfg err = new ErrLogAndCfg();
                    err.ELog = item;
                    err.eConfig = errorConfigManager.GetErrorConfigByCode(item.system_tag_code, clientCfg.unit_no, clientCfg.line_id);
                    if(err.eConfig==null)
                    {
                        err.eConfig = errorConfigManager.GetErrorConfigByCode(item.system_tag_code, clientCfg.unit_no);//以更大范围查找
                    }
                    errList.Add(err);
                }
                return View(errList);
            }
            catch(Exception ex)
            {
                Console.WriteLine("Refresh() error=" + ex.Message);
                return Json("error");
            }
        }
        /// <summary>
        /// 人员签到确认
        /// </summary>
        /// <param name="card_number"></param>
        /// <param name="personnel_name"></param>
        /// <param name="arrival_select_id"></param>
        /// <returns></returns>
        public ActionResult ArrivalPostHandle(string card_number,int log_id)
        {
            person ps;
            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            error_log errLog ;
            error_config errCfg;
            station_info station = GlobalCfgData.StationsList.FirstOrDefault(x => x.station_id == clientCfg.station_id);

            try
            {
                errLog = errorLogManager.GetErrorLogById(log_id);
                errCfg = errorConfigManager.GetErrorConfigByCode(errLog.system_tag_code, clientCfg.unit_no, clientCfg.line_id);
                if (errLog != null && errCfg != null)
                {
                    if (errCfg.check_arrival == (int)ArrivalModeEnum.CardArrival)
                    {
                        if (string.IsNullOrEmpty(card_number))
                        {
                            //return View("Views/Andon/alert-card.cshtml");
                            return Json("card_info_null");
                        }

                        ps = personManager.Selectnum(card_number);
                        if (ps == null)
                        {
                            return Json("card_info_error");
                        }
                        else
                        {
                            errLog.arrival_person_id = ps.id;
                            errLog.arrival_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                        }
                    }
                    else if (errCfg.check_arrival == (int)ArrivalModeEnum.WithoutCardArrival)
                    {
                        errLog.arrival_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    }

                    AndonMainTask.HandlePersonArrival(errLog, errCfg);
                    return Json("success");
                }
            }
            catch(Exception ex)
            {

            }
            
            return Json("fail");

        }
        /// <summary>
        /// 异常确认
        /// </summary>
        /// <param name="error_code">确认的代码</param>
        /// <param name="log_id">对应记录的id</param>
        /// <param name="defectives_count">不良品数量</param>
        /// <returns></returns>
        public ActionResult AckPostHandle(string error_code, string log_id,int defectives_count)
        {
            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            int errId = 0;
            int.TryParse(log_id, out errId);
            error_log errLog;
            error_config errCfg;
            station_info station;

            try
            {
                errLog = errorLogManager.GetErrorLogById(errId);
                errCfg = errorConfigManager.GetErrorConfigByCode(errLog.system_tag_code, clientCfg.unit_no, clientCfg.line_id);
               
                if (errCfg==null)
                {
                    errCfg = errorConfigManager.GetErrorConfigByCode(errLog.system_tag_code, clientCfg.unit_no);//查找更大记录
                }
                station = GlobalCfgData.StationsList.FirstOrDefault(x => x.station_id == clientCfg.station_id);
                if (errLog != null && errCfg != null)
                {
                    srpLogManager.Insert("安灯现场端发送解除，查找到正确的配置和记录"+ errLog.id);
                    if (errCfg.check_arrival > (int)ArrivalModeEnum.NoArrival &&
                       (errLog.arrival_time == null || errLog.arrival_time <= Convert.ToDateTime("2001-01-01 00:00:00")))
                    {
                        return Json("arrival_info_null");
                    }
                    if (errCfg.check_ack == (int)AckModeEnum.CodeAck)//需要代码确认
                    {
                        if (string.IsNullOrEmpty(error_code))
                        {
                            return Json("code_info_null");
                        }
                        error_type_details errType = errorTypeDetailsManager.SelectSingle(error_code);
                        if (errType == null)
                        {
                            return Json("code_not_fount");
                        }
                        else
                        {
                            errLog.error_type_id = errType.id;
                        }
                    }
                    errLog.release_time = DateTime.Now.AddHours(GlobalDefine.SysTimeZone);
                    errLog.defectives_count = defectives_count;
                    AndonMainTask.HandleErrorAck(errLog, errCfg);

                    return Json("success");
                }
            }
            catch(Exception ex)
            {
                srpLogManager.Insert("安灯现场端发送解除出错：" + ex.Message);
            }
            return Json("fail");
        }
        //触发安灯
        public ActionResult ExceptionHandle(string exception_name,string station_name)
        {
            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);

            DeviceTagValueInfo deviceTagValueInfo = new DeviceTagValueInfo();
            deviceTagValueInfo.device_code = clientCfg.machine_code;
            deviceTagValueInfo.device_id = clientCfg.station_id;
            deviceTagValueInfo.tag_code = SystemTagCodeEnum.andon_ack_code.ToString();
            deviceTagValueInfo.insert_time = DateTime.Now;
            // deviceTagValueInfo.tag_value = relieve_select_id + "&" + exception_code;//
            AndonMainTask.HandleErrorAckForTag(clientCfg.machine_code, clientCfg.station_id, deviceTagValueInfo);

            return Redirect("/Andon/Index");
        }
    }
}