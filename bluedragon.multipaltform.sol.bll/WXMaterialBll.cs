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
    public class WXMaterialBll
    {
        WXDB _wxdb = new WXDB("WXDB");

        public int Add(WXMaterial material)
        {
            return _wxdb.Insert(material);
        }

        public IEnumerable<WXMaterial> GetMaterials()
        {
            return _wxdb.WxMaterials;
        }
    }
}
