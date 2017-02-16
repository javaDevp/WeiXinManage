using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using bluedragon.multipaltform.sol.weixin.entity;
using LinqToDB;

namespace bluedragon.multipaltform.sol.weixin.repository
{
    public class WXDB : DataContext
    {
        public WXDB(string configurationString)
            :base(configurationString)
        {
            
        }

        public ITable<WXMenu> WXMenus { get { return this.GetTable<WXMenu>(); } }

        public ITable<WXMaterial> WxMaterials { get { return this.GetTable<WXMaterial>(); } }

    }
}
