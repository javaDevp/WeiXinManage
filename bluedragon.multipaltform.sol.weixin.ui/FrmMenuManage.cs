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
using bluedragon.weixin.tool.common;

using bluedragon.multipaltform.sol.weixin.bll;
using bluedragon.multipaltform.sol.weixin.entity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace bluedragon.multipaltform.sol.weixin.ui
{
    public partial class FrmMenuManage : Form
    {
        #region Fields
        private WXMenuBll _bll = new WXMenuBll();
        #endregion

        #region Constructors
        public FrmMenuManage()
        {
            InitializeComponent();
            dataGridView1.AutoGenerateColumns = false;
            LoadData();
        }
        #endregion

        #region Methdods

        /// <summary>
        /// 获取微信菜单数据（临时Json)
        /// </summary>
        private void LoadData()
        {
            //using (TextReader tr = File.OpenText(AppDomain.CurrentDomain.BaseDirectory + "dataWXMenu.json"))
            //{
            //    using (JsonReader jr = new JsonTextReader(tr))
            //    {
            //        JObject obj = JObject.Load(jr);
            //        AddMenuToTv(null, obj);
            //    }
            //}
            IList<WXMenu> menus = _bll.GetWXMenus().ToList();
            //
            //dataGridView1.DataSource = menus.ToList();

            AddMenuToTv(null, menus);

        }

        /// <summary>
        /// 加载微信菜单到TreeView中（临时Json）
        /// </summary>
        /// <param name="pNode"></param>
        /// <param name="obj"></param>
        private void AddMenuToTv(TreeNode pNode, IEnumerable<WXMenu> menus)
        {
            try
            {
                tvMenus.Nodes.Clear();
                var rootMenus = from menu in menus
                                where menu.ParentMenuID == 0
                                select menu;
                foreach (var rootMenu in rootMenus)
                {
                    TreeNode node = new TreeNode(rootMenu.MenuTitle);
                    node.Tag = rootMenu.MenuID;
                    tvMenus.Nodes.Add(node);
                    var childMenus = from menu in menus
                                     where menu.ParentMenuID == rootMenu.MenuID
                                     select menu;
                    foreach (var childMenu in childMenus)
                    {
                        TreeNode childNode = new TreeNode(childMenu.MenuTitle);
                        childNode.Tag = childMenu.MenuID;
                        node.Nodes.Add(childNode);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// 添加快捷菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiAdd_Click(object sender, EventArgs e)
        {
            IEnumerable<WXMenu> menus = _bll.GetWXMenus().ToList();
            var selectedNode = tvMenus.SelectedNode;
            if (selectedNode != null)
            {
                if (selectedNode.Nodes.Count > 0 &&
                    menus.Any(m => m.ParentMenuID == 0 &&
                        m.MenuID == int.Parse(selectedNode.Tag.ToString())))
                {
                    var newNode = new TreeNode();
                    newNode.Tag = 0;
                    selectedNode.Nodes.Add(newNode);
                    tvMenus.SelectedNode = newNode;
                }
            }
        }

        /// <summary>
        /// 删除快捷菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                        IEnumerable<WXMenu> menus = (from menu in _bll.GetWXMenus()
                                                     where menu.MenuID == int.Parse(selectedNode.Tag.ToString())
                                                     select menu).ToList();
                        _bll.Delete(menus.ElementAt(0));
                        selectedNode.Remove();
                    }
                }
            }
            else
            {
                MessageBox.Show("请先选择要删除的节点");
            }
        }

        /// <summary>
        /// 选中Treeview节点事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvMenus_AfterSelect(object sender, TreeViewEventArgs e)
        {
            var selectedNode = tvMenus.SelectedNode;
            IEnumerable<WXMenu> menus = (from menu in _bll.GetWXMenus()
                                         where menu.MenuID == int.Parse(selectedNode.Tag.ToString())
                                         select menu).ToList();
            dataGridView1.DataSource = menus.Count() == 0 ? new List<WXMenu>() { new WXMenu() { ParentMenuID = int.Parse(selectedNode.Parent.Tag.ToString()) } } : menus;
        }

        #endregion

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            WXMenu menu = ((IEnumerable<WXMenu>) dataGridView1.DataSource).ElementAt(0);
            _bll.Save(menu);
            LoadData();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                WeiXinHelper weiXinHelper = new WeiXinHelper();
                string menuJson = BuildWxMenuJson();
                weiXinHelper.CreateMenus(menuJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string BuildWxMenuJson()
        {
            var menus = _bll.GetWXMenus().ToList();
            StringBuilder sb = new StringBuilder();
            sb.Append("{");
            sb.Append("\"button\": [");
            foreach (var menu in menus.Where(m => m.ParentMenuID == 0))
            {
                sb.Append("{");
                sb.AppendFormat("\"name\": \"{0}\", ", menu.MenuTitle);
                sb.Append(" \"sub_button\": [");

                foreach (var childMenu in menus.Where(m => m.ParentMenuID == menu.MenuID))
                {
                    sb.Append("{");
                    sb.AppendFormat("\"type\": \"{0}\",", childMenu.MenuType);
                    sb.AppendFormat("\"name\": \"{0}\", ", childMenu.MenuTitle);
                    switch (childMenu.MenuType)
                    {
                        case "view":
                            sb.AppendFormat("\"url\": \"{0}\"", childMenu.Url);
                            break;
                        case "click":
                            sb.AppendFormat("\"key\": \"{0}\"", childMenu.Key);
                            break;
                        case "scancode_waitmsg":
                            sb.AppendFormat("\"key\": \"{0}\",", childMenu.Key);
                            sb.Append("\"sub_button\": [ ]");
                            break;
                    }
                    sb.Append("},");
                }
                sb.Remove(sb.Length - 1, 1);
                sb.Append("]");
                sb.Append("},");
            }

            sb.Remove(sb.Length - 1, 1);
            sb.Append("]");
            sb.Append("}");
            return sb.ToString();
        }

        private void tvMenus_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                //确定右键的位置  
                Point clickPoint = new Point(e.X, e.Y);
                //在确定后的位置上面定义一个节点  
                TreeNode treeNode = tvMenus.GetNodeAt(clickPoint);
                tvMenus.SelectedNode = treeNode;
            }
        }
        
    }
}
