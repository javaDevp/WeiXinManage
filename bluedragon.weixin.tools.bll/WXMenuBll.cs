using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluedragon.weixin.tools.entity;
using bluedragon.weixin.tools.repository;
using LinqToDB;

namespace bluedragon.weixin.tools.bll
{
    public class WXMenuBll
    {
        WXDB _wxdb = new WXDB("WXDB");

        public IEnumerable<WXMenu> GetWXMenus()
        {
            return _wxdb.WXMenus;
        }

        public object Save(WXMenu menu)
        {
            if (menu.MenuID.HasValue)
                return _wxdb.Update(menu);
            menu.MenuID = (int)(decimal)_wxdb.InsertWithIdentity(menu);
            return menu.MenuID;
            //_wxdb.InsertWithIdentity()
        }

        public int Delete(WXMenu menu)
        {
            return _wxdb.Delete(menu);
        }
    }
}
