﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace bluedragon.weixin.tool.common
{
    class WeiXinHelper
    {
        private static readonly string _token;
        private static readonly string _appId;
        private static readonly string _appSecret;
        private static string _accessToken;
        private static DateTime _lastTime = DateTime.MinValue;

        static WeiXinHelper()
        {
            _token = ConfigurationHelper.Token;
            _appId = ConfigurationHelper.AppId;
            _appSecret = ConfigurationHelper.AppSecret;
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public string GetAccessToken()
        {
            if (_lastTime == DateTime.MinValue || DateTime.Now.Subtract(_lastTime) >= TimeSpan.FromHours(2))
            {
                //第一次请求，或者accessToken时效到了，重新请求
                string url =
                    string.Format(
                        "https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}",
                        _appId,
                        _appSecret);
                string retString = HttpWebRequestHelper.CreateGetHttpsRequest(url);
                var obj = JObject.Parse(retString);
                if (obj["errcode"] == null)
                {
                    _accessToken = obj["access_token"].ToString();
                    return _accessToken;
                }
                throw new Exception(string.Format("获取access_token失败，错误代码：{0}", obj["errcode"]));
            }
            return _accessToken;
        }

        /// <summary>
        /// 获取微信服务器IP地址
        /// </summary>
        /// <returns></returns>
        public IEnumerable<string> GetServerIps()
        {
            string accessToken = GetAccessToken();
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/getcallbackip?access_token={0}",
                    accessToken);
            string retString = HttpWebRequestHelper.CreateGetHttpsRequest(url);
            var obj = JObject.Parse(retString);
            if (obj["errcode"] == null)
            {
                var arr = obj["ip_list"] as JArray;
                foreach (var item in arr)
                {
                    yield return item.ToString();
                }
            }
            throw new Exception(string.Format("获取微信服务器IP地址失败，错误代码：{0}", obj["errcode"]));
        }

        /// <summary>
        /// 创建自定义菜单
        /// </summary>
        /// <param name="menuJson"></param>
        public void CreateMenus(string menuJson)
        {
            string accessToken = GetAccessToken();
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/menu/create?access_token={0}",
                    accessToken);
            string retString = HttpWebRequestHelper.CreatePostHttpsRequest(url, menuJson);
            var obj = JObject.Parse(retString);
            if (obj["errcode"].ToString() != "0")
            {
                throw new Exception(string.Format("创建自定义菜单失败，错误代码：{0}", obj["errcode"]));
            }
        }

        /// <summary>
        /// 自定义菜单查询
        /// </summary>
        /// <returns></returns>
        public string GetMenus()
        {
            string accessToken = GetAccessToken();
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}",
                    accessToken);
            string retString = HttpWebRequestHelper.CreateGetHttpsRequest(url);
            return retString;
        }

        /// <summary>
        /// 删除自定义菜单
        /// </summary>
        public void DeleteMenus()
        {
            string accessToken = GetAccessToken();
            string url =
                string.Format(
                    "https://api.weixin.qq.com/cgi-bin/menu/delete?access_token={0}",
                    accessToken);
            string retString = HttpWebRequestHelper.CreateGetHttpsRequest(url);
            var obj = JObject.Parse(retString);
            if (obj["errcode"].ToString() != "0")
            {
                throw new Exception(string.Format("删除自定义菜单失败，错误代码：{0}", obj["errcode"]));
            }
        }
    }
}