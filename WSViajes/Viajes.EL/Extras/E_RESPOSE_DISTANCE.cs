using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Viajes.EL.Extras
{
    public class E_RESPOSE_DISTANCE
    {
        public List<dynamic> geocoded_waypoints { get; set; }
        public List<dynamic> routes { get; set; }
        public string status { get; set; }
    }
}
