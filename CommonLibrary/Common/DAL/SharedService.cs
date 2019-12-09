
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.CommonLibrary
{
    public class SharedService
    {
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encode">编码</param>
        /// <param name="Input">需加密内容</param>
        /// <returns></returns>
        public string Base64Encryption(Encoding encode, string input)
        {
            byte[] bytes = encode.GetBytes(input);
            try
            {
                input = Convert.ToBase64String(bytes);
                return input;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encoding">编码</param>
        /// <param name="Input">需解密内容</param>
        /// <returns></returns>
        public string Base64Decrypt(Encoding encoding, string input)
        {
            string decode = "";
            byte[] bytes = Convert.FromBase64String(input);
            try
            {
                decode = encoding.GetString(bytes);
                return decode;
            }
            catch(Exception ex)
            {
                throw ex;
            }
            
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns>返回1-100随机数</returns>
        public int GetRandom()
        {
            Random r = new Random();
            int num = r.Next(1, 100);
            return num;
        }
        /// <summary>
        /// 企业corpid：每个企业都拥有唯一的corpid，获取此信息可在管理后台“我的企业”－“企业信息”下查看（需要有管理员权限）
        /// </summary>
        //public static string corp_id = ConfigurationManager.ConnectionStrings["corp_id"].ToString();
        /// <summary>
        /// 获取系统当前时间
        /// </summary>
        public static string sysTime = System.DateTime.Now.AddHours(GlobalDefine.SysTimeZone).ToString("yyyy-MM-dd HH:mm:ss");

        /// <summary>
        /// http post方法
        /// </summary>
        /// <param name="url">url地址</param>
        /// <param name="postData">抛送数据</param>
        /// <returns></returns>
        public string PostUrl(string url, string postData)
        {
            string result = "";
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Method = "POST";
                req.Timeout = 80000;
                req.ContentType = "application/json";
                byte[] data = Encoding.UTF8.GetBytes(postData);
                req.ContentLength = data.Length;
                using (Stream reqStream = req.GetRequestStream())
                {
                    reqStream.Write(data, 0, data.Length);

                    reqStream.Close();
                }
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                //获取响应内容
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }


            return result;
        }

        /// <summary>
        /// http get方法
        /// </summary>
        /// <param name="Url">url地址</param>
        /// <param name="paramName">参数名称</param>
        /// <param name="postDataStr">抛送参数</param>
        /// <returns></returns>
        public string GetUrl(string Url)
        {
            string result = null;
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.Method = "GET";
                req.ContentType = "text/html;charset=UTF-8";
                req.Timeout = 800;
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream myResponseStream = resp.GetResponseStream();
                StreamReader reader = new StreamReader(myResponseStream, Encoding.GetEncoding("utf-8"));
                result = reader.ReadToEnd();
                reader.Close();
                myResponseStream.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return result;

        }

        /// <summary>
        /// 对比时间返回时间差（秒）
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns></returns>
        public int GetTimeDifference(object startTime, object endTime)
        {
            TimeSpan TSpan = (TimeSpan)(Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime));
            return Convert.ToInt32(TSpan.TotalSeconds);
        }

        
    }
}
