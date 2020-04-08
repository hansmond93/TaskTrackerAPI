using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.ViewModel
{
    public class CloseProjectVM : BaseValidatableModel
    {
        public string ReasonForClosure { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(ReasonForClosure))
            {
                yield return new ValidationResult($"{nameof(ReasonForClosure)} cannot be empty");
            }
        }
    }
}
