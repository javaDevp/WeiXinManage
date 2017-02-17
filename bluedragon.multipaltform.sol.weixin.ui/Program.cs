using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Skins;
using DevExpress.UserSkins;

namespace bluedragon.multipaltform.sol.weixin.ui
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            BonusSkins.Register();
            SkinManager.EnableFormSkins();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FrmMenuManage2());
        }
    }
}
