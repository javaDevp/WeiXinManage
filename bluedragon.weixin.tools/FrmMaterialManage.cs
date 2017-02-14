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
    public partial class FrmMaterialManage : Form
    {
        public FrmMaterialManage()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WeiXinHelper helper = new WeiXinHelper();
            //richTextBox1.Text = helper.GetMaterialCount();
            pictureBox1.Image = Image.FromStream(helper.GetTempMaterial("ej2jMVrM_d79LvsIib-kKY3tUCioW4H6X5xex0Cul8ZTPQfuNpTz3PCXnYtGQ4hW"));
            //MessageBox.Show(helper.GetTempMaterial("ej2jMVrM_d79LvsIib-kKY3tUCioW4H6X5xex0Cul8ZTPQfuNpTz3PCXnYtGQ4hW"));
        }
    }
}
