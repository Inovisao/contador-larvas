using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppContagem.Classes
{
    public class ImageSendRequest
    {
        public string _id { get; set; }
        public string image { get; set; }
        public double? grid_scale { get; set; }
        public double? confiance { get; set; }
        public bool return_image { get; set; }
    }
}
