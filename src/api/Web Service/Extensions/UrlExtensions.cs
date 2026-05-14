namespace TemplateWebService.Extensions
{
    public static class UrlExtensions
    {
        public static string ToPublicUrl(
            this HttpRequest req,
            string relativeOrAbsolutePath,
            IConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(relativeOrAbsolutePath))
            {
                return null!;
            }

            if (Uri.TryCreate(relativeOrAbsolutePath, UriKind.Absolute, out var abs))
            {
                return abs.ToString();
            }

            // origin = scheme + host + pathBase
            var origin = $"{req.Scheme}://{req.Host}{req.PathBase}";

            // เธ–เนเธฒเน€เธเนเธ Development เนเธฅเธฐเธกเธต PublicBaseUrl เนเธซเน override origin
            var env = req.HttpContext.RequestServices.GetRequiredService<IWebHostEnvironment>();
            var devBase = config["WebServiceSettings:PublicBaseUrl"];

            if (env.IsDevelopment() && !string.IsNullOrWhiteSpace(devBase))
            {
                origin = devBase.TrimEnd('/');
            }

            var path = relativeOrAbsolutePath.Replace("\\", "/");

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            return origin + path;
        }

        public static string BuildPublicUrl(
            string baseUrl,
            string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
            {
                return null!;
            }

            if (Uri.TryCreate(relativePath, UriKind.Absolute, out var abs))
            {
                return abs.ToString();
            }

            var origin = baseUrl.TrimEnd('/');

            var path = relativePath.Replace("\\", "/");

            if (!path.StartsWith("/"))
            {
                path = "/" + path;
            }

            return origin + path;
        }
    }
}