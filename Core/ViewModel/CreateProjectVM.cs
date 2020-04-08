using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.ViewModel
{
   public class CreateProjectVM: BaseValidatableModel
    {
        public string Name { get; set; }

        public string Code { get; set; }
        public string Description { get; set; }
        public bool IsRecurring { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                yield return new ValidationResult($"{nameof(Name)} is required");
            }

            if(string.IsNullOrWhiteSpace(Code))
            {
                yield return new ValidationResult($"{nameof(Code)} is required");
            }
        }
    }
}
