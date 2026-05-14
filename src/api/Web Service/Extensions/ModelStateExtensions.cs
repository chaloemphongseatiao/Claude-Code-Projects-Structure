using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TemplateWebService.Extensions
{
    public static class ModelStateExtensions
    {
        public static string GetErrorMessages(
            this ModelStateDictionary modelState,
            string separator = " | ") =>
            string.Join(
                separator,
                modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));

        public static List<string> GetErrorMessagesList(
            this ModelStateDictionary modelState) =>
            modelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
    }
}

