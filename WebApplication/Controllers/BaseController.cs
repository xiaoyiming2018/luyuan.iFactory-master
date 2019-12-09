using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WebApplication.Controllers
{
    public class BaseController:Controller
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        { 
            byte[] result;
            //WebApplication.Controllers.UserController.Login (WebConfig)
            string actionName = filterContext.ActionDescriptor.DisplayName;
            try
            {
                string[] infos = actionName.Split('.');
                if (infos[2].Contains("UserController")==false && infos[3].ToLower().Contains("Login") == false)//不是登录页面
                {
                    filterContext.HttpContext.Session.TryGetValue("user_name", out result);
                    if (result == null)
                    {
                        filterContext.Result = new RedirectResult("/User/LoginAlert");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            base.OnActionExecuting(filterContext);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
        }
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            return base.OnActionExecutionAsync(context, next);
        }
    }
}
