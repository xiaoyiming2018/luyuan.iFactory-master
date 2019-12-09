using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Advantech.IFactory.CommonLibrary;

namespace Advantech.IFactory.WebCommonLibrary
{
    public class WebUserManager: UserManager
    {
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="user_name">用户名称</param>
        /// <param name="pass_word">用户密码</param>
        /// <param name="remember">是否需要长时间登录</param>
        /// <returns></returns>
        public bool Login(string user_name, string pass_word, bool remember = false)
        {
            int user_level = 0;
            bool result = US.Login(user_name, pass_word,ref user_level);
            if (result)
            {
                var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
                var _session = _httpContextAccessor.HttpContext.Session;
                _session.SetString("user_name", user_name);
                _session.SetString("user_level", user_level.ToString());
                //假如选中记住用户名，那么Session保持30分钟
                if (remember)
                {
                    
                }
            }
            return result;
        }
        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
            var _session = _httpContextAccessor.HttpContext.Session;
            return _session.GetString(key);
        }
        /// <summary>
        /// 获取当前session中的权限
        /// </summary>
        /// <returns></returns>
        public UserLevelEnum GetLevel
        {
            get
            {
                int level = 0;
                var _httpContextAccessor = ServiceLocator.Instance.GetService<IHttpContextAccessor>();
                var _session = _httpContextAccessor.HttpContext.Session;
                string tmp = _session.GetString("user_level");
                int.TryParse(tmp, out level);
                return (UserLevelEnum)level;
            }
        }
        /// <summary>
        /// 当前用户
        /// </summary>
        public static WebUserManager Current
        {
            get
            {
                return new WebUserManager();
            }
        }
    }
}
