using SportNews.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using SportNews.Constants;
using MySql.Data.MySqlClient;
using SportNews.Utility;
using System.Data.Common;

namespace SportNews.Controllers
{
    public class HomeController : Controller
    {
        List<MatchInfoModel> matches = new List<MatchInfoModel>();
        MySqlConnection connection = DbUtil.GetDBConnection();
        MySqlCommand cmd = new MySqlCommand();
        //List<MatchInfoModel> rounds = new List<MatchInfoModel>();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Wc18Detail()
        {
            ViewBag.Message = "World cup 2018 Info";
            return View("_Wc18InfoPartial", matches);
        }

        [HttpGet]
        public ActionResult MatchInfo18()
        {
            string json = string.Empty;
            string JsonText = FlagsConst.ftLink;
            WcInfoModel info = new WcInfoModel();
            info.RoundLst = new List<RoundInfo>();
            //using (StreamReader reader = new StreamReader(JsonText))
            //{
            //    json = reader.ReadToEnd();
            //}

            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(JsonText);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.Deserialize<dynamic>(json);
            int len = obj["tournaments"].Length;

            for (int i = 0; i < len; i++)
            {
                int lende = obj["tournaments"][i]["events"].Length;
                for (int j = 0; j < lende; j++)
                {
                    var match = new MatchInfoModel();
                    match.MatchId = obj["tournaments"][i]["events"][j]["id"];
                    match.HomeTeam = obj["tournaments"][i]["events"][j]["homeTeam"]["name"];
                    match.AwayTeam = obj["tournaments"][i]["events"][j]["awayTeam"]["name"];
                    //HomeFlag = obj["object"]["tournaments"]["events"][i]["firstName"],
                    //AwayFlag = obj["object"]["tournaments"]["events"][i]["firstName"],
                    match.HomeId = obj["tournaments"][i]["events"][j]["homeTeam"]["id"];
                    match.AwayId = obj["tournaments"][i]["events"][j]["awayTeam"]["id"];
                    match.HomeFlag = FlagsConst.ptLink + match.HomeId.ToString() + ".png";
                    match.AwayFlag = FlagsConst.ptLink + match.AwayId.ToString() + ".png";
                    try
                    {
                        int hsc = obj["tournaments"][i]["events"][j]["homeScore"]["current"] == null ? "?" : obj["tournaments"][i]["events"][j]["homeScore"]["current"];
                        match.HomeScore = hsc.ToString();
                        int asc = obj["tournaments"][i]["events"][j]["awayScore"]["current"] == null ? "?" : obj["tournaments"][i]["events"][j]["awayScore"]["current"];
                        match.AwayScore = asc.ToString();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        match.HomeScore = "?";
                        match.AwayScore = "?";
                    }
                    double time = obj["tournaments"][i]["events"][j]["startTimestamp"];
                    match.timeSt = unixConvert(time);
                    //StatusMatch = obj["object"]["tournaments"]["events"][i]["firstName"],
                    match.RoundInfo = obj["tournaments"][i]["events"][j]["roundInfo"]["round"];
                    //TimeMatch = obj["object"]["tournaments"]["events"][i]["firstName"],
                    //DateMatch = obj["object"]["tournaments"]["events"][i]["firstName"]

                    matches.Add(match);
                }
                //i++;
            }
            int k = 0;
            for (int i = 1; i <= 8; i++)
            {
                RoundInfo ri = new RoundInfo();
                ri.RoundId = i;
                ri.Schelst = new List<MatchInfoModel>();
                int lenr = 0;
                switch (i)
                {
                    case 1:
                        ri.RoundName = "Round 1";
                        lenr = 15;
                        break;
                    case 2:
                        ri.RoundName = "Round 2";
                        lenr = 31;
                        break;
                    case 3:
                        ri.RoundName = "Round 3";
                        lenr = 47;
                        break;
                    case 4:
                        ri.RoundName = "Round 1/16";
                        lenr = 55;
                        break;
                    case 5:
                        ri.RoundName = "Quarter-Final";
                        lenr = 59;
                        break;
                    case 6:
                        ri.RoundName = "Semi-Final";
                        lenr = 61;
                        break;
                    case 7:
                        ri.RoundName = "3rd";
                        lenr = 62;
                        break;
                    case 8:
                        ri.RoundName = "Final";
                        lenr = 63;
                        break;
                }
                for (int a = k; a <= lenr; a++)
                {
                    ri.Schelst.Add(matches[a]);
                }
                k = lenr + 1;
                info.RoundLst.Add(ri);
            }
            return PartialView("_MatchInfo18", info);
        }

