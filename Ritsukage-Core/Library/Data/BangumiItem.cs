using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ritsukage.Library.Data
{
   
    [Table("BangumiItem"), AutoInitTable]
    public class BangumiItem : DataTable
    {
        [Column("Title"), PrimaryKey]
        public string Title { get; set; }

        [Column("ZHTitle")]
        public string? ZHTitle { get; set; }
        [Column("Broadcast")]
        public string? Broadcast { get; set; }

        [Column("Begin"), Indexed, NotNull]
        public DateTime Begin { get; set; }

        [Column("End")]
        public DateTime? End { get; set; }
    }
}
