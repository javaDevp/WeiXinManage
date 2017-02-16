using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluedragon.multipaltform.sol.entity;
using LinqToDB;

namespace bluedragon.multipaltform.sol.repository
{
    public class SysDB : DataContext
    {
        public SysDB(string configurationString)
            :base(configurationString)
        {
            
        }

        public ITable<SysMenu> SysMenus { get { return this.GetTable<SysMenu>(); } }
    }
}