        public ActionResult Standing18()
        {
            //ViewBag.Message = "World cup 2018 Info";
            string json = string.Empty;
            string JsonText = FlagsConst.stdLink;
            MatchInfoModel mim = new MatchInfoModel();
            mim.StdGroup = new List<StandingModel>();
            //using (StreamReader reader = new StreamReader(JsonText))
            //{
            //    json = reader.ReadToEnd();
            //}

            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(JsonText);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.Deserialize<dynamic>(json);
            int len = obj.Length;

            for (int i = 0; i < len; i++)
            {
                StandingModel std = new StandingModel();
                std.name = obj[i]["name"];
                std.id = obj[i]["id"];
                std.listStand = new List<GroupTeam>();
                int lende = obj[i]["tableRows"].Length;
                for (int j = 0; j < lende; j++)
                {
                    var match = new GroupTeam();
                    match.TeamName = obj[i]["tableRows"][j]["team"]["name"];
                    match.TeamId = obj[i]["tableRows"][j]["team"]["id"];
                    match.TeamFlag = FlagsConst.ptLink + match.TeamId.ToString() + ".png";
                    match.Position = obj[i]["tableRows"][j]["position"];
                    match.matchesTotal = obj[i]["tableRows"][j]["totalFields"]["matchesTotal"];
                    match.winTotal = obj[i]["tableRows"][j]["totalFields"]["winTotal"];
                    match.drawTotal = obj[i]["tableRows"][j]["totalFields"]["drawTotal"];
                    match.lossTotal = obj[i]["tableRows"][j]["totalFields"]["lossTotal"];
                    match.goalsTotal = obj[i]["tableRows"][j]["totalFields"]["goalsTotal"];
                    match.goalDiffTotal = obj[i]["tableRows"][j]["totalFields"]["goalDiffTotal"];
                    match.pointsTotal = obj[i]["tableRows"][j]["totalFields"]["pointsTotal"];

                    std.listStand.Add(match);
                }
                mim.StdGroup.Add(std);
            }
            return PartialView("_Standing18", mim);
        }

        public string unixConvert(double x)
        {
            System.DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(x);
            string printDate = dateTime.ToString("dd/M/yyyy", null) + " " + dateTime.ToShortTimeString();
            return printDate;
        }

