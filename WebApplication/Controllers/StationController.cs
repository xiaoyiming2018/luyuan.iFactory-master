using System;
using Advantech.IFactory.CommonLibrary;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Mvc;

namespace WebApplication.Controllers
{
    public class StationController : BaseController
    {
        StationManager SM = new StationManager();
        PlantInfoManager plantInfoManager = new PlantInfoManager();
        LineInfoManager lineinfoManager = new LineInfoManager();
        DeptInfoManager deptInfoManager = new DeptInfoManager();
        UnitInfoManager unitInfoManager = new UnitInfoManager();
        LineInfoManager lineInfoManager = new LineInfoManager();
        CityInfoManager cityInfoManager = new CityInfoManager();
        /// <summary>
        /// 站别信息列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            //station_info obj = new station_info();]
            int station_id = 0;
            var objList = SM.SelectAll(station_id);//.ToPagedList((int)page, 1000);
            var pagedList = PagedList<station_info>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
        }


        /// <summary>
        /// 站别编辑页面（添加，编辑）
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                int station_id = Convert.ToInt32(Request.Query["station_id"]);
                if (station_id > 0)
                {
                    ViewBag.station_id = station_id;
                    var obj = SM.SelectSingle(station_id);
                    if (obj != null)
                    {
                        unit_info unit = unitInfoManager.SelectSingle(obj.unit_no);
                        if (unit != null)
                        {
                            dept_info dept = deptInfoManager.SelectSingle(unit.dept_id);
                            if (dept != null)
                            {
                                ViewBag.dept_id = dept.dept_id;
                                plant_info plant = plantInfoManager.SelectSingle(dept.plant_id);
                                if (plant != null)
                                {
                                    ViewBag.plant_id = plant.plant_id;
                                    city_info city = cityInfoManager.SelectSingle(plant.city_id);
                                    if (city != null)
                                    {
                                        ViewBag.city_id = city.city_id;
                                        ViewBag.area_id = city.area_id;
                                    }
                                }
                            }
                        }
                    }
                    return View(obj);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                return View("alert-fail");
            }
        }
        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditHandle(station_info StationModel)
        {
            int count = 0;
           
            if(StationModel.convert_multiplier == 0)
            {
                StationModel.convert_multiplier = 1;
            }
           
            if (StationModel.station_id > 0)
            {
                count = SM.Update(StationModel);
            }
            else
            {
                count = SM.Insert(StationModel);
            }

            if (count > 0)
            {
                return View("Views/Station/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
            }
        }

        /// <summary>
        /// 验证数据是否存在
        /// </summary>
        /// <returns></returns>
        //public ActionResult Verification()
        //{
        //    int id = 0;
        //    string station_code = HttpContext.Request.Form["station_code"];
        //    if (!string.IsNullOrEmpty(HttpContext.Request.Form["id"]))
        //    {
        //        id = Convert.ToInt32(HttpContext.Request.Form["id"]);
        //    }
        //    station_info obj = SM.SelectSingle(station_code: station_code);

        //    if (id <= 0)
        //    {
        //        if (obj == null)
        //        {
        //            return Json("Success");
        //        }
        //        else
        //        {
        //            return Json("Fail");
        //        }
        //    }
        //    else
        //    {
        //        if (obj == null)
        //        {
        //            return Json("Success");
        //        }
        //        else
        //        {
        //            if (id == obj.station_id)
        //            {
        //                return Json("Success");
        //            }
        //            else
        //            {
        //                return Json("Fail");
        //            }
        //        }
        //    }
        //}

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int count = SM.Del(id);
                if (count > 0)
                {
                    return View("Views/Station/alert.cshtml");
                }
                else
                {
                    return View("alert-fail");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取城市信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetCityList(int area_id)
        {
            ViewBag.cityList = cityInfoManager.SelectAll(area_id);
            return View();
        }
        /// <summary>
        /// 获取厂别信息
        /// </summary>
        /// <returns></returns>
        public ActionResult GetPlantList(int city_id)
        {
            ViewBag.plantList = plantInfoManager.SelectAll(city_id);
            return View();
        }

        /// <summary>
        /// 获取部门清单
        /// </summary>
        /// <param name="plant_id"></param>
        /// <returns></returns>
        public ActionResult GetDeptList(int plant_id)
        {
            ViewBag.deptList = deptInfoManager.SelectAll(plant_id);
            return View();
        }
        /// <summary>
        /// 获取制程清单
        /// </summary>
        /// <param name="plant_id"></param>
        /// <returns></returns>
        public ActionResult GetUnitList(int dept_id)
        {
            ViewBag.unitList = unitInfoManager.SelectAll(dept_id);
            return View();
        }
        /// <summary>
        /// 获取线别清单
        /// </summary>
        /// <param name="plant_id"></param>
        /// <returns></returns>
        public ActionResult GetLineList(string unit_no)
        {
            ViewBag.lineList = lineinfoManager.SelectAll(0,unit_no);
            return View();
        }
    }
}