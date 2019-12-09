using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Advantech.IFactory.CommonHelper.ScadaAPI
{
    public class ScadaAPIHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="SsoUrl"></param>
        /// <param name="DeviceIdAndTagName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static string GetTokenAsync()
        {
            try
            {
                //ServicePointManager.SecurityProtocol =  SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ScadaAPIConfig.SsoUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                byte[] buffer = Encoding.UTF8.GetBytes("{\"username\":\"" + ScadaAPIConfig.UserName + "\",\"password\":\"" + ScadaAPIConfig.PassWord + "\"}");
                Stream newStream = request.GetRequestStream();
                newStream.Write(buffer, 0, buffer.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    TokenResult tokenResult = JsonConvert.DeserializeObject<TokenResult>(reader.ReadToEnd());
                    return tokenResult.accessToken;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetToken() error="+ex.Message);
                return "false";
            }
        }

        /// <summary>
        /// 写值
        /// </summary>
        /// <param name="SsoToken"></param>
        /// <returns></returns>
        public static bool WriteValueAsync( string DeviceId, String TagName, object Value)
        {
            try
            {
                string SsoToken =GetTokenAsync();
                if(SsoToken=="false")
                {
                    SsoToken = GetTokenAsync();//再次获取
                }
                if(SsoToken == "false")
                {
                    return false;//token获取失败
                }
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ScadaAPIConfig.ApiBaseUrl);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", "Bearer " + SsoToken);
                String Body = "{\"scadaId\":\"" + ScadaAPIConfig.ScadaID + "\"," +
                               "\"deviceId\":\"" + DeviceId + "\"," +
                               "\"tagName\":\"" + TagName + "\"," +
                               "\"value\":\"" + Value + "\"," +
                               "\"index\":" + 0 + "}";
                byte[] buffer = Encoding.UTF8.GetBytes(Body);
                request.ContentLength = buffer.Length;
                Stream newStream = request.GetRequestStream();
                newStream.Write(buffer, 0, buffer.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    string ret = reader.ReadToEnd();
                    Console.WriteLine("WriteValueAsync true,DeviceId=" +DeviceId + ",TagName=" + TagName + ",Value=" + Value);
                    if(ret.ToLower().Trim()=="true")
                    {
                        return true;
                    }
                }
                response.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("WriteValue error="+ex.Message);
            }
            return false;
        }

        /// <summary>
        /// 透过ScadaAPI写值到webaccess.云端的结构为 "t" : "ConstPoint\\tag_idle",ConstPoint对应的是deviceid
        /// </summary>
        /// <param name="DeviceId"></param>
        /// <param name="TagName"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public static bool WriteValueAsync(string DeviceIdAndTagName, object Value)
        {
            if (DeviceIdAndTagName.Contains("\\"))
            {
                string[] tagS = DeviceIdAndTagName.Split('\\');
                if(tagS.Length>=2)
                {
                    string DeviceId = tagS[0];
                    string TagName = tagS[1];
                    bool ret= WriteValueAsync(DeviceId, TagName, Value);
                    Console.WriteLine("WriteValueAsync =" + DeviceIdAndTagName +" "+ Value.ToString());
                    return ret;
                }
            }
            return false;
        }
        
    }
}
