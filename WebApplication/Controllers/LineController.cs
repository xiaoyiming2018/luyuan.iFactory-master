using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using AspNetCoreMvcPager;
using Microsoft.AspNetCore.Http;

namespace WebApplication.Controllers
{
    public class LineController : BaseController
    {
        PlantInfoManager plantInfoManager = new PlantInfoManager();
        LineInfoManager lineinfoManager = new LineInfoManager();
        DeptInfoManager deptInfoManager = new DeptInfoManager();
        UnitInfoManager unitInfoManager = new UnitInfoManager();
        CityInfoManager cityInfoManager = new CityInfoManager();
        //
        // GET: /Line/
        public ActionResult Index(int plant_id,string unit_no, int pageindex = 1, int pagesize = 15)
        {
            if (plant_id > 0)
            {
                var objList = lineinfoManager.SelectAll(plant_id: plant_id, unit_no: unit_no);
                var pagedList = PagedList<line_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
                //var objList = LIM.SelectAll(plant_id: plant_id, unit_no: unit_no);//.ToPagedList((int)page, (int)size);
                //return View(objList);
            }
            else
            {
                var objList = lineinfoManager.SelectAll();
                var pagedList = PagedList<line_info>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
                //return View(objList);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Del()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                int count = lineinfoManager.Del(id);
                if (count > 0)
                {
                    return View("Views/Line/alert.cshtml");
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
        /// 插入更新页面
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                int id = Convert.ToInt32(Request.Query["id"]);
                if (id > 0)
                {
                    ViewBag.id = id;
                    var obj = lineinfoManager.SelectSingle(id);
                    if (obj != null)
                    {
                        unit_info unit = unitInfoManager.SelectSingle(obj.unit_no);
                        if (unit != null)
                        {
                            ViewBag.unit_no = unit.unit_no;
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
        public ActionResult EditHandle(line_info LineModel)
        {
            int count = 0;
            
            if (LineModel.line_id > 0)
            {
                count = lineinfoManager.Update(LineModel);
            }
            else
            {
                count = lineinfoManager.Insert(LineModel);
            }

            if (count > 0)
            {
                return View("Views/Line/alert.cshtml");
            }
            else
            {
                return View("alert-fail");
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
    }
}