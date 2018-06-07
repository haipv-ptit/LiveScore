using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class WcInfoModel
    {
        public List<RoundInfo> RoundLst;
        public SeasonInfo season;
    }

    public class RoundInfo
    {
        public int RoundId { get; set; }
        public string RoundName { get; set; }

        public List<MatchInfoModel> Schelst;
    }
}