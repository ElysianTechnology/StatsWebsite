using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatsWebsite.Nancy.Models
{
    public class NextFixtures
    {
        public _Links _links { get; set; }
        public int count { get; set; }
        public Fixture[] fixtures { get; set; }
    }

    public class _Links
    {
        public Self self { get; set; }
        public Competition competition { get; set; }
    }

    public class Self
    {
        public string href { get; set; }
    }

    public class Competition
    {
        public string href { get; set; }
    }

    public class Fixture
    {
        public _Links1 _links { get; set; }
        public DateTime date { get; set; }
        public string status { get; set; }
        public int matchday { get; set; }
        public string homeTeamName { get; set; }
        public string awayTeamName { get; set; }
        public Result result { get; set; }
        public Odds odds { get; set; }
    }

    public class _Links1
    {
        public Self1 self { get; set; }
        public Competition1 competition { get; set; }
        public Hometeam homeTeam { get; set; }
        public Awayteam awayTeam { get; set; }
    }

    public class Self1
    {
        public string href { get; set; }
    }

    public class Competition1
    {
        public string href { get; set; }
    }

    public class Hometeam
    {
        public string href { get; set; }
    }

    public class Awayteam
    {
        public string href { get; set; }
    }

    public class Result
    {
        public object goalsHomeTeam { get; set; }
        public object goalsAwayTeam { get; set; }
    }

    public class Odds
    {
        public float homeWin { get; set; }
        public float draw { get; set; }
        public float awayWin { get; set; }
    }

}