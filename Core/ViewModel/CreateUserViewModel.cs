using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Core.Timing;

namespace Core.ViewModel
{
    public class CreateUserViewModel : IValidatableObject
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime CreationTIme { get; set; } = Clock.Now;
        public bool IsDeleted { get; set; } = false;
        public bool IsLocked { get; set; } = false;
        public DateTime? LastModificationTime { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string Designation { get; set; }
        public string EmployeeID { get; set; }
        public string Birthday { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (string.IsNullOrWhiteSpace(FirstName))
            {
                yield return new ValidationResult($"{nameof(FirstName)} is required");
            }

            if (string.IsNullOrWhiteSpace(LastName))
            {
                yield return new ValidationResult($"{nameof(LastName)} is required");
            }

            if (string.IsNullOrWhiteSpace(MiddleName))
            {
                yield return new ValidationResult($"{nameof(MiddleName)} is required");
            }

            if (string.IsNullOrWhiteSpace(Designation))
            {
                yield return new ValidationResult($"{nameof(Designation)} is required");
            }
        }
    }
}
