using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Advantech.IFactory.CommonHelper
{
    public class JsonHelper
    {
        /// <summary>
        /// 解析json
        /// </summary>
        /// <param name="msg">传入参数</param>
        /// <returns></returns>
        public static List<string> GetJson(string msg)
        {
            JsonReader reader = new JsonTextReader(new StringReader(msg));
            List<string> str = new List<string>();
            try
            {
                while (reader.Read())
                {
                    if (reader.Value != null)
                    {
                        str.Add(reader.Value.ToString());
                    }
                }
            }
            catch
            {
                str = new List<string>();
            }
            return str;
        }
    }
}
