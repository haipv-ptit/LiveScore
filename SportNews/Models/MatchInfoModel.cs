using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class MatchInfoModel
    {
        public long MatchId { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public string DateMatch { get; set; }
        public string TimeMatch { get; set; }
        public string HomeFlag { get; set; }
        public string AwayFlag { get; set; }
        public string StatusMatch { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public int RoundInfo { get; set; }
        public long HomeId { get; set; }
        public long AwayId { get; set; }
        public string timeSt { get; set; }

        public List<StandingModel> StdGroup;
    }
}