using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using Nancy;
using StatsWebsite.Data_Context;

namespace StatsWebsite.Nancy.Modules
{
    public class TeamModule : NancyModule
    {
        public TeamModule() : base("/api")
        {
            Get["/teamlist/{division:int}"] = _ =>
            {
                int division = _.division;

                return Response.AsJson(Startup.leagueRepo.TeamsInLeague(division));
            };

            Get["/update/{year:int}"] = _ =>
            {
                var leagues = new[] {"E0", "E1", "E2", "E3", "EC"};
                //var leagues = new[] { "E0" };
                var fileUrl = string.Format("http://www.football-data.co.uk/mmz4281/{0}", _.year);
                foreach (var league in leagues)
                {
                    var results = Startup.csvHandler.DownloadFile(string.Format("{0}/{1}.csv", fileUrl, league)).Result;
                    foreach (var result in results)
                    {
                        Startup.leagueRepo.AddFixture(result);
                        Debug.WriteLine("Adding Fixture: " + result.HomeTeam + " v " + result.AwayTeam);
                    }
                }

                Startup.context.SaveChanges();
                return 200;
            };

            Get["/upcoming"] = _ =>
            {
                return Response.AsJson(Startup.leagueRepo.GetNextFixtures(0,4));
            };
        }
    }
}