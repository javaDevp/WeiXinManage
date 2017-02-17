using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluedragon.multipaltform.sol.weixin.entity;
using bluedragon.multipaltform.sol.weixin.repository;
using LinqToDB;

namespace bluedragon.multipaltform.sol.weixin.bll
{
    public class WXMenuBll
    {
        WXDB _wxdb = new WXDB("WXDB");

        public IEnumerable<WXMenu> GetWXMenus()
        {
            return _wxdb.WXMenus;
        }

        public int Save(WXMenu menu)
        {
            //if (menu.MenuID.HasValue)
            //{
            //   return _wxdb.Update(menu);
            //}
            //else
            //{
            //    return (int)(decimal)_wxdb.InsertWithIdentity(menu);
            //}
            return 0;
        }

        public void Delete(WXMenu menu)
        {
            _wxdb.Delete(menu);
        }
    }
}
