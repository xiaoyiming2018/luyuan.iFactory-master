using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Advantech.IFactory.WebCommonLibrary;

namespace WebApplication.Controllers
{
    public class UserController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        WebUserManager UM = new WebUserManager();
        PlantInfoManager plantInfoManager = new PlantInfoManager();
        LineInfoManager lineinfoManager = new LineInfoManager();
        DeptInfoManager deptInfoManager = new DeptInfoManager();
        UnitInfoManager unitInfoManager = new UnitInfoManager();
        CityInfoManager cityInfoManager = new CityInfoManager();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        /// <summary>
        /// 用户列表首页
        /// </summary>
        /// <param name="page">分页页码</param>
        /// <param name="size">每页显示数量</param>
        /// <returns></returns>
        public ActionResult Index(int pageindex = 1, int pagesize = 15)
        {
            string user_name = Request.Query["user_name"];
            if (!string.IsNullOrEmpty(user_name))
            {
                var objList = UM.GetUserDept(user_name);
                var pagedList = PagedList<UserDept>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }
            else
            {
                var objList = UM.GetUserDept();
                var pagedList = PagedList<UserDept>.PageList(pageindex, pagesize, objList);
                ViewBag.model = pagedList.Item2;
                return View(pagedList.Item1);
            }

        }
        /// <summary>
        /// 登录弹框提示，跳出框架
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginAlert()
        {
            return View("alert-login");
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录逻辑处理
        /// </summary>
        /// <returns></returns>
        public ActionResult LoginHandle()
        {
            //获取登录的账户密码
            string user_name = HttpContext.Request.Form["userName"];
            string pass_word = HttpContext.Request.Form["passWord"];
            //获取是否将session延迟为2小时
            bool remember = Convert.ToBoolean(Request.Query["remember"]);
            bool result = UM.Login(user_name, pass_word, remember);
            if (result)
            {
                return Json("Success");
            }
            else
            {
                return Json("Fail");
            }
        }

        /// <summary>
        /// 注销用户
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            HttpContext.Session.Remove("user_name");
            HttpContext.Session.Remove("user_level");
            return View("Views/User/logout.cshtml");
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
                int count = UM.Del(id);
                if (count > 0)
                {
                    ViewBag.Message = "数据删除成功！";
                }
                else
                {
                    ViewBag.Message = "数据删除失败！";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("alert");
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Reset()
        {
            try
            {
                string user_name = Request.Query["user_name"];

                int count = UM.ResetPassWord(user_name);
                if (count > 0)
                {
                    ViewBag.Message = "密码重置成功！";
                }
                else
                {
                    ViewBag.Message = "密码重置失败！";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return View("alert");
        }

        /// <summary>
        /// 编辑页面(添加，修改)
        /// </summary>
        /// <returns></returns>
        public ActionResult Edit()
        {
            try
            {
                string user_name = Request.Query["user_name"];
                if (string.IsNullOrEmpty(user_name))
                {
                    return View();
                }
                else
                {
                    int id = Convert.ToInt32(Request.Query["id"]);
                    ViewBag.id = id;
                    var obj = UM.SelectSingle(user_name);
                    if (obj != null)
                    {
                        dept_info dept = deptInfoManager.SelectSingle(obj.dept_id);
                        if (dept != null)
                        {
                            ViewBag.plant_id = dept.plant_id;
                            plant_info plant = plantInfoManager.SelectSingle(dept.plant_id);
                            if (plant != null)
                            {
                                ViewBag.city_id = plant.city_id;
                                city_info city = cityInfoManager.SelectSingle(plant.city_id);
                                if (city != null)
                                {
                                    ViewBag.area_id = city.area_id;
                                }
                            }
                        }
                    }
                    return View(obj);
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
        public ActionResult EditHandle(user UserModel)
        {
            int count = 0;

            if (UserModel.id > 0)
            {
                count = UM.Update(UserModel);
            }
            else
            {
                count = UM.Insert(UserModel);
            }
            if (count > 0)
            {
                ViewBag.Message = "执行成功！";
            }
            else
            {
               ViewBag.Message = "执行失败！";
            }
            return View("alert");
        }

        /// <summary>
        /// 验证账户是否已经存在
        /// </summary>
        /// <returns></returns>
        public ActionResult Verification()
        {
            int id = 0;
            string user_name = Request.Query["userName"];
            if (!string.IsNullOrEmpty(Request.Query["id"]))
            {
                id = Convert.ToInt32(Request.Query["id"]);
            }
            user obj = UM.SelectSingle(user_name);

            if (id <= 0)
            {
                if (obj == null)
                {
                    return Json("Success");
                }
                else
                {
                    return Json("Fail");
                }
            }
            else
            {
                if (obj == null)
                {
                    return Json("Success");
                }
                else
                {
                    if (id == obj.id)
                    {
                        return Json("Success");
                    }
                    else
                    {
                        return Json("Fail");
                    }
                }
            }
        }

        /// <summary>
        /// 更新密码
        /// </summary>
        /// <returns></returns>
        public ActionResult EditPassWord()
        {
            return View();
        }

        public ActionResult EditPassWordHandle()
        {
            try
            {
                string user_name = HttpContext.Request.Form["user_name"];
                string newpassword = HttpContext.Request.Form["newpassword"];
                int count = UM.UpdatePassWord(user_name, newpassword);
                if (count > 0)
                {
                    HttpContext.Session.Remove("user_name");//Session.Remove("user_name");
                    HttpContext.Session.Remove("user_level");//Session.Remove("user_level");
                    ViewBag.Message = "密码修改成功！";
                }
                else
                {
                    ViewBag.Message = "密码修改失败！";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "密码修改出错！错误信息为="+ ex.Message;
            }
            return View("alert");
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