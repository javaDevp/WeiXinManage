﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using bluedragon.weixin.tool.common;
using bluedragon.weixin.tools.ui;

namespace bluedragon.weixin.tools
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //判断是否配置了appId & appSecret & token
            if (!ConfigurationHelper.IsAppSettingExist("appId")
                || !ConfigurationHelper.IsAppSettingExist("appSecret")
                || !ConfigurationHelper.IsAppSettingExist("token"))
            {
                FrmSettings frm = new FrmSettings();
                frm.ShowDialog();
            }
            Application.Run(new FrmMain());
        }
    }
}