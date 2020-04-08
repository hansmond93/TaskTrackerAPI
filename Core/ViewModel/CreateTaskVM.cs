using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Core.ViewModel
{
    public class CreateTaskVM: BaseValidatableModel
    {
        public string Description { get; set; }
        public byte Hours { get; set; }
        public byte Minutes { get; set; }

        public int ProjectId { get; set; }

        public static readonly int maximumDuration = 8;
        public static readonly int zeroDuration = 0;
        public static readonly int zeroProjectId = 0;
        


        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Description))
            {
                yield return new ValidationResult($"{nameof(Description)} is required");
            }

            if (Hours < zeroDuration || Minutes < zeroDuration)
            {
                yield return new ValidationResult($"{nameof(Hours)} or {nameof(Minutes)}cannot be less than {zeroDuration}");
            }

            if(Hours == zeroDuration && Minutes == zeroDuration)
            {
                yield return new ValidationResult($"{nameof(Hours)} or {nameof(Minutes)} cannot be {zeroDuration}");
            }


            if(ProjectId <= zeroProjectId)
            {
                yield return new ValidationResult($"Invalid {nameof(ProjectId)}");
            }
        }
    }
}
