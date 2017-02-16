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
    public class WXMaterialBll
    {
        WXDB _wxdb = new WXDB("WXDB");

        public int Add(WXMaterial material)
        {
            return _wxdb.Insert(material);
        }
    }
}
