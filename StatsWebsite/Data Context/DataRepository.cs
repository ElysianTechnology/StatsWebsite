using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ServiceStack.Text;
using StatsWebsite.Nancy.Models;

namespace StatsWebsite.Data_Context
{
    public class DataRepository
    {
        private readonly AlgoTestContext _context;

        public DataRepository(AlgoTestContext context)
        {
            _context = context;
        }

        public void AddFixture(LeagueData fixture)
        {
            if (
                !_context.LeagueData.Any(x =>
                        x.DateTime == fixture.DateTime
                        && x.HomeTeam == fixture.HomeTeam
                        && x.AwayTeam == fixture.AwayTeam))
            {
                _context.LeagueData.Add(fixture);
            }
        }

        public string GetFTResult(string home, string away)
        {
            var data =
                _context.LeagueData.Where(y => y.HomeTeam.Contains(home) && y.AwayTeam.Contains(away) && y.DateTime >= new DateTime(2015, 07, 30) && y.DateTime < DateTime.Now)
                    .Select(x => x.FullTimeResult)
                    .FirstOrDefault();

            return data;
        }

        public List<TeamStats> LoadTeam(string team)
        {
            var data = _context.LeagueData.Where(y => y.HomeTeam.Contains(team) || y.AwayTeam.Contains(team)).Select(x => new TeamStats
            {
                Team = team,
                League = x.League,
                Location = x.HomeTeam.Contains(team) ? "H" : "A",
                Date = x.DateTime,
                GoalsFor = x.HomeTeam.Contains(team) ? x.FullTimeHomeGoals : x.FullTimeAwayGoals,
                GoalsAgainst = x.HomeTeam.Contains(team) ? x.FullTimeAwayGoals : x.FullTimeHomeGoals,
                FTResult = x.FullTimeResult
            }).OrderByDescending(x => x.Date).Take(10);

            return data.ToList();
        }

        public List<TeamStats> LoadHomeTeam(string team)
        {
            var data = _context.LeagueData.Where(y => y.HomeTeam.Contains(team)).Select(x => new TeamStats
            {
                Team = team,
                Location = "H",
                Date = x.DateTime,
                GoalsFor = x.FullTimeHomeGoals,
                GoalsAgainst = x.FullTimeAwayGoals,
                FTResult = x.FullTimeResult
            }).OrderByDescending(x => x.Date).Take(10);

            return data.ToList();
        }

        public List<TeamStats> LoadAwayTeam(string team)
        {
            var data = _context.LeagueData.Where(y => y.AwayTeam.Contains(team)).Select(x => new TeamStats
            {
                Team = team,
                Location = "A",
                Date = x.DateTime,
                GoalsFor = x.FullTimeAwayGoals,
                GoalsAgainst = x.FullTimeHomeGoals,
                FTResult = x.FullTimeResult
            }).OrderByDescending(x => x.Date).Take(10);

            return data.ToList();
        }

        public List<TeamStats> H2H(string home, string away)
        {
            var data =
                _context.LeagueData.Where(x => x.HomeTeam.Contains(home) && x.AwayTeam.Contains(away)).Select(y => new TeamStats
                    {
                        Team = home + " v " + away,
                        Location = "n/a",
                        Date = y.DateTime,
                        GoalsFor = y.FullTimeHomeGoals,
                        GoalsAgainst = y.FullTimeAwayGoals,
                        FTResult = y.FullTimeResult
                    });

            return data.ToList();
        }

        public List<String> TeamsInLeague(int league)
        {

            var league_ = string.Format("E{0}", league);
            var data =
                _context.LeagueData.Where(x => x.League == league_)
                    .Select(x => x.HomeTeam)
                    .Distinct().ToList();

            return data;

        }

        public NextFixtures GetNextFixtures(int league, int matchdays)
        {
            var i = 426+league;
            
            string WEBSERVICE_URL = string.Format("http://api.football-data.org/v1/competitions/{0}/fixtures?timeFrame=n{1}", i, matchdays);
            try
            {
                var webRequest = System.Net.WebRequest.Create(WEBSERVICE_URL);
                var NextFixtures = new NextFixtures();
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 12000;
                    webRequest.ContentType = "application/json";
                    webRequest.Headers.Add("X-Auth-Token", "f17d593c4bb84a6bbf800fe204f24a9b");

                    using (System.IO.Stream s = webRequest.GetResponse().GetResponseStream())
                    {
                        using (System.IO.StreamReader sr = new System.IO.StreamReader(s))
                        {
                            var jsonResponse = sr.ReadToEnd();
                            var jobject = JsonObject.Parse(jsonResponse);

                            NextFixtures = (NextFixtures)JsonSerializer.DeserializeFromString< NextFixtures > (jsonResponse);
                        }
                    }
                }
                return NextFixtures;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return new NextFixtures();
            }
        }

