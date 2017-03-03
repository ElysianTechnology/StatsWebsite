using Nancy;

namespace StatsWebsite.Nancy.Modules
{
    public class Home
    {
        public class HomeModule : NancyModule
        {
            public HomeModule()
            {
                Get["/"] = _ => View["Index"];
            }
        }
    }
}