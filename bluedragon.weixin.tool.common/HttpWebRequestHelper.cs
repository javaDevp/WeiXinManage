using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace bluedragon.weixin.tool.common
{
    /// <summary>
    /// Http 请求辅助类
    /// </summary>
    public class HttpWebRequestHelper
    {
        /// <summary>
        /// get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string CreateGetHttpsRequest(string url, int timeout = 60000)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "GET";
            request.ContentType = "application/json;charset=utf-8"; //"application/x-www-form-urlencoded";
            request.Timeout = timeout;
            string result = "";

            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

        /// <summary>
        /// post请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="pramJson"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public static string CreatePostHttpsRequest(string url, string pramJson, int timeout = 60000)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }
            Uri address = new Uri(url);
            HttpWebRequest request = WebRequest.Create(address) as HttpWebRequest;
            request.Method = "POST";
            request.ContentType = "application/json;charset=utf-8"; //"application/x-www-form-urlencoded";
            request.Timeout = timeout;
            byte[] byteData = UTF8Encoding.UTF8.GetBytes(pramJson == null ? "" : pramJson);
            request.ContentLength = byteData.Length;
            using (Stream stream = request.GetRequestStream())
            {
                stream.Write(byteData, 0, byteData.Length);
            }
            string result = "";
            using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
            {
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    result = reader.ReadToEnd();
                }
            }
            return result;
        }

    }
}
