using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class StandingModel
    {
        public string name { get; set; }
        public long id { get; set; }
        public List<GroupTeam> listStand;

    }

    public class GroupTeam
    {
        public string TeamName { get; set; }
        public long TeamId { get; set; }
        public string TeamFlag { get; set; }
        public string Position { get; set; }
        public string matchesTotal { get; set; }
        public string winTotal { get; set; }
        public string drawTotal { get; set; }
        public string lossTotal { get; set; }
        public string goalsTotal { get; set; }
        public string goalDiffTotal { get; set; }
        public string pointsTotal { get; set; }
    }
}