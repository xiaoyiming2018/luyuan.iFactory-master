using log4net;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Xml;

namespace Advantech.IFactory.CommonHelper
{
    /// <summary>
    /// MQTT连接配置信息
    /// </summary>
    public class WebAccessConfig
    {
        public static string UserName = "admin";
        public static string Password = "";
        public static string Ip = "172.21.136.38";
        public static string ProjectName = "";
    }

    public class WebaccessHelper
    {
        private string _WebAccessIp;
        private string _WebAccessProject;
        private log4net.ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public WebaccessHelper()
        {
            this._WebAccessIp = WebAccessConfig.Ip;
            this._WebAccessProject = WebAccessConfig.ProjectName;
            WaLogin();
        }

        public WebaccessHelper(string WebAccessIp, string WebAccessProject)
        {
            this._WebAccessIp = WebAccessIp;
            this._WebAccessProject = WebAccessProject;
            WaLogin();
        }
        
        /// <summary>
        /// WebAccess登录
        /// </summary>
        private void WaLogin()
        {
            try
            {
                string userName = WebAccessConfig.UserName, password = WebAccessConfig.Password;
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("http://localhost/WaWebService/Logon");
                request.ContentType = "application/xml";
                request.Method = System.Net.WebRequestMethods.Http.Get;
                byte[] authBytes = Encoding.UTF8.GetBytes((userName + ":" + password).ToCharArray());
                request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(authBytes);

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                string responseStr = getStringFromResponse(response);
                //textBox1.AppendText(DateTime.Now + responseStr);
            }
            catch (Exception ex)
            {
                _log.Error(DateTime.Now + ex.Message);
            }
        }
        private string getStringFromResponse(HttpWebResponse response)
        {
            try
            {
                Stream resStream = response.GetResponseStream();
                StringBuilder resString = new StringBuilder();
                int count = 0;
                byte[] buf = new byte[8192];
                do
                {
                    count = resStream.Read(buf, 0, buf.Length);
                    if (count != 0)
                    {
                        resString.Append(Encoding.UTF8.GetString(buf, 0, count));
                    }
                } while (count > 0);
                resStream.Close();
                response.Close();
                return resString.ToString();
            }
            catch (Exception ex)
            {
                // Log.write_log_message(DateTime.Now + ex.Message);
            }
            return "";
        }
        /// <summary>
        /// 获取Tag点值
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public string GetTagVal(string TagName)
        {
            string server = "http://" + _WebAccessIp + "/WaWebService/GetTagValueText/" + _WebAccessProject;
            string xmlstring = "";

            xmlstring = "<GetTagValueText>";
            xmlstring += "<Tags>";
            xmlstring += "<TagName>";
            xmlstring += "<Name>";
            xmlstring += TagName;
            xmlstring += "</Name>";
            xmlstring += "</TagName>";
            xmlstring += "</Tags>";
            xmlstring += "</GetTagValueText>";

            string rst = PostWebServiceByXml(server, xmlstring, WebAccessConfig.UserName, WebAccessConfig.Password);

            var xdoc = new XmlDocument();
            if (rst != string.Empty)
            {
                xdoc.LoadXml(rst);
                XmlNode xnode = xdoc.SelectSingleNode("GetTagValueText/Values/Value/Value");
                if (xnode != null)
                {
                    return xnode.InnerText;
                }
                else
                {
                    return string.Empty;
                }
            }
            return string.Empty;
        }

        //POST方式向webAccess发送xml格式文件,更新点数据
        public string PostWebServiceByXml(String URL, string Pars, string UserName, string PassWord)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Headers.Add("Authorization", "Basic " + Convert.ToBase64String(Encoding.ASCII.GetBytes(UserName + ":" + PassWord)));
            request.Method = "POST";
            request.KeepAlive = false;
            request.ContentType = "application/xml; charset=utf-8 ";
            request.ReadWriteTimeout = 1000;

            string str = Pars;
            byte[] byData = Encoding.UTF8.GetBytes(str); // convert POST data to bytes
            request.ContentLength = byData.Length;


            try
            {
                // Add the post data to the web request
                Stream outputStream = request.GetRequestStream();
                outputStream.Write(byData, 0, byData.Length);
                outputStream.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream s = response.GetResponseStream();
                StreamReader sr = new StreamReader(s, Encoding.GetEncoding("utf-8"));
                string strResult = sr.ReadToEnd();
                return strResult;
            }
            catch (Exception ex)
            {
                _log.Error(DateTime.Now + ex.Message);
            }

            return string.Empty;

        }

        /// <summary>
        /// 获取Tag的float值
        /// </summary>
        /// <param name="TagName"></param>
        /// <returns></returns>
        public float GetWAValue(string TagName)
        {
            string value = GetTagVal(TagName);
            float ret = 0;
            float.TryParse(value, out ret);
            return ret;
        }
    }
}
