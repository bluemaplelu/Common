using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Mail;
using System.Configuration;

namespace DotNet.Utilities
{
    public class EmailHelper
    {
        static System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"\A(?:(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\]))\Z", System.Text.RegularExpressions.RegexOptions.IgnoreCase);

        static string EmailAccount
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailAccount"]) ? "system@yangdou.com" : ConfigurationManager.AppSettings["EmailAccount"];
            }
        }
        static string EmailAddress
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailAddress"]) ? "system@yangdou.com" : ConfigurationManager.AppSettings["EmailAddress"];
            }
        }//= "system@qihuiwang.com";
        static string EmailPassword
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailPassword"]) ? "YangDouMail11" : ConfigurationManager.AppSettings["EmailPassword"];
            }
        }// = "qhw123456";
        static string EmailSmtpClient
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailSmtpClient"]) ? "mail.yangdou.com" : ConfigurationManager.AppSettings["EmailSmtpClient"];
            }
        }// = "mail.qihuiwang.com";
        static int EmailPort
        {
            get
            {
                return string.IsNullOrEmpty(ConfigurationManager.AppSettings["EmailPort"]) ? 25 : TypeParse.ObjectToInt(ConfigurationManager.AppSettings["EmailPort"], 25);
            }
        }// = 25;



        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param email="email">邮件集合（多个以;隔开）</param>
        /// <param name="title">标题</param>
        /// <param name="body">字段</param>
        /// <param name="displayName">字段</param>
        /// <returns>邮件是否发送成功</returns>
        public static bool SendEmail(string email, string title, string body, string displayName)
        {
            MailAddress msFrom = new MailAddress(EmailAddress, displayName, System.Text.Encoding.GetEncoding("utf-8"));

            MailMessage message = new MailMessage();
            message.From = msFrom;
            message.Body = body;
            message.Subject = title;
            message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            message.Priority = MailPriority.Normal;
            message.IsBodyHtml = true;
            string[] names = email.Replace('；', ';').Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string to in names)
            {
                if (regex.IsMatch(to.Trim()))
                    message.To.Add(to.Trim());
                else
                {
                    continue;
                }
            }

            try
            {
                SmtpClient client;
                if (EmailPort > 0)
                {
                    client = new SmtpClient(EmailSmtpClient, EmailPort);
                }
                else
                {
                    client = new SmtpClient(EmailSmtpClient);
                }
                client.Credentials = new System.Net.NetworkCredential(EmailAccount, EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    //client.SendAsync(message, null);
                    client.Send(message);
                }
                catch
                {
                    client.Send(message);
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public static bool SendEmailBcc(string email, string title, string body, string displayName)
        {
            MailAddress msFrom = new MailAddress(EmailAddress, displayName, System.Text.Encoding.GetEncoding("utf-8"));

            MailMessage message = new MailMessage();
            message.From = msFrom;
            message.Body = body;
            message.Subject = title;
            message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
            message.Priority = MailPriority.High;
            message.IsBodyHtml = true;


            string[] bccMails = email.Split(',', ';', '；');
            bool isFirst = true;
            foreach (string bccMail in bccMails)
            {
                string bbc = bccMail;
                if (string.IsNullOrEmpty(bbc))
                {
                    continue;
                }
                else
                {
                    bbc = bbc.Trim();
                }
                if (regex.IsMatch(bbc))
                {
                    if (isFirst)
                    {
                        message.To.Add(bbc);
                        isFirst = false;
                    }
                    else
                    {
                        message.Bcc.Add(bbc);
                    }
                }
            }

            try
            {
                SmtpClient client;
                if (EmailPort > 0)
                {
                    client = new SmtpClient(EmailSmtpClient, EmailPort);
                }
                else
                {
                    client = new SmtpClient(EmailSmtpClient);
                }

                client.Credentials = new System.Net.NetworkCredential(EmailAccount, EmailPassword);
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    client.SendAsync(message, null);

                }
                catch
                {
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return true;
        }

    }
}
