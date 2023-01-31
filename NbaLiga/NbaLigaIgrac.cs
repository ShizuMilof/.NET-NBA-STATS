using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga
{
    public  class NbaLigaIgrac
    {

            public int team_id { get; set; }
            public string team_full_name { get; set; }
            public   int ID { get; set; }
            public string first_name { get; set; } 
            public string last_name { get; set; }
            public string height_feet { get; set; }
            public string height_inches { get; set; }
            public string position { get; set; }
            public string weight_pounds { get; set; }
            public Bitmap image { get; set; }

    }
}
