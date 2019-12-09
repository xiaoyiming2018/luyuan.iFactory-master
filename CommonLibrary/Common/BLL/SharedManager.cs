using System;
using System.Collections.Generic;
using System.Text;

namespace Advantech.IFactory.CommonLibrary
{
    public class SharedManager
    {
        SharedService SS = new SharedService();
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="encode">编码</param>
        /// <param name="Input">需加密内容</param>
        /// <returns></returns>
        public string Base64Encryption(Encoding encode, string input)
        {
            string result = SS.Base64Encryption(encode, input);
            return result;
        }

        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="encoding">编码</param>
        /// <param name="Input">需解密内容</param>
        /// <returns></returns>
        public string Base64Decrypt(Encoding encoding, string input)
        {
            string result = SS.Base64Decrypt(encoding, input);
            return result;
        }
        /// <summary>
        /// 获取随机数
        /// </summary>
        /// <returns>返回1-100随机数</returns>
        public int GetRandom()
        {
            int num = SS.GetRandom();
            return num;
        }

        public string PostUrl(string url, string postData)
        {
            string result = SS.PostUrl(url, postData);
            return result;
        }
        public string GetUrl(string Url)
        {
            string result = SS.GetUrl(Url);
            return result;
        }
        public int GetTimeDifference(object startTime, object endTime)
        {
            int totalSeconds = SS.GetTimeDifference(startTime, endTime);
            return totalSeconds;
        }
    }
}
