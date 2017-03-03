using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;
using StatsWebsite.Data_Context;

namespace StatsWebsite
{
    public class AlgoTestContext : DbContext
    {
        public AlgoTestContext()
            : base(
                string.Format(
                    @"Server={0};Database={1};User ID={2};Password={3};Trusted_Connection=False;Encrypt=True;Connection Timeout=30;",
                    DbInfo.DbServer, DbInfo.DbName, DbInfo.DbUser, DbInfo.DbPassword))
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.AddFromAssembly(typeof(LeagueDataEntityMap).Assembly);
        }

        public DbSet<LeagueData> LeagueData { get; set; }
    }
    public class LeagueDataEntityMap : EntityTypeConfiguration<LeagueData>
    {
        public LeagueDataEntityMap()
        {
            ToTable("LeagueData");
            HasKey(x => new { x.DateTime, x.HomeTeam, x.AwayTeam });
        }
    }
    public class DbInfo
    {
        public static string DbName = "CDM";
        public static string DbUser = "cdmuser";
        public static string DbServer = "txlyljg67d.database.windows.net,1433";
        public static string DbPassword = "M.yp@2sw()rD";
    }
}