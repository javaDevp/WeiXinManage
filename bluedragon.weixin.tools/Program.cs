using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using bluedragon.weixin.tool.common;
using bluedragon.multipaltform.sol.ui;
using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace bluedragon.multipaltform.sol.ui
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                BonusSkins.Register();
                SkinManager.EnableFormSkins();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //判断是否配置了appId & appSecret & token
                if (!ConfigurationHelper.IsAppSettingExist("appId")
                    || !ConfigurationHelper.IsAppSettingExist("appSecret")
                    || !ConfigurationHelper.IsAppSettingExist("token"))
                {
                    FrmSettings frm = new FrmSettings();
                    if (DialogResult.OK == frm.ShowDialog())
                    {
                        Application.Run(new FrmMain());
                    }
                }
                else
                {
                    Application.Run(new FrmMain());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "出错了", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
