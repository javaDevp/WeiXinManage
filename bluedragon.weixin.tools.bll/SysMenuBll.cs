using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluedragon.multipaltform.sol.entity;
using bluedragon.multipaltform.sol.repository;

namespace bluedragon.multipaltform.sol.bll
{
    public class SysMenuBll
    {
        SysDB _sysdb = new SysDB("WXDB");

        public IEnumerable<SysMenu> GetMenus()
        {
            return _sysdb.SysMenus;
        }
    }
}
