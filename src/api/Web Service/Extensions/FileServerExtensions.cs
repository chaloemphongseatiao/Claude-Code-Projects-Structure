using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.FileProviders;

namespace TemplateWebService.Extensions
{
    public static class FileServerExtensions
    {
        public static IApplicationBuilder UseConfiguredStaticFiles(
            this IApplicationBuilder app,
            IConfiguration configuration,
            ILogger? logger = null)
        {
            var section = configuration.GetSection("WebServiceSettings:FileServer:FileSource");

            if (!section.Exists())
            {
                logger?.LogWarning("Config section 'WebServiceSettings:FileServer:FileSource' not found.");
                return app;
            }

            // Custom MIME
            var contentTypes = new FileExtensionContentTypeProvider();
            contentTypes.Mappings[".m4a"] = "audio/m4a";
            contentTypes.Mappings[".heic"] = "image/heic";

            var usedRequestPaths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var mountedRoot = false;

            foreach (var src in section.GetChildren())
            {
                var name = src.Key; // e.g. Default / Public
                var remotePath = src.GetValue<string>("RemotePath");
                var reqCfg = src.GetValue<string>("RequestPath");

                if (string.IsNullOrWhiteSpace(remotePath) || !Path.IsPathRooted(remotePath))
                {
                    logger?.LogError(
                        "FileSource '{Name}' has invalid RemotePath: '{RemotePath}'",
                        name,
                        remotePath ?? "<null>");

                    continue;
                }

                var fullPath = Path.GetFullPath(remotePath);

                if (!Directory.Exists(fullPath))
                {
                    Directory.CreateDirectory(fullPath);

                    logger?.LogWarning(
                        "FileSource '{Name}' directory not found. Created: {Path}",
                        name,
                        fullPath);
                }

                // Normalize request path
                PathString requestPath = string.IsNullOrWhiteSpace(reqCfg) || reqCfg == "/"
                    ? PathString.Empty
                    : new PathString(reqCfg.StartsWith('/') ? reqCfg : "/" + reqCfg);

                var requestPathKey = requestPath.HasValue
                    ? requestPath.ToString()
                    : "<root>";

                // Allow only one root mount
                if (requestPath == PathString.Empty)
                {
                    if (mountedRoot)
                    {
                        logger?.LogWarning(
                            "Skip '{Name}' because another FileSource already mounted at root.",
                            name);

                        continue;
                    }

                    mountedRoot = true;
                }
                else
                {
                    // Prevent duplicate /public etc.
                    if (!usedRequestPaths.Add(requestPathKey))
                    {
                        logger?.LogWarning(
                            "Duplicate RequestPath '{Req}' for source '{Name}'. Skipped.",
                            requestPathKey,
                            name);

                        continue;
                    }
                }

                app.UseStaticFiles(
                    new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(fullPath),
                        RequestPath = requestPath, // PathString.Empty = root
                        ContentTypeProvider = contentTypes,
                        ServeUnknownFileTypes = false,
                        OnPrepareResponse = ctx =>
                        {
                            ctx.Context.Response.Headers["Cache-Control"] = "public,max-age=86400";
                            ctx.Context.Response.Headers["X-Content-Type-Options"] = "nosniff";
                        }
                    });

                logger?.LogInformation(
                    "Mounted static files: {Name} => {Path} at {Req}",
                    name,
                    fullPath,
                    requestPath == PathString.Empty ? "<root>" : requestPath.ToString());
            }

            return app;
        }
    }
}