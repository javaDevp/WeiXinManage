using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bluedragon.weixin.tools.ui
{
    public partial class FrmMenuManage : Form
    {
        public FrmMenuManage()
        {
            InitializeComponent();
            LoadData();
        }

        /// <summary>
        /// 获取微信菜单数据（临时Json)
        /// </summary>
        private void LoadData()
        {
            using (TextReader tr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "dataWXMenu.json"))
            {
                using (JsonReader jr = new JsonTextReader(tr))
                {
                    JObject obj = JObject.Load(jr);
                    AddMenuToTv(null, obj);
                }
            }
        }

        /// <summary>
        /// 加载微信菜单到TreeView中（临时Json）
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="obj"></param>
        private void AddMenuToTv(TreeNode pNode, JObject obj)
        {
            var rootMenus = from rm in obj["button"]
                            select rm["name"];
            foreach (var rootMenu in rootMenus)
            {
                TreeNode node = new TreeNode(rootMenu.ToString());
                tvMenus.Nodes.Add(node);
                var childMenus = from cm in obj["button"]
                    where cm["name"].ToString() == rootMenu.ToString()
                    select cm["sub_button"];
                foreach (var childMenu in childMenus.First())
                {
                    TreeNode childNode = new TreeNode(childMenu["name"].ToString());
                    node.Nodes.Add(childNode);
                }
            }
        }

        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            var selectedNode = tvMenus.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Nodes.Count > 0)
                {
                    
                }
            }
        }

        private void tsmiDelete_Click(object sender, EventArgs e)
        {
            var selectedNode = tvMenus.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Nodes.Count > 0)
                {
                    //带有子节点
                    if (MessageBox.Show("将删除该菜单及其所有子菜单?", "确认", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        
                    }
                }
                else
                {
                    if (MessageBox.Show("确认删除?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        selectedNode.Remove();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的节点");
            }
        }
    }
}
