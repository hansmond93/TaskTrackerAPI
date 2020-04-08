using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModel
{
    public abstract class BaseValidatableModel : IValidatableObject
    {
        public abstract IEnumerable<ValidationResult> Validate(ValidationContext validationContext);
    }
}