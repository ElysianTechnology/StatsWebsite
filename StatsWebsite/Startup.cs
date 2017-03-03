using System.Collections.Generic;
using System.Linq;
using FluentScheduler;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Nancy.Owin;
using Owin;
using StatsWebsite.Data_Context;
using StatsWebsite.Nancy;
using StatsWebsite.Nancy.Helper_Functions;
using Schedule = StatsWebsite.Nancy.Helper_Functions.Schedule;

namespace StatsWebsite
{
    public class Startup
    {
        public static DataRepository leagueRepo;
        public static AlgoTestContext context;
        public static Utils.CsvHandler<LeagueData, Utils.LeagueDataMap> csvHandler;
        public static List<MatchResult> newReturn;
        public void Configuration(IAppBuilder app)
        {
            var physicalFileSystem = new PhysicalFileSystem(@".\"); //. = root, Web = your physical directory that contains all other static content, see prev step
            var options = new FileServerOptions
            {
                EnableDefaultFiles = true,
                FileSystem = physicalFileSystem
            };
            options.StaticFileOptions.FileSystem = physicalFileSystem;
            options.StaticFileOptions.ServeUnknownFileTypes = true;
            app.UseFileServer(options);

            app.UseNancy(NancyConfig);
            app.UseStageMarker(PipelineStage.MapHandler);
            context = new AlgoTestContext();
            leagueRepo = new DataRepository(context);
            csvHandler = new Utils.CsvHandler<LeagueData, Utils.LeagueDataMap>();
            JobManager.Initialize(new Schedule.WeeklyRegistry()); 

        }

        public static void GetData()
        {
            
        }
        private static void NancyConfig(NancyOptions nancyOptions)
        {
            nancyOptions.Bootstrapper = new Bootstrapper();
        }
    }
}