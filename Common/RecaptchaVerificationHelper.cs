using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Web;
namespace DotNet.Utilities
{
    /// <summary>
    /// google验证码校验类
    /// </summary>
    public class RecaptchaVerificationHelper
    {
        public static string RecaptchaJsUrl = "http://www.google.com/recaptcha/api/challenge?k=6LeIrewSAAAAANsXLn2pPpPv5tYLkL-2FGxg8Lm5";

        private string _Challenge = null;

        /// <summary>
        /// Creates an instance of the <see cref="RecaptchaVerificationHelper"/> class.
        /// </summary>
        /// <param name="privateKey">Sets the private key of the recaptcha verification request.</param>
        public RecaptchaVerificationHelper()
        {
            string privateKey = "6LeIrewSAAAAAJDuVlUZkaF9RViofoXjzqqAWSKS";
            this.PublicKey = "6LeIrewSAAAAANsXLn2pPpPv5tYLkL-2FGxg8Lm5";
            if (String.IsNullOrEmpty(privateKey))
            {
                throw new InvalidOperationException("Private key cannot be null or empty.");
            }

            if (HttpContext.Current == null || HttpContext.Current.Request == null)
            {
                throw new InvalidOperationException("Http request context does not exist.");
            }

            HttpRequest request = HttpContext.Current.Request;

            if (String.IsNullOrEmpty(request.Form["recaptcha_challenge_field"]))
            {
                throw new InvalidOperationException("Recaptcha challenge field cannot be empty.");
            }

            this.PrivateKey = privateKey;
            this.UserHostAddress = request.UserHostAddress;
            this._Challenge = request.Form["recaptcha_challenge_field"];
            this.Response = request.Form["recaptcha_response_field"];
        }

        /// <summary>
        /// Gets the privae key of the recaptcha verification request.
        /// </summary>
        public string PrivateKey
        {
            get;
            private set;
        }

        public string PublicKey
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the user's host address of the recaptcha verification request.
        /// </summary>
        public string UserHostAddress
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the user's response to the recaptcha challenge of the recaptcha verification request.
        /// </summary>
        public string Response
        {
            get;
            private set;
        }

        /// <summary>
        /// 请求google服务，获取校验结果
        /// </summary>
        /// <returns>Returns the result as a value of the <see cref="RecaptchaVerificationResult"/> enum.</returns>
        public RecaptchaVerificationResult VerifyRecaptchaResponse()
        {
            string privateKey = PrivateKey;

            string postData = String.Format("privatekey={0}&remoteip={1}&challenge={2}&response={3}", privateKey, this.UserHostAddress, this._Challenge, this.Response);

            byte[] postDataBuffer = System.Text.Encoding.ASCII.GetBytes(postData);

            Uri verifyUri = new Uri("http://api-verify.recaptcha.net/verify", UriKind.Absolute);

            try
            {
                HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(verifyUri);
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = postDataBuffer.Length;
                webRequest.Method = "POST";

                IWebProxy proxy = WebRequest.GetSystemWebProxy();
                proxy.Credentials = CredentialCache.DefaultCredentials;

                webRequest.Proxy = proxy;

                Stream requestStream = webRequest.GetRequestStream();
                requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);

                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                string[] responseTokens = null;
                using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                {
                    responseTokens = sr.ReadToEnd().Split('\n');
                }

                if (responseTokens.Length == 2)
                {
                    Boolean success = responseTokens[0].Equals("true", StringComparison.CurrentCulture);

                    if (success)
                    {
                        return RecaptchaVerificationResult.Success;
                    }
                    else
                    {
                        if (responseTokens[1].Equals("incorrect-captcha-sol", StringComparison.CurrentCulture))
                        {
                            return RecaptchaVerificationResult.IncorrectCaptchaSolution;
                        }
                        else if (responseTokens[1].Equals("invalid-site-private-key", StringComparison.CurrentCulture))
                        {
                            return RecaptchaVerificationResult.InvalidPrivateKey;
                        }
                        else if (responseTokens[1].Equals("invalid-request-cookie", StringComparison.CurrentCulture))
                        {
                            return RecaptchaVerificationResult.InvalidCookieParameters;
                        }
                    }
                }

                return RecaptchaVerificationResult.UnknownError;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Verifies whether the user's response to the recaptcha request is correct.
        /// </summary>
        /// <returns>Returns the result as a value of the <see cref="RecaptchaVerificationResult"/> enum.</returns>
        public Task<RecaptchaVerificationResult> VerifyRecaptchaResponseTaskAsync()
        {
            Task<RecaptchaVerificationResult> result = Task<RecaptchaVerificationResult>.Factory.StartNew(() =>
            {
                string privateKey = PrivateKey;

                string postData = String.Format("privatekey={0}&remoteip={1}&challenge={2}&response={3}", privateKey, this.UserHostAddress, this._Challenge, this.Response);

                byte[] postDataBuffer = System.Text.Encoding.ASCII.GetBytes(postData);

                Uri verifyUri = new Uri("http://api-verify.recaptcha.net/verify", UriKind.Absolute);

                try
                {
                    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(verifyUri);
                    webRequest.ContentType = "application/x-www-form-urlencoded";
                    webRequest.ContentLength = postDataBuffer.Length;
                    webRequest.Method = "POST";

                    IWebProxy proxy = WebRequest.GetSystemWebProxy();
                    proxy.Credentials = CredentialCache.DefaultCredentials;

                    webRequest.Proxy = proxy;

                    Stream requestStream = webRequest.GetRequestStream();
                    requestStream.Write(postDataBuffer, 0, postDataBuffer.Length);

                    HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

                    string[] responseTokens = null;
                    using (StreamReader sr = new StreamReader(webResponse.GetResponseStream()))
                    {
                        responseTokens = sr.ReadToEnd().Split('\n');
                    }

                    if (responseTokens.Length == 2)
                    {
                        Boolean success = responseTokens[0].Equals("true", StringComparison.CurrentCulture);

                        if (success)
                        {
                            return RecaptchaVerificationResult.Success;
                        }
                        else
                        {
                            if (responseTokens[1].Equals("incorrect-captcha-sol", StringComparison.CurrentCulture))
                            {
                                return RecaptchaVerificationResult.IncorrectCaptchaSolution;
                            }
                            else if (responseTokens[1].Equals("invalid-site-private-key", StringComparison.CurrentCulture))
                            {
                                return RecaptchaVerificationResult.InvalidPrivateKey;
                            }
                            else if (responseTokens[1].Equals("invalid-request-cookie", StringComparison.CurrentCulture))
                            {
                                return RecaptchaVerificationResult.InvalidCookieParameters;
                            }
                        }
                    }

                    return RecaptchaVerificationResult.UnknownError;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            });

            return result;
        }
    }

    /// <summary>
    /// 校验结果
    /// </summary>
    public enum RecaptchaVerificationResult
    {
        /// <summary>
        /// Verification failed but the exact reason is not known.
        /// </summary>
        UnknownError = 0,
        /// <summary>
        /// Verification succeeded.
        /// </summary>
        Success = 1,
        /// <summary>
        /// The user's response to recaptcha challenge is incorrect.
        /// </summary>
        IncorrectCaptchaSolution = 2,
        /// <summary>
        /// The request parameters in the client-side cookie are invalid.
        /// </summary>
        InvalidCookieParameters = 3,
        /// <summary>
        /// The private supplied at the time of verification process is invalid.
        /// </summary>
        InvalidPrivateKey = 4,
        /// <summary>
        /// The user's response to the recaptcha challenge is null or empty.
        /// </summary>
        NullOrEmptyCaptchaSolution = 5
    }
}
