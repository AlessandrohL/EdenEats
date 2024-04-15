using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace EdenEats.WebApi.Extensions
{
    public static class FluentValidationExtensions
    {
        public static void AddValidationToModelState
            (this ValidationResult result, ModelStateDictionary modelState)
        {
            foreach (var error in result.Errors)
            {
                modelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
        }
    }
}
