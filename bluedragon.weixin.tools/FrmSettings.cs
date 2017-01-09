using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using bluedragon.weixin.tool.common;

namespace bluedragon.weixin.tools.ui
{
    public partial class FrmSettings : Form
    {
        public FrmSettings()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (ControlValidate())
            {
                SaveConfig();
            }
        }
        private void FrmSettings_Load(object sender, EventArgs e)
        {
            txtAppID.Text = ConfigurationHelper.AppId;
            txtAppSecret.Text = ConfigurationHelper.AppSecret;
            txtToken.Text = ConfigurationHelper.Token;
        }

        private void SaveConfig()
        {
            ConfigurationHelper.AddAppSetting("appId", txtAppID.Text);
            ConfigurationHelper.AddAppSetting("appSecret", txtAppSecret.Text);
            ConfigurationHelper.AddAppSetting("token", txtToken.Text);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool ControlValidate()
        {
            if (string.IsNullOrEmpty(txtAppID.Text))
            {
                MessageBox.Show("AppID不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtAppSecret.Text))
            {
                MessageBox.Show("AppSecret不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            if (string.IsNullOrEmpty(txtToken.Text))
            {
                MessageBox.Show("Token不能为空", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            return true;
        }

    }
}
