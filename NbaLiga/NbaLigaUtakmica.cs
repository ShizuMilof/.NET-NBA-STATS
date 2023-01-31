using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Liga
{
    public class NbaLigaUtakmica
    {

        public int home_team_id { get; set; }
        public string home_team_full_name { get; set; }
       
        public int visitor_team_id { get; set; }
        public string visitor_team_full_name { get; set; }
        public int id { get; set; }
        public string date { get; set; }
        public string home_team_score { get; set; }
        public string season { get; set; }
     
        public string visitor_team_score { get; set; }

    }

}
