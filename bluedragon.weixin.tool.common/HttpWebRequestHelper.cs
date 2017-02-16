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

        /// <summary>
        /// 模拟表单文件上传
        /// </summary>
        /// <param name="path"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string UploadFile(string path, string url)
        {
            #region 模板方式
            //    HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //    string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            //    request.Method = "post";
            //    CookieContainer cookieContainer = new CookieContainer();
            //    request.CookieContainer = cookieContainer;
            //    request.KeepAlive = true;
            //    request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            //    StringBuilder sb = new StringBuilder();
            //    int pos = path.LastIndexOf("\\");
            //    string fileName = path.Substring(pos + 1);
            //    byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            //    byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");
            //    //文件数据模板  
            //    string fileFormdataTemplate =
            ////        "\r\n--" + boundary +
            //        "Content-Disposition: form-data; name=\"file\"; filename=\"{0}\"" +
            //        "\r\nContent-Type: application/octet-stream" +
            //        "\r\n\r\n";
            //    //文本数据模板  
            //    string dataFormdataTemplate =
            //     //   "\r\n--" + boundary +
            //        "Content-Disposition: form-data; name=\"{0}\"" +
            //        "\r\n\r\n{1}";
            //    StringBuilder sbHeader = new StringBuilder(string.Format(fileFormdataTemplate, fileName));
            //    byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            //    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    {
            //        byte[] bArr = new byte[fs.Length];
            //        fs.Read(bArr, 0, bArr.Length);

            //        using (Stream postStream = request.GetRequestStream())
            //        {
            //            postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
            //            postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
            //            postStream.Write(bArr, 0, bArr.Length);
            //            postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
            //        }
            //    }

            //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //    //直到request.GetResponse()程序才开始向目标网页发送Post请求 
            //    Stream instream = response.GetResponseStream();
            //    StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //    //返回结果网页（html）代码 
            //    return sr.ReadToEnd();
            #endregion
            // 设置参数 
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            request.AllowAutoRedirect = true;
            request.Method = "POST";
            string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线 
            request.ContentType = "multipart/form-data;charset=utf-8;boundary=" + boundary;
            byte[] itemBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "\r\n");
            byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n--" + boundary + "--\r\n");

            int pos = path.LastIndexOf("\\");
            string fileName = path.Substring(pos + 1);

            //请求头部信息 
            StringBuilder sbHeader = new StringBuilder(string.Format("Content-Disposition:form-data;name=\"media\";filename=\"{0}\"\r\nContent-Type:application/octet-stream\r\n\r\n", fileName));
            byte[] postHeaderBytes = Encoding.UTF8.GetBytes(sbHeader.ToString());

            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                byte[] bArr = new byte[fs.Length];
                fs.Read(bArr, 0, bArr.Length);
                using (Stream postStream = request.GetRequestStream())
                {
                    postStream.Write(itemBoundaryBytes, 0, itemBoundaryBytes.Length);
                    postStream.Write(postHeaderBytes, 0, postHeaderBytes.Length);
                    postStream.Write(bArr, 0, bArr.Length);
                    postStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                }
            }

            //发送请求并获取相应回应数据 
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            //直到request.GetResponse()程序才开始向目标网页发送Post请求 
            Stream instream = response.GetResponseStream();
            StreamReader sr = new StreamReader(instream, Encoding.UTF8);
            //返回结果网页（html）代码 
            return sr.ReadToEnd();
        }
    }
}