        public ActionResult MatchDetail(long id)
        {
            string json = string.Empty;
            string jsonGo = string.Empty;
            string jsonMa = string.Empty;
            string jsonSts = string.Empty;

            string JsonText = FlagsConst.lineuplnk + id + "/lineups";
            string JsonTextGo = FlagsConst.GoalLnk + id + "/incidents";
            string JsonTextMa = FlagsConst.matchLnk + id + "/details";
            string JsonTextSts = FlagsConst.statsLnk + id + "/statistics";

            Formation info = new Formation();
            info.homeLst = new List<PlayerInfo>();
            info.awayLst = new List<PlayerInfo>();
            info.HomeLineLst = new List<LineInfo>();
            info.AwayLineLst = new List<LineInfo>();
            info.subHomeLst = new List<PlayerInfo>();
            info.subAwayLst = new List<PlayerInfo>();
            info.goalHomeLst = new List<Incidents>();
            info.goalAwayLst = new List<Incidents>();
            info.goalLst = new List<Incidents>();
            info.statsInfo = new List<lineStats>();

            info.photoHome = FlagsConst.PhotoPlay + id + "/jersey/home/player/image";
            info.photoAway = FlagsConst.PhotoPlay + id + "/jersey/away/player/image";

            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(JsonText);
                jsonGo = wc.DownloadString(JsonTextGo);
                jsonMa = wc.DownloadString(JsonTextMa);
                jsonSts = wc.DownloadString(JsonTextSts);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.Deserialize<dynamic>(json);
            var objGo = jss.Deserialize<dynamic>(jsonGo);
            var objMa = jss.Deserialize<dynamic>(jsonMa);
            var objSts = jss.Deserialize<dynamic>(jsonSts);

            int lenHome = obj["home"].Length;
            int lenAway = obj["away"].Length;
            int lenGo = objGo.Length;
            int lenSt = objSts.Count;

            //info.FtResult = objGo[0]["text"];
            info.HomeName = objMa["game"]["tournaments"][0]["events"][0]["homeTeam"]["name"];
            info.AwayName = objMa["game"]["tournaments"][0]["events"][0]["awayTeam"]["name"];
            long homeId = objMa["game"]["tournaments"][0]["events"][0]["homeTeam"]["id"];
            long awayId = objMa["game"]["tournaments"][0]["events"][0]["awayTeam"]["id"];

            info.HomeFlag = FlagsConst.ptLink + homeId.ToString() + ".png";
            info.AwayFlag = FlagsConst.ptLink + awayId.ToString() + ".png";
            try
            {
                int hsc = objMa["game"]["tournaments"][0]["events"][0]["homeScore"]["current"] == null ? "?" : objMa["game"]["tournaments"][0]["events"][0]["homeScore"]["current"];
                info.HomeScore = hsc.ToString();
                int asc = objMa["game"]["tournaments"][0]["events"][0]["awayScore"]["current"] == null ? "?" : objMa["game"]["tournaments"][0]["events"][0]["awayScore"]["current"];
                info.AwayScore = asc.ToString();
            }
            catch (KeyNotFoundException ex)
            {
                info.HomeScore = "?";
                info.AwayScore = "?";
            }

            if (lenHome > 0 && lenAway > 0)
            {
                info.homeFormation = obj["homeFormation"];
                info.awayFormation = obj["awayFormation"];
                info.lenghHome = chkleng(info.homeFormation).Count;
                info.lenghAway = chkleng(info.awayFormation).Count;
                for (int i = 0; i < lenHome; i++)
                {
                    PlayerInfo pl = new PlayerInfo();
                    pl.PlayerName = obj["home"][i]["player"]["name"];
                    pl.shortName = obj["home"][i]["player"]["shortName"];
                    pl.PlayerId = obj["home"][i]["player"]["id"];
                    try
                    {
                        pl.position = obj["home"][i]["position"];
                    }
                    catch (Exception ex)
                    {
                        pl.position = i + 1;
                    }

                    try
                    {
                        int hsc = obj["home"][i]["shirtNumber"] == null ? "" : obj["home"][i]["shirtNumber"];
                        pl.shirtNumber = hsc.ToString();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        pl.shirtNumber = "";
                    }
                    pl.substitute = obj["home"][i]["substitute"];
                    pl.positionName = obj["home"][i]["positionName"];
                    pl.captain = obj["home"][i]["captain"];
                    info.homeLst.Add(pl);
                }

                for (int j = 0; j < lenAway; j++)
                {
                    PlayerInfo pl = new PlayerInfo();
                    pl.PlayerName = obj["away"][j]["player"]["name"];
                    pl.shortName = obj["away"][j]["player"]["shortName"];
                    pl.PlayerId = obj["away"][j]["player"]["id"];
                    try
                    {
                        pl.position = obj["away"][j]["position"];
                    }
                    catch (Exception ex)
                    {
                        pl.position = j + 1;
                    }

                    try
                    {
                        int hsc = obj["home"][j]["shirtNumber"] == null ? "" : obj["home"][j]["shirtNumber"];
                        pl.shirtNumber = hsc.ToString();
                    }
                    catch (KeyNotFoundException ex)
                    {
                        pl.shirtNumber = "";
                    }
                    pl.substitute = obj["away"][j]["substitute"];
                    pl.positionName = obj["away"][j]["positionName"];
                    pl.captain = obj["away"][j]["captain"];
                    info.awayLst.Add(pl);
                }
                int k = 0, k2 = 0;
                for (int i = 0; i < info.lenghHome; i++)
                {
                    LineInfo li = new LineInfo();
                    li.PlayerLine = new List<PlayerInfo>();
                    li.lineLengh = chkleng(info.homeFormation)[i] + k;
                    for (int a = k; a < li.lineLengh; a++)
                    {
                        li.PlayerLine.Add(info.homeLst[a]);
                    }
                    k = li.lineLengh;
                    info.HomeLineLst.Add(li);
                }

                for (int i = 0; i < info.lenghAway; i++)
                {
                    LineInfo li = new LineInfo();
                    li.PlayerLine = new List<PlayerInfo>();
                    li.lineLengh = chkleng(info.awayFormation)[i] + k2;
                    for (int a = k2; a < li.lineLengh; a++)
                    {
                        li.PlayerLine.Add(info.awayLst[a]);
                    }
                    k2 = li.lineLengh;
                    info.AwayLineLst.Add(li);
                }

                for (int i = 11; i < lenHome; i++)
                {
                    info.subHomeLst.Add(info.homeLst[i]);
                }

                for (int j = 11; j < lenAway; j++)
                {
                    info.subAwayLst.Add(info.awayLst[j]);
                }
            }

            if (lenGo > 0)
            {
                for (int a = lenGo - 1; a >= 0; a--)
                {
                    Incidents idt = new Incidents();
                    idt.InciType = objGo[a]["incidentType"];
                    if (idt.InciType == "goal")
                    {
                        idt.time = objGo[a]["time"];
                        idt.scoringTeam = objGo[a]["scoringTeam"];
                        idt.Player = objGo[a]["player"]["name"];
                        try
                        {
                            string hsc = objGo[a]["from"] == null ? "" : objGo[a]["from"];
                            idt.from = hsc.ToString();
                        }
                        catch (KeyNotFoundException ex)
                        {
                            idt.from = "";
                        }
                        info.goalLst.Add(idt);
                        //if (idt.scoringTeam == 1)
                        //{
                        //    info.goalHomeLst.Add(idt);
                        //}else
                        //{
                        //    info.goalAwayLst.Add(idt);
                        //}
                    }
                    else if (idt.InciType == "card")
                    {
                        idt.time = objGo[a]["time"];
                        idt.scoringTeam = objGo[a]["playerTeam"];
                        idt.Player = objGo[a]["player"]["name"];
                        idt.type = objGo[a]["type"];
                        info.goalLst.Add(idt);
                    }
                    else if (idt.InciType == "substitution")
                    {
                        idt.time = objGo[a]["time"];
                        idt.scoringTeam = objGo[a]["playerTeam"];
                        idt.PlayerIn = objGo[a]["playerIn"]["name"];
                        idt.PlayerOut = objGo[a]["playerOut"]["name"];
                        info.goalLst.Add(idt);
                    }
                    else if (idt.InciType == "period")
                    {
                        idt.content = objGo[a]["text"];
                        info.goalLst.Add(idt);
                    }
                }
            }

            if (lenSt > 0)
            {
                int realeng = objSts["periods"][0]["groups"].Length;
                for (int i = 0; i < realeng; i++)
                {
                    lineStats ls = new lineStats();
                    ls.groupName = objSts["periods"][0]["groups"][i]["groupName"];
                    ls.statItems = new List<Stats>();
                    int lengr = objSts["periods"][0]["groups"][i]["statisticsItems"].Length;
                    for (int j = 0; j < lengr; j++)
                    {
                        Stats st = new Stats();
                        st.name = objSts["periods"][0]["groups"][i]["statisticsItems"][j]["name"];
                        st.home = objSts["periods"][0]["groups"][i]["statisticsItems"][j]["home"];
                        st.away = objSts["periods"][0]["groups"][i]["statisticsItems"][j]["away"];
                        ls.statItems.Add(st);
                    }
                    info.statsInfo.Add(ls);
                }
            }
            return PartialView("_MatchDetail", info);
        }

