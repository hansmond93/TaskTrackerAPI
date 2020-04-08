using Core.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Core.Extensions
{
    public static class ValidationExtensions
    {
        public static bool IsValid<T>(this T source, out List<string> errorList) where T : BaseValidatableModel
        {
            var errors = new List<string>();

            var validationResults = source.Validate(new ValidationContext(source, null, null));

            errors.AddRange(validationResults.Select(e => e.ErrorMessage));

            errorList = errors;
            return !errors.Any();
        }
    }
}
