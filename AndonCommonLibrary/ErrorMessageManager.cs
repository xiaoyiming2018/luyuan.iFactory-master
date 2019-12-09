using Advantech.IFactory.CommonLibrary;
using System.Collections.Generic;
using System.Linq;
using Advantech.IFactory.CommonHelper.ScadaAPI;
using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Advantech.IFactory.Andon
{
    public class ErrorMessageManager
    {
        private ClassInfoManager classInfoManager = new ClassInfoManager();
        private ErrorLogPersonManager errorLogPersonManager= new ErrorLogPersonManager();
        private TagInfoManager tagInfoManager = new TagInfoManager();
        private webaccess_tag_info SpeechTag;
        private SRPLogManager srpLogManager = new SRPLogManager();

        public ErrorMessageManager()
        {
            SpeechTag = tagInfoManager.SelectOne(SystemTagCodeEnum.andon_speech_message.ToString());
        }
        /// <summary>
        /// 异常通知
        /// </summary>
        /// <param name="errorObject"></param>
        /// <param name="Level">层级</param>
        /// <param name="EventFlow">事件流程</param>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        /// <param name="FinishedMode">最后完成模式，通知已通知人员</param>
        public async Task SendErrorMessageAsync(ErrorObject errorObject, int Level, EventHandleFlowEnum EventFlow,
                                                string Subject, string Content, bool FinishedMode = false)
        {
            List<person> persons;
            List<string> infoList = new List<string>();
            
            persons = GetMsgedPersonList(errorObject, Level, EventFlow,FinishedMode);//获取需要通知的人员列表
            bool res=await PostMessageToLuyuan(errorObject, EventFlow, persons);//对外发送信息
            if (errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.All ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.WeChat ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.WeChat_Broadcast ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.WeChat_Email)
            {
                //微信通知
                foreach (person p in persons)
                {
                    infoList.Add(p.user_name);
                }
                WeChatHelper.SendWeChatMessage(infoList, Content);
            }
            if (errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.All ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.Email ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.Email_Broadcast ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.WeChat_Email)
            {
                infoList = new List<string>();
                //邮件通知
                foreach (person p in persons)
                {
                    infoList.Add(p.user_email);
                }
                MailHelper.SendMail(infoList, Subject, Content);
            }
            if (errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.All ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.Broadcast ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.Email_Broadcast ||
                errorObject.eConfig.trigger_message_type == (int)ErrorMsgType.WeChat_Broadcast)
            {
                //语音通知
                //speechSynth.Speak(Content);
                if(SpeechTag!=null)
                {
                    if (ScadaAPIConfig.EnableScadaApi)
                    {
                        ScadaAPIHelper.WriteValueAsync(SpeechTag.tag_code, Content);
                    }
                    else
                    {
                        //MqttManager.MqttHelper.WriteValueToWA(SpeechTag.tag_code, Content, true);//输出语音播报文本信息
                    }
                }
            }
        }
        /// <summary>
        /// 获取发送通知的人员列表
        /// </summary>
        /// <param name="errorObject">异常对象</param>
        /// <param name="Level">等级</param>
        /// <param name="EventFlow">事件流程点</param>
        /// <param name="FinishedMode">完成模式</param>
        /// <returns></returns>
        public List<person> GetMsgedPersonList(ErrorObject errorObject, int Level, EventHandleFlowEnum EventFlow,
                                               bool FinishedMode = false)
        {
            List<person> persons = new List<person>();
            List<string> infoList = new List<string>();
            List<ErrorCfgPerson> ePersons = new List<ErrorCfgPerson>();
            string classNo = classInfoManager.GetCurrentClassNo();//当前班次

            if (FinishedMode == false)
            {
                if(EventFlow== EventHandleFlowEnum.Event_Timeout)//超时加载人员为当前级别以及向下一级
                {
                    ePersons = errorObject.eCfgPersonList.Where(x => (x.eCfgPerson.person_level == Level || x.eCfgPerson.person_level == (Level - 1)) &&
                                                                x.eCfgPerson.class_no.Equals(classNo)).ToList();//获取多级配置人员列表
                }
                else
                {
                    if (errorObject.eConfig.message_multilevel == 1 && Level < (int)MessageLevel.Level3)//多级人员发送模式
                    {
                        ePersons = errorObject.eCfgPersonList.Where(x => (x.eCfgPerson.person_level == Level || x.eCfgPerson.person_level == Level + 1) &&
                                                                    x.eCfgPerson.class_no.Equals(classNo)).ToList();//获取多级配置人员列表
                    }
                    else
                    {
                        ePersons = errorObject.eCfgPersonList.Where(x => x.eCfgPerson.person_level == Level &&
                                                                         x.eCfgPerson.class_no.Equals(classNo)).ToList();//获取当前级别配置人员列表
                    }
                }

                foreach (var item in ePersons)
                {
                    if (item.ePerson != null)
                    {
                        //判断此人员是否已经通知过，通知过则去除不重复通知
                        if (errorObject.eMsgedPersonList.Any(x=>x.eLogPerson.error_log_id== errorObject.ELog.id && 
                                                                x.eLogPerson.person_id== item.ePerson.id &&
                                                                x.eLogPerson.message_flow== EventFlow.ToString() &&
                                                                x.eLogPerson.message_level== item.eCfgPerson.person_level)==false)
                        {
                            persons.Add(item.ePerson);
                            ErrorLogPerson eLogPersonObject = new ErrorLogPerson();
                            //记录已通知人员
                            error_log_person ePerson = errorLogPersonManager.InsertMessagePerson(errorObject.ELog.id, item.ePerson.id, item.eCfgPerson.person_level, EventFlow);
                            eLogPersonObject.ePerson = item.ePerson;
                            eLogPersonObject.eLogPerson = ePerson;
                            errorObject.eMsgedPersonList.Add(eLogPersonObject);
                        }
                    }
                }
            }
            else
            {
                //结束模式需去除已经为事件确认标识的人员
                foreach (var item in errorObject.eMsgedPersonList.Where(x=>x.eLogPerson.message_flow != EventFlow.ToString()))//为结束模式，则取出已通知的人员
                {
                    if(!persons.Any(x=>x.id== item.ePerson.id))//去除重复项
                    {
                        errorLogPersonManager.InsertMessagePerson(errorObject.ELog.id, item.ePerson.id, item.eLogPerson.message_level, EventFlow);
                        persons.Add(item.ePerson);
                    }
                }
            }
            return persons;
        }

        /// <summary>
        /// 人员通知时向绿源抛送信息，此接口为定制
        /// </summary>
        /// <param name="errorObject">安灯对象</param>
        /// <param name="EventFlow">流程节点</param>
        /// <param name="Persons">人员清单</param>
        /// <returns></returns>
        public async Task<bool> PostMessageToLuyuan(ErrorObject errorObject, EventHandleFlowEnum EventFlow, List<person> Persons)
        {
            if(EventFlow != EventHandleFlowEnum.Event_Trigger && EventFlow != EventHandleFlowEnum.Event_Timeout)//只有事件发生时需要通知
            {
                return false;
            }
            try
            {
                string ApiBaseUrl = "https://api.luyuan.cn/MESApi/GetErrorMsg";
                line_info line = AndonGlobalCfg.LinesList.FirstOrDefault(x => x.line_id == errorObject.ELog.line_id);
                //ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
                foreach (person p in Persons)
                {
                    try
                    { 
                        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ApiBaseUrl);
                        request.ContentType = "application/json; charset=utf-8";
                        request.Method = "POST";
                        request.Headers.Add("api-key", "23333");
                        request.Headers.Add("api-sign", "463fbc8ffc4f1da5bbc94ffc5b911ae5b76ff3cd0ee1196ef978b079f74acce7");

                        String Body = "{\"触发时间\":\"" + errorObject.ELog.start_time + "\"," +
                                       "\"呼叫用户姓名\":\"" + p.user_name + "\"," +
                                       "\"呼叫号码\":\"" + p.mobile_phone + "\"," +
                                       "\"流水线名称\":\"" + line.line_name_en + "\"," +
                                       "\"站别名称\":\"" + errorObject.DeviceCode + "\"," +
                                       "\"问题类型\":\"" + errorObject.ELog.error_name + "\"}";
                        byte[] buffer = Encoding.UTF8.GetBytes(Body);
                        //request.ContentLength = buffer.Length;
                        //request.GetRequestStream().Write(buffer, 0, buffer.Length);
                        Stream newStream = await request.GetRequestStreamAsync().ConfigureAwait(false);
                        newStream.Write(buffer, 0, buffer.Length);
                        newStream.Close();
                        HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                        using (StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                        {
                            string ret = reader.ReadToEnd();

                            if (ret.ToLower().Trim().Contains("statuscode: 200"))
                            {
                                //continue;
                            }
                            else
                            {
                                srpLogManager.Insert("PostMessageToLuyuan failed=" + Body);
                            }
                            Console.WriteLine("PostMessageToLuyuan=" + ret);
                        }
                        response.Close();
                    }
                    catch (WebException ex)
                    {
                        Console.WriteLine("PostMessageToLuyuan error=" + ex.Message);
                        srpLogManager.Insert("PostMessageToLuyuan error=" + ex.Message);
                        WebResponse errResp = ex.Response;
                        using (Stream respStream = errResp.GetResponseStream())
                        {
                            StreamReader reader = new StreamReader(respStream);
                            string text = reader.ReadToEnd();
                            Console.WriteLine(text);
                            srpLogManager.Insert("PostMessageToLuyuan error=" + text);
                        }
                        return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("PostMessageToLuyuan error=" + ex.Message);
                srpLogManager.Insert("PostMessageToLuyuan error="+ex.Message);
            }
            return false;
        }

    }
}
