using TemplateWebService.Models.Settings;

namespace TemplateWebService.Helpers
{
    public static class FileSourceResolverExtensions
    {
        public static FileSourceDetail Resolve(
            this IReadOnlyDictionary<string, FileSourceDetail> sources,
            string name = "Default")
        {
            if (sources is null)
            {
                throw new ArgumentNullException(nameof(sources));
            }

            if (sources.Count == 0)
            {
                throw new InvalidOperationException("No file sources configured.");
            }

            return sources.TryGetValue(name, out var src)
                ? src
                : sources.Values.First();
        }

        public static string ResolveRootPath(
            this IReadOnlyDictionary<string, FileSourceDetail> sources,
            string name = "Default") =>
            Path.GetFullPath(sources.Resolve(name).RemotePath);

        public static PathString ToRequestPath(this FileSourceDetail src)
        {
            var rp = src.RequestPath;

            if (string.IsNullOrWhiteSpace(rp) || rp == "/")
            {
                return PathString.Empty;
            }

            return new PathString(rp.StartsWith('/') ? rp : "/" + rp);
        }
    }
}