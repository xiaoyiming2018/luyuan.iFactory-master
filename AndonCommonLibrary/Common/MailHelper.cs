using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Advantech.IFactory.CommonLibrary;

namespace Advantech.IFactory.Andon
{
    
    public class MailHelper
    {
        static List<eMailMessage> MailMessagesList = new List<eMailMessage>();

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="Address">地址列表</param>
        /// <param name="Subject">主题</param>
        /// <param name="Content">内容</param>
        public static async void SendMail(List<string> Address, string Subject,string Content)
        {
            try
            {
                eMailMessage MailMessages = new eMailMessage();
                MailMessages.text = Content;
                MailMessages.MailAddressList = Address;
                MailMessages.subject = Subject;
                bool result = await MailSend(MailMessages);
            }
            catch(Exception ex)
            {

            }
        }

        private static async Task<bool> MailSend(eMailMessage MessageItem)
        {
            if (MessageItem.MailAddressList.Count > 0 && MessageItem.subject != null && MessageItem.text != "")
            {
                MailMessage message = new MailMessage();
                try
                {
                    //设置发件人,发件人需要与设置的邮件发送服务器的邮箱一致
                    MailAddress fromAddr = new MailAddress(MailConfig.MailAddress);
                    message.From = fromAddr;
                    //设置收件人,可添加多个,添加方法与下面的一样
                    for (int i = 0; i < MessageItem.MailAddressList.Count; i++)
                    {
                        message.To.Add(MessageItem.MailAddressList[i]);
                    }
                    //设置邮件标题
                    message.Subject = MessageItem.subject;
                    //设置邮件内容
                    message.Body = MessageItem.text;//Convert.ToString(sr.ReadToEnd());
                    message.IsBodyHtml = true;
                    //设置邮件发送服务器,服务器根据你使用的邮箱而不同,可以到相应的 邮箱管理后台查看
                    SmtpClient client = new SmtpClient(MailConfig.MailHost, MailConfig.MailPort);
                    //设置发送人的邮箱账号和密码
                    client.Credentials = new NetworkCredential(MailConfig.MailUserName, MailConfig.MailUserPassword);
                    //启用ssl,也就是安全发送
                    client.EnableSsl = false;
                    client.UseDefaultCredentials = false;
                    //发送邮件
                    client.Send(message);
                }
                catch (Exception ex)
                {
                    string msg = ex.Message;
                    return false;
                }
            }
            return true;
        }
    }
}
