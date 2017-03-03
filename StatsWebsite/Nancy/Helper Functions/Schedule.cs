using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using FluentScheduler;
using StatsWebsite.Data_Context;

namespace StatsWebsite.Nancy.Helper_Functions
{
    public class Schedule
    {
        public class WeeklyRegistry : Registry
        {
            public WeeklyRegistry()
            {
                Schedule<Daily>().ToRunNow().AndEvery(1).Days().At(0,0); // run WeeklyTask on a weekly basis           
            }
        }
        public class Daily : IJob
        {
            public void Execute()
            {
                var data = Startup.leagueRepo.GetNextFixtures(0, 4);
                Startup.newReturn = data.fixtures.Select(x => new MatchResult
                {
                    DateTime = x.date,
                    HomeTeam = x.homeTeamName,
                    AwayTeam = x.awayTeamName,
                    HomeOdds = x.odds.homeWin,
                    DrawOdds = x.odds.draw,
                    AwayOdds = x.odds.awayWin,
                    //OurPercents = PredictEngine.Predict(x.homeTeamName, x.awayTeamName, 0)
                }).ToList();
                Debug.WriteLine("Done");
            }
        }
    }
}