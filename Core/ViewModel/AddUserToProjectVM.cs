using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.ViewModel
{
    public class AddUserToProjectVM : BaseValidatableModel
    {
        public int UserId { get; set; }
        public int ProjectId { get; set; }


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserId <= 0)
            {
                yield return new ValidationResult($"Invalid {nameof(UserId)}");
            }

            if (ProjectId <= 0)
            {
                yield return new ValidationResult($"Invalid {nameof(ProjectId)}");
            }
        }
    }
}