        public List<int> chkleng(string str)
        {
            List<int> kq = new List<int>();
            kq.Add(1);
            //var x = str.ToArray();
            foreach (var i in str)
            {
                if (Char.IsNumber(i))
                {
                    kq.Add(int.Parse(i.ToString()));
                }
            }
            return kq;
        }

        [HttpGet]
        public ActionResult SearchBoard()
        {
            newslist nl = new newslist();
            nl.ctLst = new List<ContentModel>();
            string sql = @"select a.news_id, a.title, a.description, a.image, b.category_name 
                           from news a 
                           join category b on a.cat_id = b.category_id
                           order by a.news_id desc  
                           limit 10 ";
            connection.Open();
            int count = 0;

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ++count;
                            ContentModel cm = new ContentModel();
                            cm.news_id = Convert.ToInt64(reader.GetValue(0));
                            cm.title = reader.GetString(1);
                            cm.descp = reader.GetString(2);
                            try
                            {
                                cm.imageLnk = reader.GetString(3);
                            }
                            catch (Exception ex)
                            {
                                cm.imageLnk = "";
                            }
                            cm.cat_name = reader.GetString(4);
                            cm.Stt = count;
                            //cm.category_id = Convert.ToInt64(reader.GetValue(0));
                            nl.ctLst.Add(cm);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + sql);
                Console.WriteLine(e.StackTrace);
            }
            return PartialView("_NewsBoard", nl);
        }

        [HttpGet]
        public ActionResult NewsDetail(long id)
        {
            ContentModel cm = new ContentModel();
            string sql = @"select a.news_id, a.title, a.description, a.image, b.category_name 
                           from news a 
                           join category b on a.cat_id = b.category_id
                           where a.news_id = " + id;
            connection.Open();

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            cm.news_id = Convert.ToInt64(reader.GetValue(0));
                            cm.title = reader.GetString(1);
                            cm.descp = reader.GetString(2);
                            try
                            {
                                cm.imageLnk = reader.GetString(3);
                            }
                            catch (Exception ex)
                            {
                                cm.imageLnk = "";
                            }
                            cm.cat_name = reader.GetString(4);
                            //cm.category_id = Convert.ToInt64(reader.GetValue(0));
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + sql);
                Console.WriteLine(e.StackTrace);
            }
            return PartialView("_NewsDetail", cm);
        }

        [HttpGet]
        public ActionResult FullNews()
        {
            newslist nl = new newslist();
            nl.ctLst = new List<ContentModel>();
            string sql = @"select a.news_id, a.title, a.description, a.image, b.category_name 
                           from news a 
                           join category b on a.cat_id = b.category_id";
            connection.Open();
            int count = 0;

            try
            {
                cmd.Connection = connection;
                cmd.CommandText = sql;
                using (DbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            ++count;
                            ContentModel cm = new ContentModel();
                            cm.news_id = Convert.ToInt64(reader.GetValue(0));
                            cm.title = reader.GetString(1);
                            cm.descp = reader.GetString(2);
                            try
                            {
                                cm.imageLnk = reader.GetString(3);
                            }
                            catch (Exception ex)
                            {
                                cm.imageLnk = "";
                            }
                            cm.cat_name = reader.GetString(4);
                            cm.Stt = count;
                            //cm.category_id = Convert.ToInt64(reader.GetValue(0));
                            nl.ctLst.Add(cm);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + sql);
                Console.WriteLine(e.StackTrace);
            }
            return PartialView("_FullNews", nl);
        }

        public ActionResult SoiKeo(long id)
        {
            //ViewBag.Message = "World cup 2018 Info";
            string json = string.Empty;
            string JsonText = FlagsConst.matchLnk + id + "/odds";
            SumLst ct = new SumLst();
            ct.LstAll = new List<CatOdds>();
            //using (StreamReader reader = new StreamReader(JsonText))
            //{
            //    json = reader.ReadToEnd();
            //}

            using (WebClient wc = new WebClient())
            {
                json = wc.DownloadString(JsonText);
            }

            JavaScriptSerializer jss = new JavaScriptSerializer();
            var obj = jss.Deserialize<dynamic>(json);
            int len = obj.Length;

            for (int i = 0; i < len - 1; i++)
            {
                CatOdds std = new CatOdds();
                std.OddLst = new List<OddsRate>();
                std.name = obj[i]["name"];
                int lende = obj[i]["regular"][0]["odds"].Length;
                //for (int j = 0; j < lende; j++)
                //{
                var match = new OddsRate();
                if (i != len - 1)
                {
                    if (lende == 3)
                    {
                        match.HomeWn = obj[i]["regular"][0]["odds"][lende - 3]["decimalValue"];
                        match.Draw = obj[i]["regular"][0]["odds"][lende - 2]["decimalValue"];
                        match.AwayWn = obj[i]["regular"][0]["odds"][lende - 1]["decimalValue"];
                        match.HomeChoice = obj[i]["regular"][0]["odds"][lende - 3]["choice"];
                        match.DrawChoice = obj[i]["regular"][0]["odds"][lende - 2]["choice"];
                        match.AwayChoice = obj[i]["regular"][0]["odds"][lende - 1]["choice"];
                    }
                    else
                    {
                        match.HomeWn = obj[i]["regular"][0]["odds"][lende - 2]["decimalValue"];
                        match.AwayWn = obj[i]["regular"][0]["odds"][lende - 1]["decimalValue"];
                        match.HomeChoice = obj[i]["regular"][0]["odds"][lende - 2]["choice"];
                        match.AwayChoice = obj[i]["regular"][0]["odds"][lende - 1]["choice"];

                    }
                }
                else
                {
                    match.GoaLst = new List<MatchGoals>();
                    var lenfina = obj[len - 1]["regular"].Length;
                    for (int j = 0; j < lenfina; j++)
                    {
                        var mt = new MatchGoals();
                        mt.goal = obj[i]["regular"][0]["handicap"];
                    }
                }


                std.OddLst.Add(match);
                //}
                ct.LstAll.Add(std);
            }

            return PartialView("_SoiKeo", ct);
        }
    }
}