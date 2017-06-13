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
using bluedragon.multipaltform.sol.weixin.bll;
using bluedragon.multipaltform.sol.weixin.entity;
using Newtonsoft.Json.Linq;

namespace bluedragon.multipaltform.sol.weixin.ui
{
    public partial class FrmMaterialManage : Form
    {
        private WXMaterialBll _bll = new WXMaterialBll();

        public FrmMaterialManage()
        {
            InitializeComponent();
        }

        private void FrmMaterialManage_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void RefreshData()
        {
            gridControl1.DataSource = _bll.GetMaterials();
        }

        private void btnUploadImg_Click(object sender, EventArgs e)
        {
            UploadMaterial("image");
            RefreshData();
        }

        private void UploadMaterial(string type)
        {
            WeiXinHelper helper = new WeiXinHelper();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (DialogResult.OK == openFileDialog.ShowDialog(this))
            {
                string strJson = helper.AddPermanentMaterial(type, openFileDialog.FileName);
                if (strJson.Contains("media_id"))
                {
                    JObject obj = JObject.Parse(strJson);
                    WXMaterial material = new WXMaterial()
                    {
                        MediaId = obj["media_id"].ToString(),
                        Type = type,
                        Url = obj["url"].ToString()
                    };
                    _bll.Add(material);
                }
            }
        }
    }
}
