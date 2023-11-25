using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContagem.Model
{
    [Table("image")]
    public class image
    {
        [PrimaryKey, AutoIncrement]
        public int id { get; set; }
        public string? nome { get; set; }
        public double? confiance { get; set; }
        public int? total_count { get; set; }
        public string? path { get; set; }
        public long? group_id { get; set; }
    }
}
