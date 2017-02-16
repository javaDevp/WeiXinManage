using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using bluedragon.multipaltform.sol.bll;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;

namespace bluedragon.multipaltform.sol.ui
{
    public partial class FrmMain : BaseForm
    {
        #region 字段
        /// <summary>
        /// 业务类
        /// </summary>
        private SysMenuBll _bll = new SysMenuBll();
        #endregion

        #region 构造函数
        public FrmMain()
        {
            ApplySkin("McSkin");
            InitializeComponent();
            LoadMenu();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 加载菜单
        /// </summary>
        private void LoadMenu()
        {
            //1.获取数据源
            //string filePath = AppDomain.CurrentDomain.BaseDirectory + "dataMenu.xml";
            //XDocument doc = XDocument.Load(filePath);
            //2.与相应的树/菜单控件进行绑定
            //var pMenus = from m in doc.Root.Elements("Item")
            //             select m;
            var menus = _bll.GetMenus().ToList();
            InitRibbonMenus(menus);
        }

        /// <summary>
        /// Ribbon菜单初始化
        /// </summary>
        /// <param name="menus"></param>
        private void InitRibbonMenus(List<entity.SysMenu> menus)
        {
            //Ribbon的菜单项
            var pageMenus = from menu in menus
                where menu.ParentMenuID == 0 && menu.IsUsed == true
                select menu;
            foreach (var pageMenu in pageMenus)
            {
                RibbonPage page = new RibbonPage(pageMenu.MenuTitle);
                ribbonControl1.Pages.Add(page);
                //Ribbon菜单下的功能组
                var groupMenus = from menu in menus
                    where menu.ParentMenuID == pageMenu.MenuID && menu.IsUsed == true
                    select menu;
                foreach (var groupMenu in groupMenus)
                {
                    RibbonPageGroup group = new RibbonPageGroup(groupMenu.MenuTitle);
                    page.Groups.Add(group);
                    //功能组下的功能项
                    var itemMenus = from menu in menus
                                     where menu.ParentMenuID == groupMenu.MenuID && menu.IsUsed == true
                                     select menu;
                    string basePath = AppDomain.CurrentDomain.BaseDirectory;
                    foreach (var itemMenu in itemMenus)
                    {
                        BarLargeButtonItem menuItem = new BarLargeButtonItem();
                        menuItem.Caption = itemMenu.MenuTitle;
                        menuItem.Id = itemMenu.MenuID;
                        menuItem.Glyph = Image.FromFile(basePath + "imgs" + Path.DirectorySeparatorChar + itemMenu.MenuIcon);
                        menuItem.ItemClick += new ItemClickEventHandler(delegate
                        {
                            AddTabPage(itemMenu.AssemblyFile, itemMenu.TypeName, itemMenu.MenuTitle);
                        });
                        group.ItemLinks.Add(menuItem);}
                }
            }
            
        }

        /// <summary>
        /// 递归加载菜单
        /// </summary>
        /// <param name="menuElement"></param>
        /// <param name="menuItem"></param>
        private void AddMenuItem(XElement menuElement, BarItem parentMenuItem)
        {
            var text = menuElement.Element("Text").Value;
            if (parentMenuItem == null)
            {

                //Ribbon的菜单项
                RibbonPage page = new RibbonPage(text);
                ribbonControl1.Pages.Add(page);
                var menuElementChild = menuElement.Elements("SubItem");
                foreach (var ele in menuElementChild)
                {
                    //菜单下面的各个功能项
                    RibbonPageGroup menuGroup = new RibbonPageGroup(ele.Element("Text").Value);
                    page.Groups.Add(menuGroup);
                    BarButtonItem menuItem = new BarButtonItem();
                    menuItem.Caption = ele.Element("Text").Value;
                    menuItem.Name = ele.Element("Text").Value;
                    menuItem.Id = int.Parse(ele.Element("ID").Value);
                    menuItem.ItemClick += new ItemClickEventHandler(delegate
                    {
                        AddTabPage(ele.Element("AssemblyFile").Value, ele.Element("TypeName").Value, text);
                    });
                    menuGroup.ItemLinks.Add(menuItem);
                }
            }
            #region old Version
            //ToolStripMenuItem menuItemChild = null;
            //var text = menuElement.Element("Text").Value;
            //if (menuItem == null)
            //{
            //    menuItemChild = (ToolStripMenuItem)msMenu.Items.Add(text);
            //}
            //else
            //{
            //    menuItemChild = (ToolStripMenuItem)menuItem.DropDownItems.Add(text);
            //}
            //var assemblyFile = menuElement.Element("AssemblyFile") == null ? "" : menuElement.Element("AssemblyFile").Value;
            //var typeName = menuElement.Element("TypeName") == null ? "" : menuElement.Element("TypeName").Value;
            //if (!string.IsNullOrEmpty(assemblyFile) && !string.IsNullOrEmpty(typeName))
            //{
            //    menuItemChild.Click += new EventHandler(delegate
            //    {
            //        AddTabPage(assemblyFile, typeName, text);
            //    });
            //}
            //var menuElementChild = menuElement.Elements("SubItem");
            //foreach (var ele in menuElementChild)
            //{
            //    AddMenuItem(ele, menuItemChild);
            //}
            #endregion
        }

        /// <summary>
        /// 递归加载树节点
        /// </summary>
        /// <param name="menuElement"></param>
        /// <param name="menuNode"></param>
        private void AddTvMenuItem(XElement menuElement, TreeNode menuNode)
        {
            TreeNode menuNodeChild = null;
            var text = menuElement.Element("Text").Value;
            if (menuNode == null)
            {
                menuNodeChild = tvMenu.Nodes.Add(text);
            }
            else
            {
                menuNodeChild = menuNode.Nodes.Add(text);
            }
            var menuElementChild = menuElement.Elements("SubItem");
            foreach (var ele in menuElementChild)
            {
                AddTvMenuItem(ele, menuNodeChild);
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
        #endregion
        
    }
}
