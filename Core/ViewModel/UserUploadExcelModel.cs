using ExcelManager;
using Core.Utils;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace Core.ViewModel
{
    public class UserUploadExcelModel : BaseValidatableModel
    {
        [ExcelReaderCell("User Id")]
        public string Id { get; set; }
        [ExcelReaderCell("FirstName")]
        public string FirstName { get; set; }
        [ExcelReaderCell("LastName")]
        public string LastName { get; set; }
        [ExcelReaderCell("Middle Name")]
        public string MiddleName { get; set; }
        [ExcelReaderCell("Email")]
        public string Email { get; set; }
        [ExcelReaderCell("Birthday")]
        public string Birthday { get; set; }
        [ExcelReaderCell("Designation")]
        public string Designation { get; set; }
        [ExcelReaderCell("Employee ID")]
        public string EmployeeID { get; set; }
        [ExcelReaderCell("UserName")]
        public string UserName { get; set; }

        public override IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(Id))
            {
                yield return new ValidationResult($"{nameof(Email)} is invalid.");
            }

            if (string.IsNullOrWhiteSpace(Email) || !Email.IsValidEmail())
            {
                yield return new ValidationResult($"{nameof(Email)} is invalid.");
            }

            if (string.IsNullOrWhiteSpace(FirstName))
            {
                yield return new ValidationResult($"{nameof(FirstName)} is invalid.");
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult($"{nameof(LastName)} is invalid.");
            }
        }
    }
}
