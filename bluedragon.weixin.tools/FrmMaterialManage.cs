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
using bluedragon.weixin.tools.bll;
using bluedragon.weixin.tools.entity;
using Newtonsoft.Json.Linq;

namespace bluedragon.weixin.tools.ui
{
    public partial class FrmMaterialManage : Form
    {
        private WXMaterialBll _bll = new WXMaterialBll();

        public FrmMaterialManage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WeiXinHelper helper = new WeiXinHelper();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (DialogResult.OK == openFileDialog.ShowDialog(this))
            {
                string strJson = helper.AddPermanentMaterial("image", openFileDialog.FileName);
                if (strJson.Contains("media_id"))
                {
                    JObject obj = JObject.Parse(strJson);
                    WXMaterial material = new WXMaterial()
                    {
                        MediaId = obj["media_id"].ToString(),
                        Type = "image",
                        Url = obj["url"].ToString()
                    };
                    _bll.Add(material);
                }
            }
            //richTextBox1.Text = helper.GetMaterialCount();
            //pictureBox1.Image = Image.FromStream(helper.GetTempMaterial("ej2jMVrM_d79LvsIib-kKY3tUCioW4H6X5xex0Cul8ZTPQfuNpTz3PCXnYtGQ4hW"));
            //MessageBox.Show(helper.GetTempMaterial("ej2jMVrM_d79LvsIib-kKY3tUCioW4H6X5xex0Cul8ZTPQfuNpTz3PCXnYtGQ4hW"));
        }
    }
}
