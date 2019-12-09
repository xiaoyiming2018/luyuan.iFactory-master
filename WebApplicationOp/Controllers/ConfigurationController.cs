using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Http;
using iFactory.Op.Models;
using iFactory.Op.Common;
using Advantech.IFactory.WebCommonLibrary;
using WebApplicationForOp.Common;

namespace iFactory.Op.Controllers
{
    public class ConfigurationController : BaseController
    {
        MachineInfoManager MIM = new MachineInfoManager();
        ClientConfigInfoManager CCIM = new ClientConfigInfoManager();
        UnitInfoManager UM = new UnitInfoManager();
        StationManager stationManager = new StationManager();

        public IActionResult Index()
        {
            client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);
            if (WebUserManager.Current.GetLevel < UserLevelEnum.Manager)
            {
                if(clientCfg.station_id > 0)//已有配置，权限不足，则返回工单默认页
                {
                    return View("alert-error");
                }
                else//没有配置，权限不足，返回登录页面
                {
                    return View("Views/User/Login.cshtml");
                }
            }
                
            client_config_info identification = new client_config_info();
            //用于更新和插入
            client_config_info cci = new client_config_info();
            //用于获取city_id
            MachineInfoName min = new MachineInfoName();
            MachineInfoName min1 = new MachineInfoName();

            string city_id = Request.Cookies[CookiesEnum.CityId.ToString()];
            string plant_id = Request.Cookies[CookiesEnum.plantId.ToString()];
            string unit_id = Request.Cookies[CookiesEnum.UnitNo.ToString()];
            string line_id = Request.Cookies[CookiesEnum.LineId.ToString()];
            string station_id = Request.Cookies[CookiesEnum.StationId.ToString()];
            string machine_id = Request.Cookies[CookiesEnum.MachineCode.ToString()];

            if (station_id == null)
            {
                //如果查询不到信息，则提交
                ViewBag.city_id = null;
                List<MachineInfoName> insert = new List<MachineInfoName>();
                ViewBag.PlantList = insert;
                ViewBag.UnitList = insert;
                ViewBag.LineList = insert;
                ViewBag.StationList = insert;
                ViewBag.MachineList = insert;
            }
            else
            {
                //如果查询到信息，则显示修改
                ViewBag.city_id =Convert.ToInt32(city_id);

                min1.city_id = Convert.ToInt32(city_id);
                ViewBag.PlantList = MIM.SelectAllName(min1);


                min1.plant_id = Convert.ToInt32(plant_id);
                ViewBag.UnitList = MIM.SelectAllName(min1);

                min1.unit_no = unit_id;
                ViewBag.LineList = MIM.SelectAllName(min1);


                if (line_id == "")
                {
                    cci.line_id = 0;
                    ViewBag.StationList = stationManager.SelectAllByUnit(min1.unit_no);
                }
                else
                {
                    min1.line_id = Convert.ToInt32(line_id);
                    cci.line_id = Convert.ToInt32(line_id);
                    ViewBag.StationList = MIM.SelectAllName(min1);
                }

                min1.station_id = Convert.ToInt32(station_id);
                ViewBag.MachineList = MIM.SelectAllName(min1);

                cci.plant_id = Convert.ToInt32(plant_id);
                cci.unit_no = unit_id;
                
                cci.station_id = Convert.ToInt32(station_id);
                cci.machine_code = machine_id;

            }

            return View(cci);
        }

        public IActionResult Submit()
        {
            string city_id = "";
            string plant_id = "";
            string unit_id = "";
            string line_id = "";
            string station_id = "";
            string machine_id = "";
            
            int result = 1;
            city_id = HttpContext.Request.Form["city_id"];
            plant_id = HttpContext.Request.Form["plant_id"];
            unit_id = HttpContext.Request.Form["unit_no"];
            line_id = HttpContext.Request.Form["line_id"];
            station_id = HttpContext.Request.Form["station_id"];
            machine_id = HttpContext.Request.Form["machine_code"];

            CookieOptions co = new CookieOptions();     
            co.Expires = DateTime.MaxValue;    //设置cookies保存的时间
            Response.Cookies.Append(CookiesEnum.CityId.ToString(), city_id, co);
            Response.Cookies.Append(CookiesEnum.plantId.ToString(), plant_id, co);
            Response.Cookies.Append(CookiesEnum.UnitNo.ToString(), unit_id, co);
            Response.Cookies.Append(CookiesEnum.LineId.ToString(), line_id, co);
            Response.Cookies.Append(CookiesEnum.StationId.ToString(), station_id, co);
            Response.Cookies.Append(CookiesEnum.MachineCode.ToString(), machine_id, co);      //将信息存入到cookies中

            if(line_id!=null)
            {
                GlobalCfgData.StationsList = stationManager.SelectAll(int.Parse(line_id));
            }
            
            if (result > 0)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }
        }

        public ActionResult GetPlantList()
        {
            MachineInfoName min = new MachineInfoName();
            min.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            int i =min.city_id;
            ViewBag.plantList = MIM.SelectAllName(min);
            return View();
        }

        public ActionResult GetUnitList()
        {
            MachineInfoName min = new MachineInfoName();
            min.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            min.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            ViewBag.unitList = MIM.SelectAllName(min);
            return View();
        }

        public ActionResult GetLineList()
        {
            MachineInfoName min = new MachineInfoName();
            min.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            min.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            min.unit_no = HttpContext.Request.Form["unit_no"];
            ViewBag.lineList = MIM.SelectAllName(min);
            return View();
        }

        public ActionResult GetStationList()
        {
            MachineInfoName min = new MachineInfoName();
            min.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            min.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            min.unit_no = HttpContext.Request.Form["unit_no"];
            min.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            if(min.line_id == 0)
            {
                ViewBag.stationList = stationManager.SelectAllByUnit(min.unit_no);
            }
            else
            {
                ViewBag.stationList = MIM.SelectAllName(min);
            }            
            return View();
        }

        public ActionResult GetMachineList()
        {
            MachineInfoName min = new MachineInfoName();
            //min.city_id = Convert.ToInt32(HttpContext.Request.Form["city_id"]);
            //min.plant_id = Convert.ToInt32(HttpContext.Request.Form["plant_id"]);
            //min.unit_no = HttpContext.Request.Form["unit_no"];
            //min.line_id = Convert.ToInt32(HttpContext.Request.Form["line_id"]);
            min.station_id = Convert.ToInt32(HttpContext.Request.Form["station_id"]);
            ViewBag.machineList = MIM.SelectAllName(min);
            return View();
        }

    }
}