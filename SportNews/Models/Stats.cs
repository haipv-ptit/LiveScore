using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class Stats
    {
        //public int homeBallPossession { get; set; }
        //public int awayBallPossession { get; set; }
        //public int homeCornerKicks { get; set; }
        //public int awayCornerKicks { get; set; }
        //public int homeShotsOnGoal { get; set; }
        //public int awayShotsOnGoal { get; set; }
        //public int homeShotsOffGoal { get; set; }
        //public int awayShotsOffGoal { get; set; }
        //public int homeOffsides { get; set; }
        //public int awayOffsides { get; set; }
        //public int homeGoalkeeperSaves { get; set; }
        //public int awayGoalkeeperSaves { get; set; }
        //public int homeFouls { get; set; }
        //public int awayFouls { get; set; }
        //public int homeYellowCards { get; set; }
        //public int awayYellowCards { get; set; }
        //public int homeRedCards { get; set; }
        //public int awayRedCards { get; set; }
        public string home { get; set; }
        public string away { get; set; }
        public string name { get; set; }
    }

    public class lineStats
    {
        public string groupName { get; set; }
        public List<Stats> statItems;
    }
}