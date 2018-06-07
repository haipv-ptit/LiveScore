using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class OddsRate
    {
        //public decimal FtHomeTeam { get; set; }
        //public decimal FtAwayTeam { get; set; }
        //public decimal FtDraw { get; set; }
        //public decimal DbChanceHome { get; set; }
        //public decimal DbChanceAway { get; set; }
        //public decimal DbChanceNoDraw { get; set; }
        //public decimal DrawNoBtHome { get; set; }
        //public decimal DrawNoBtAway { get; set; }
        //public decimal HtHomeTeam { get; set; }
        //public decimal HtAwayTeam { get; set; }
        //public decimal HtDraw { get; set; }
        //public decimal BothScYes { get; set; }
        //public decimal BothScNo { get; set; }
        //public decimal FirstScHome { get; set; }
        //public decimal FirstScAway { get; set; }
        //public decimal NoScore { get; set; }
        public decimal HomeWn { get; set; }
        public decimal AwayWn { get; set; }
        public decimal Draw { get; set; }
        public string HomeChoice { get; set; }
        public string AwayChoice { get; set; }
        public string DrawChoice { get; set; }
        public List<MatchGoals> GoaLst;
    }

    public class MatchGoals
    {
        public decimal goal { get; set; }
        public decimal OverGo { get; set; }
        public decimal UnderGo { get; set; }
    }

    public class CatOdds
    {
        public string name { get; set; }
        public List<OddsRate> OddLst;
    }

    public class SumLst
    {
        public List<CatOdds> LstAll;
    }
}