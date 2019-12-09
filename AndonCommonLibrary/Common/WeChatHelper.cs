using Advantech.IFactory.CommonLibrary;
using Advantech.IFactory.CommonHelper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.Andon
{
    public class WeChatHelper
    {
        /// <summary>
        /// 发送微信通知内容
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="Content"></param>
        /// <returns></returns>
        public static async void SendWeChatMessage(List<string> Names, string Content)
        {
            try
            {
                List<WeChatMessage> WeChatMessagesList = new List<WeChatMessage>();
                WeChatMessage weChatMessage;
                foreach (string name in Names)
                {
                    weChatMessage = new WeChatMessage();
                    weChatMessage.touser = name;
                    weChatMessage.text = Content;
                    WeChatMessagesList.Add(weChatMessage);
                }
                bool result = await MessageSend(WeChatMessagesList);
            }
            catch(Exception ex)
            {

            }
        }

        private static async Task<bool> MessageSend(List<WeChatMessage> MessagesList)
        {
            SharedManager SM = new SharedManager();
            foreach (WeChatMessage messageItem in MessagesList)
            {
                WeChatMessage objper = new WeChatMessage();
                objper.touser = messageItem.touser;//要发送微信的人员姓名
                objper.text = messageItem.text;//要发送的微信内容
                objper.safe = 0;
                try
                {
                    string postData = JsonConvert.SerializeObject(objper);
                    string result = SM.PostUrl(WeChatConfig.WeChatUrl, postData);
                    List<string> objwx = JsonHelper.GetJson(result);
                    if (Convert.ToInt32(objwx[1]) != 0)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
    }
}


