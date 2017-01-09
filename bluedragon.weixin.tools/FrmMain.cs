using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace bluedragon.weixin.tools.ui
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
            LoadMenu();
        }

        /// <summary>
        /// 加载菜单
        /// </summary>
        private void LoadMenu()
        {
            //1.获取数据源
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "dataMenu.xml";
            XDocument doc = XDocument.Load(filePath);
            //2.与相应的树/菜单控件进行绑定
            var pMenus = from m in doc.Root.Elements("Item")
                         select m;
            foreach (var pMenu in pMenus)
            {
                var pId = pMenu.Element("ID").Value;
                var pText = pMenu.Element("Text").Value;
                //添加父菜单
                var pNode = tvMenu.Nodes.Add(pId, pText);
                var pMenuItem = (ToolStripMenuItem)msMenu.Items.Add(pText);
                var cMenus = from m in pMenu.Elements("SubItem")
                    select m;
                //添加子菜单及绑定事件
                foreach (var cMenu in cMenus)
                {
                    var cId = cMenu.Element("ID").Value;
                    var cText = cMenu.Element("Text").Value;
                    var assemblyFile = cMenu.Element("AssemblyFile").Value;
                    var typeName = cMenu.Element("TypeName").Value;
                    pNode.Nodes.Add(cId, cText);
                    var cMenuItem = (ToolStripMenuItem)pMenuItem.DropDownItems.Add(cText);
                    if (!string.IsNullOrEmpty(assemblyFile) && !string.IsNullOrEmpty(typeName))
                    {
                        cMenuItem.Click += new EventHandler(delegate
                        {
                            AddTabPage(assemblyFile, typeName, cText);
                        });
                    }
                }
            }
        }

        /// <summary>
        /// 加载TabPage
        /// </summary>
        /// <param name="assemblyFile"></param>
        /// <param name="typeName"></param>
        /// <param name="tabPageName"></param>
        private void AddTabPage(string assemblyFile, string typeName, string tabPageName)
        {
            var ass = Assembly.Load(assemblyFile);
            Type t = ass.GetType(typeName);
            var frm = (Form)Activator.CreateInstance(t);
            TabPage tabPage = tabControl1.TabPages[tabPageName];
            if (tabPage == null)
            {
                tabPage = new TabPage(tabPageName);
                tabPage.Name = tabPageName;
                frm.TopLevel = false;
                frm.Dock = DockStyle.Fill;
                frm.Parent = tabPage;
                frm.FormBorderStyle = FormBorderStyle.None;
                frm.Show();
                tabPage.Controls.Add(frm);
                tabControl1.TabPages.Add(tabPage);
            }
            tabControl1.SelectedTab = tabPage;
        }
    }
}
