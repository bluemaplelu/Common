using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace DotNet.Utilities
{
    public class SendSMS
    {
        //发送短信
        public static string Send(string PhoneNumber,  SMSTemplateType type, string json)
        {
            string tmp = "";
            if (PhoneNumber != "")
            {
                string accountname = "www.shipinhui.com";
                string accountpwd = "SHIPINHUI@SHIPINHUI";
                //string json = "[{'key':'code','value':'" + Code + "'},{'key':'date','value':'" + Option + "'}]";

                string templateCode = GetSmsTemplateCode(type); 
                com.qihuiwang.qhwsms.SmsService service = new com.qihuiwang.qhwsms.SmsService();
                tmp = service.SmsSendCodeByTemplateId(accountname, accountpwd, PhoneNumber, templateCode, json);
                return tmp;
            }
            return tmp;
        }
        /// <summary>
        /// 阿里大鱼发送短信接口
        /// </summary>
        /// <param name="phoneNumber">手机号</param>
        /// <param name="signName">签名</param>
        /// <param name="parmsjson">参数json</param>
        /// <param name="templateCode">短信模板编号</param>
        /// <returns></returns>
        public static string Send(string phoneNumber,string signName,string parmsjson,string templateCode)
        {
            string appkey = "23352902";
            string secret = "88a3ab0b4d9c02e26375a863b602f216";
            ITopClient client = new DefaultTopClient("http://gw.api.taobao.com/router/rest", appkey, secret);
            AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
            req.SmsType = "normal";
            req.SmsFreeSignName = signName;
            req.SmsParam = parmsjson;
            req.RecNum = phoneNumber;
            req.SmsTemplateCode = templateCode;
            AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
            return JsonConvert.SerializeObject(rsp.Result);
        }
      
        /// <summary>
        /// 定义短信模板
        /// </summary> 
        public enum SMSTemplateType
        { 
            账户注册,
            找回密码,
            修改密码  
        }

        /// <summary>
        /// 根据短信模板类型获取短信模板Code
        /// </summary>
        /// <param name="smsType">短信模板类型
        /// 在此处添加各个Code所对应的模板内容
        /// 
        /// </param>
        /// <returns></returns>
        public static string GetSmsTemplateCode(SMSTemplateType smsType)
        {
            string code = string.Empty;

            switch (smsType)
            { 
                case SMSTemplateType.账户注册:
                    code = "";
                    break;
                case SMSTemplateType.找回密码:
                    code = "";
                    break;
                case SMSTemplateType.修改密码:
                    code = "";
                    break;
                default :
                    code = "";
                    break;
            } 
            return code;
        }

        /// <summary>
        /// 发送付款成功提醒短信
        /// </summary>
        /// <param name="PhoneNumber"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendSms(string PhoneNumber, string ordernum)
        {
            string tmp = "";
            if (!String.IsNullOrEmpty(PhoneNumber))
            {
                string accountname = "www.yangdou.com";
                string accountpwd = "YANGDOU@YANGDOU";
                string json = "[{'key':'ordernum','value':'" + ordernum + "'}]";
                com.qihuiwang.qhwsms.SmsService service = new com.qihuiwang.qhwsms.SmsService();
                tmp = service.SmsSendCodeByTemplateId(accountname, accountpwd, PhoneNumber, "YANGDOU_DINGDANTIXING", json);
            }
            return tmp;
        }
    }
}
