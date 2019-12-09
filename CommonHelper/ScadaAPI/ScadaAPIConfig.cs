using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonHelper.ScadaAPI
{
    public class ScadaAPIConfig
    {
        /// <summary>
        /// 是否允许使用Scada APi方式写入
        /// </summary>
        public static bool EnableScadaApi = false;
        /// <summary>
        /// ScadaID
        /// </summary>
        public static string ScadaID = "a57b361c-387c-4919-9e6f-bbd789a6a725";
        /// <summary>
        /// Base Url
        /// </summary>
        public static string ApiBaseUrl = "https://portal-scada-MKP-190702091134-default-space.wise-paas.cn/api/Realdata/set";
        /// <summary>
        /// SSO UserName
        /// </summary>
        public static string UserName = "szkuenzhi@163.com";
        /// <summary>
        /// SSO password
        /// </summary>
        public static string PassWord = "94C4044F@Ss0";
        /// <summary>
        /// SSO url
        /// </summary>
        public static string SsoUrl = "https://portal-sso.wise-paas.cn/v2.0/auth/native";
    }
}
