using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StatsWebsite.Nancy.Helper_Functions
{
    public class Mapping
    {
        public static string ChangeName(string team)
        {
            switch (team)
            {
                default:
                    return "none";
                case "Arsenal FC":
                    return "Arsenal";
                case "AFC Bournemouth":
                    return "Bournemouth";
                case "Stoke City FC":
                    return "Stoke";
                case "Middlesbrough FC":
                    return "Middlesbrough";
                case "Manchester United FC":
                    return "Man United";
                case "Liverpool FC":
                    return "Liverpool";
                case "Leicester City FC":
                    return "Leicester";
                case "Hull City FC":
                    return "Hull";
                case "Sunderland AFC":
                    return "Sunderland";
                case "Manchester City FC":
                    return "Man City";
                case "Swansea City FC":
                    return "Swansea";
                case "Burnley FC":
                    return "Burnley";
                case "Tottenham Hotspur FC":
                    return "Tottenham";
                case "Everton FC":
                    return "Everton";
                case "Watford FC":
                    return "Watford";
                case "Southampton FC":
                    return "Southampton";
                case "West Bromwich Albion FC":
                    return "West Brom";
                case "Crystal Palace FC":
                    return "Crystal Palace";
                case "West Ham United FC":
                    return "West Ham";
                case "Chelsea FC":
                    return "Chelsea";
            }
        }
    }
}