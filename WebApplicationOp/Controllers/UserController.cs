using Advantech.IFactory.CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Http;
using AspNetCoreMvcPager;
using Advantech.IFactory.WebCommonLibrary;
using WebApplicationForOp.Common;

namespace iFactory.Op.Controllers 
{ 
    public class UserController : BaseController
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private ISession _session => _httpContextAccessor.HttpContext.Session;
        WebUserManager UM = new WebUserManager();
        
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        public UserController(IHttpContextAccessor httpContextAccessor) 
        {
            _httpContextAccessor = httpContextAccessor;
        }
        
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            List<user>UserList= UM.SelectAll();
            ViewBag.UserList = UserList;
            return View();
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
        /// 登录逻辑处理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public ActionResult LoginHandle(string userName,string passWord,bool remember)
        {
            bool result = UM.Login(userName, passWord, remember);
            if (result)
            {
                //HttpContext.Session.SetString("user_level", user_level.ToString());
                client_config_info clientCfg = ClientCfgHelper.GetLineAndStation(Request);
                if(clientCfg.station_id<=0 || clientCfg.line_id<=0)
                {
                    if (WebUserManager.Current.GetLevel < UserLevelEnum.Manager)
                    {
                        return Json("config_error");
                    }
                }
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
        /// 验证账户是否已经存在
        /// </summary>
        /// <returns></returns>
        public ActionResult Verification()
        {
            int id = 0;
            string user_name = Request.Query["userName"];
            if(!string.IsNullOrEmpty(Request.Query["id"]))
            {
                id = Convert.ToInt32(Request.Query["id"]);
            }
            user obj = UM.SelectSingle(user_name);

            if (id<=0)
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

	}
}