using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AppContagem.Model;
using SQLite;

namespace AppContagem.Classes
{
    public class GridResult
    {
        public List<int> grid_xyxy { get; set; }
        public List<int> grid_index { get; set; }
        public List<List<int>> grid_boxes_xyxy { get; set; }
    }

    public class ImageDTO
    {
        public string? _id { get; set; }
        public double? grid_scale { get; set; }
        public double? confiance { get; set; }
        public int? total_count { get; set; }
        public List<GridResult>? grid_results { get; set; }
        public string? annotated_image { get; set; }
        public string? path { get; set; }

        public image ToImage(long groupId)
        {
            return new image
            {
                nome = this._id,
                confiance = this.confiance,
                total_count = this.total_count,
                path = this.path,
                group_id = groupId
            };
        }
    }

    public class ImageReceiveRequest
    {
        public List<ImageDTO> results { get; set; }
    }
}
