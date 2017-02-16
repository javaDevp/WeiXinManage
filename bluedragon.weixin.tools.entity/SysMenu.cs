using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace bluedragon.multipaltform.sol.entity
{
    [Table("Sys_Menu")]
    public class SysMenu
    {
        [Column("Menu_ID")]
        public int MenuID { get; set; }
        [Column("Menu_Title")]
        public string MenuTitle { get; set; }
        [Column("Menu_Icon")]
        public string MenuIcon { get; set; }
        [Column("Parent_Menu_ID")]
        public int ParentMenuID { get; set; }
        [Column("Assembly_File")]
        public string AssemblyFile { get; set; }
        [Column("Type_Name")]
        public string TypeName { get; set; }
        [Column("Is_Used")]
        public bool IsUsed { get; set; }
        [Column("Order_No")]
        public int OrderNo { get; set; }
    }
}
