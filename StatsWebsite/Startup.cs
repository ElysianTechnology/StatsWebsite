using Microsoft.Owin.Extensions;
using Microsoft.Owin.FileSystems;
using Microsoft.Owin.StaticFiles;
using Nancy.Owin;
using Owin;
using StatsWebsite.Nancy;

namespace StatsWebsite
{
    public class Startup
    {
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
        }

        private static void NancyConfig(NancyOptions nancyOptions)
        {
            nancyOptions.Bootstrapper = new Bootstrapper();
        }
    }
}