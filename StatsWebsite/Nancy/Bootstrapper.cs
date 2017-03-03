using Nancy;
using Nancy.Bootstrapper;
using Nancy.Conventions;
using Nancy.Json;
using Nancy.Responses.Negotiation;
using Nancy.TinyIoc;

namespace StatsWebsite.Nancy
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            Conventions.ViewLocationConventions.Add((viewName, model, context) => string.Concat(@"app/", viewName));
            JsonSettings.MaxJsonLength = int.MaxValue;

        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("build"));
            nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory("app/assets/img"));
        }

        protected override NancyInternalConfiguration InternalConfiguration
        {
            get
            {
                return NancyInternalConfiguration.WithOverrides(x => x.ResponseProcessors = new[]
                {
                    typeof(ViewProcessor),
                    typeof(JsonProcessor),
                    typeof(XmlProcessor)
                });
            }
        }
    }
}