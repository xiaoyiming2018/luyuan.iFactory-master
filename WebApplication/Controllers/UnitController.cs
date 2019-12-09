using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;

namespace WebApplication.Controllers
{
    public class UnitController : BaseController
    {
        PlantInfoManager plantInfoManager = new PlantInfoManager();
        LineInfoManager lineinfoManager = new LineInfoManager();
        DeptInfoManager deptInfoManager = new DeptInfoManager();
        UnitInfoManager unitInfoManager = new UnitInfoManager();
        CityInfoManager cityInfoManager = new CityInfoManager();
        //
        // GET: /Unit/
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            var objList = unitInfoManager.SelectAll();//.ToPagedList((int)page, (int)size);
            var pagedList = PagedList<unit_info>.PageList(pageindex, pagesize, objList);
            ViewBag.model = pagedList.Item2;
            return View(pagedList.Item1);
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
                int count = unitInfoManager.Del(id);
                if (count > 0)
                {
                    return View("Views/Unit/alert.cshtml");
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
                    var obj = unitInfoManager.SelectSingle(id: id);
                    if (obj != null)
                    {
                        dept_info dept = deptInfoManager.SelectSingle(obj.dept_id);
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
                    return View(obj);
                }
                else
                {
                    return View();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 编辑处理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult EditHandle(unit_info UnitModel)
        { 
            int count = 0;

            if (UnitModel.seq > 0)
            {
                count = unitInfoManager.Update(UnitModel);
            }
            else
            {
                count = unitInfoManager.Insert(UnitModel);
            }

            if (count > 0)
            {
                return View("Views/Unit/alert.cshtml");
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
    }
}