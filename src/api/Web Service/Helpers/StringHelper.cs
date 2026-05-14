namespace TemplateWebService.Helpers
{
    public static class StringHelper
    {
        public static string ToCamelCase(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return value;

            if (char.IsLower(value[0]))
                return value;

            return char.ToLowerInvariant(value[0]) + value.Substring(1);
        }
    }
}