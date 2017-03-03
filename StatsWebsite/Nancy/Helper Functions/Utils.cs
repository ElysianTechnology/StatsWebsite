using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;
using StatsWebsite.Data_Context;

namespace StatsWebsite.Nancy.Helper_Functions
{
    public class Utils
    {
        public class CsvHandler<T, TMap> where TMap : CsvClassMap
        {
            public IEnumerable<T> ParseFile(string directory)
            {
                var files = Directory.GetFiles(directory, "*.*", SearchOption.AllDirectories);
                var records = new List<T>();
                foreach (var file in files)
                {
                    using (var sr = new StreamReader(file))
                    {
                        records.AddRange(GetRecords(sr));
                    }
                }

                return records;
            }

            public async Task<IEnumerable<T>> DownloadFile(string urlFilePath)
            {
                try
                {
                    using (var client = new HttpClient())
                    {
                        var response = await client.GetAsync(urlFilePath);
                        response.EnsureSuccessStatusCode();
                        var httpStream = await response.Content.ReadAsStreamAsync();

                        using (var sr = new StreamReader(httpStream))
                        {
                            return GetRecords(sr);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return new List<T>();
                }
            }

            private static IEnumerable<T> GetRecords(TextReader sr)
            {
                var reader = new CsvReader(sr);
                reader.Configuration.RegisterClassMap<TMap>();
                reader.Configuration.WillThrowOnMissingField = false;
                try
                {
                    var records = reader.GetRecords<T>().ToList();
                    return records;
                }
                catch (CsvTypeConverterException ex)
                {
                    Console.WriteLine(ex.Data["CsvHelper"]);
                    return new List<T>();
                }


            }
        }

        public sealed class LeagueDataMap : CsvClassMap<LeagueData>
        {
            public LeagueDataMap()
            {
                Map(m => m.League).Name("Div");
                Map(m => m.DateTime).Name("Date");
                Map(m => m.HomeTeam).Name("HomeTeam");
                Map(m => m.AwayTeam).Name("AwayTeam");
                Map(m => m.FullTimeHomeGoals).Name("FTHG");
                Map(m => m.FullTimeAwayGoals).Name("FTAG");
                Map(m => m.FullTimeResult).Name("FTR");
                Map(m => m.HalfTimeHomeGoals).Name("HTHG");
                Map(m => m.HalfTimeAwayGoals).Name("HTAG");
                Map(m => m.HalfTimeResult).Name("HTR");
                Map(m => m.HomeTeamShots).Name("HS");
                Map(m => m.AwayTeamShots).Name("AS");
                Map(m => m.HomeTeamShotsOnTarget).Name("HST");
                Map(m => m.AwayTeamShotsOnTarget).Name("AST");
                Map(m => m.HomeTeamFoulsCommited).Name("HF");
                Map(m => m.AwayTeamFoulsCommited).Name("AF");
                Map(m => m.HomeTeamCorners).Name("HC");
                Map(m => m.AwayTeamCorners).Name("AC");
                Map(m => m.HomeTeamYellowCards).Name("HY");
                Map(m => m.AwayTeamYellowCards).Name("AY");
                Map(m => m.HomeTeamRedCards).Name("HR");
                Map(m => m.AwayTeamRedCards).Name("AR");
            }
        }
    }
}