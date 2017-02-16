using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Mapping;

namespace bluedragon.multipaltform.sol.entity
{
    [Table("WX_Material")]
    public class WXMaterial
    {
        [Column("media_id")]
        public string MediaId { get; set; }

        [Column("type")]
        public string Type { get; set; }

        [Column("url")]
        public string Url { get; set; }
    }
}
