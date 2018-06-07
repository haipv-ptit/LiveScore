using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SportNews.Models
{
    public class Formation
    {
        public long MatchId { get; set; }
        public string homeFormation { get; set; }
        public string awayFormation { get; set; }
        public string homeManager { get; set; }
        public string awayManager { get; set; }
        public List<PlayerInfo> homeLst;
        public List<PlayerInfo> awayLst;
        public string homePlayerColNum { get; set; }
        public string homePlayerColLine { get; set; }
        public string homeGkColNum { get; set; }
        public string homeGkColLine { get; set; }
        public string awayPlayerColNum { get; set; }
        public string awayPlayerColLine { get; set; }
        public string awayGkColNum { get; set; }
        public string awayGkColLine { get; set; }
        public List<Incidents> homeIncidents;
        public List<Incidents> awayIncidents;
        public int lenghHome { get; set; }
        public int lenghAway { get; set; }
        public List<LineInfo> HomeLineLst;
        public List<LineInfo> AwayLineLst;
        public string photoHome { get; set; }
        public string photoAway { get; set; }

        public List<PlayerInfo> subHomeLst;
        public List<PlayerInfo> subAwayLst;
        public string HomeName { get; set; }
        public string AwayName { get; set; }
        public string HomeFlag { get; set; }
        public string AwayFlag { get; set; }

        public List<Incidents> goalHomeLst;
        public List<Incidents> goalAwayLst;
        public List<Incidents> goalLst;
        public string FtResult { get; set; }
        public string HomeScore { get; set; }
        public string AwayScore { get; set; }
        public List<lineStats> statsInfo;
    }

    public class LineInfo
    {
        public int lineLengh { get; set; }
        public List<PlayerInfo> PlayerLine;
    }
}