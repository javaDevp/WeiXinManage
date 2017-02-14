using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace bluedragon.weixin.tools.entity
{
    [Table("WX_Menu")]
    public class WXMenu
    {
        [Column("Menu_ID", IsPrimaryKey = true, IsIdentity = true)]
        public int? MenuID { get; set; }
        [Column("Menu_Title")]
        public string MenuTitle { get; set; }
        [Column("Menu_Type")]
        public string MenuType { get; set; }
        [Column("Parent_Menu_ID")]
        public int ParentMenuID { get; set; }
        [Column("Media_ID")]
        public string MediaID { get; set; }
        [Column("Key")]
        public string Key { get; set; }
        [Column("Url")]
        public string Url { get; set; }
        [Column("OrderNo")]
        public int OrderNo { get; set; }
    }
}
