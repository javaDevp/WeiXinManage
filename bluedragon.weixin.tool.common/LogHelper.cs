using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace bluedragon.weixin.tool.common
{
    /// <summary>
    /// 日志辅助类
    /// </summary>
    public class LogHelper
    {
        #region Fields
        private static readonly LogHelper _logHelper = new LogHelper();

        //异常的类
        public string _logClass;
        #endregion

        #region Constructors
        private LogHelper() { }
        #endregion

        #region Methods
        /// <summary>
        /// 获取日志对象
        /// </summary>
        /// <returns></returns>
        public static LogHelper GetInstance(string logClass)
        {
            _logHelper._logClass = logClass;
            return _logHelper;
        }

        /// <summary>
        /// 写日志
        /// </summary>
        /// <param name="logInfo"></param>
        public void WriteLog(string logInfo)
        {
            bool haveExp = true;//先设为true，执行正常改为false
            string errMsg = "";
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    using (FileStream fs = File.Open(this.GetLogFile(), FileMode.Append, FileAccess.Write, FileShare.None))
                    {
                        byte[] bytes = Encoding.UTF8.GetBytes(this.GetFullInfo(logInfo));
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Close();
                        haveExp = false;
                        break;
                    }
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    Thread.Sleep(200);
                }
            }
            if (haveExp)
            {
                throw new Exception(errMsg + " 日志写入失败。");
            }
        }

        /// <summary>
        /// 获取文件路径及文件名
        /// </summary>
        /// <returns></returns>
        private string GetLogFile()
        {
            bool haveExp = true;//先设为true，执行正常改为false
            string path = "";
            string errMsg = "";
            DateTime now = DateTime.Now;
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    path = AppDomain.CurrentDomain.BaseDirectory + "App_Data\\exp";
                    path = string.Concat(new object[] { path, @"\", now.Year, "-", now.Month });
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    path = string.Concat(new object[] { path, @"\", now.Year, "-", now.Month, "-", now.Day, ".exp" });
                    if (!File.Exists(path))
                    {
                        File.CreateText(path).Close();
                    }
                    haveExp = false;
                    break;
                }
                catch (Exception ex)
                {
                    errMsg = ex.Message;
                    Thread.Sleep(200);
                }
            }
            if (haveExp)
            {
                path = "";
                throw new Exception(errMsg + " 创建日志目录失败。");
            }
            return path;
        }

        /// <summary>
        /// 对错误信息进行格式化输出
        /// </summary>
        /// <param name="errMsg"></param>
        /// <returns></returns>
        private string GetFullInfo(string errMsg)
        {
            return ("[异常类型:" + this._logClass + " " + this.GetTime() + "]\r\n" + errMsg + "\r\n\r\n");
        }

        /// <summary>
        /// 获取当前时间的格式化
        /// </summary>
        /// <returns></returns>
        private string GetTime()
        {
            return DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        }

        #endregion
    }
}