        public List<LeagueShotStats> LeagueShotStatsHome()
        {
            var data =
                _context.LeagueData.Where(x => x.DateTime >= new DateTime(2015, 08, 08))
                    .GroupBy(x => x.HomeTeam)
                    .Select(x => new LeagueShotStats
                    {
                        Team = x.Key,
                        Shots = x.Sum(y => y.HomeTeamShots),
                        ShotsFaced = x.Sum(y => y.AwayTeamShots),
                        Goals = x.Sum(y => y.FullTimeHomeGoals),
                        GoalsConceded = x.Sum(y => y.FullTimeAwayGoals),
                        OnTarget = x.Sum(y => y.HomeTeamShotsOnTarget),
                        ShotsFacedOnTarget = x.Sum(y => y.AwayTeamShotsOnTarget)
                    }).ToList();

            foreach (var a in data)
            {
                a.League = _context.LeagueData.Where(x => x.HomeTeam == a.Team).OrderByDescending(x => x.DateTime).Select(x => x.League).FirstOrDefault();
                a.OffensiveRatio = Convert.ToDouble(a.Shots) / Convert.ToDouble(a.Goals);
                a.DefensiveRatio = Convert.ToDouble(a.ShotsFaced) / Convert.ToDouble(a.GoalsConceded);
            }

            return data;
        }

        public List<LeagueShotStats> LeagueShotStatsAway()
        {
            var data =
                _context.LeagueData.Where(x => x.DateTime > new DateTime(2015, 07, 30))
                    .GroupBy(x => x.AwayTeam)
                    .Select(x => new LeagueShotStats
                    {
                        Team = x.Key,
                        Shots = x.Sum(y => y.AwayTeamShots),
                        ShotsFaced = x.Sum(y => y.HomeTeamShots),
                        Goals = x.Sum(y => y.FullTimeAwayGoals),
                        GoalsConceded = x.Sum(y => y.FullTimeHomeGoals),
                        OnTarget = x.Sum(y => y.AwayTeamShotsOnTarget),
                        ShotsFacedOnTarget = x.Sum(y => y.HomeTeamShotsOnTarget)
                    }).ToList();
            foreach (var a in data)
            {
                a.League = _context.LeagueData.Where(x => x.HomeTeam == a.Team).OrderByDescending(x => x.DateTime).Select(x => x.League).FirstOrDefault();
                a.OffensiveRatio = Convert.ToDouble(a.Shots) / Convert.ToDouble(a.Goals);
                a.DefensiveRatio = Convert.ToDouble(a.ShotsFaced) / Convert.ToDouble(a.GoalsConceded);
            }

            return data;
        }

    }
     public class TeamStats
    {
        public string Team { get; set; }
        public string League { get; set; }
        public string Location { get; set; }
        public DateTime Date { get; set; }
        public int GoalsFor { get; set; }
        public int GoalsAgainst { get; set; }
        public string FTResult { get; set; }
    }

    public class LeagueShotStats
    {
        public string Team { get; set; }
        public string League { get; set; }
        public int Shots { get; set; }
        public int ShotsFaced { get; set; }
        public int Goals { get; set; }
        public int GoalsConceded { get; set; }
        public int OnTarget { get; set; }
        public int ShotsFacedOnTarget { get; set; }
        public double OffensiveRatio { get; set; }
        public double DefensiveRatio { get; set; }
    }

    public class MatchResult
    {
        public DateTime DateTime { get; set; }
        public string HomeTeam { get; set; }
        public string AwayTeam { get; set; }
        public float HomeOdds { get; set; }
        public float DrawOdds { get; set; }
        public float AwayOdds { get; set; }
        public float OurHomeAdds { get; set; }
        public float OurAwayOdds { get; set; }
    }
    
}