using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Core.ViewModel
{
    public class UserUploadModel : BaseValidatableModel
    {
        public IFormFile File { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var validExt = new[] { ".xls", ".xlsx" };

            if (File is null || File.Length <= 0 || string.IsNullOrWhiteSpace(File.FileName))
                yield return new ValidationResult("Please upload a valid file.");
            else
            {
                var extension = Path.GetExtension(File.FileName).ToLower();

                if (!validExt.Any(x => x.Equals(extension)))
                    yield return new ValidationResult("Invalid file type. Please select a valid excel file");
            }
        }
    }
}
