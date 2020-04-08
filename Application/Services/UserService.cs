using System;
using System.Collections.Generic;
using Core.DataAccess.UnitOfWork;
using System.ComponentModel.DataAnnotations;
using Application.Interfaces;
using Core.ViewModel;
using Entities;
using Microsoft.AspNetCore.Identity;
using ExcelManager;
using Core.Extensions;
using System.Linq;
using System.Threading.Tasks;
using Core.Messaging.Email;
using Core.Messaging.Messages;
using Microsoft.EntityFrameworkCore;

namespace Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _usermgr;
        private readonly List<ValidationResult> errorResults = new List<ValidationResult>();
        private readonly  IMailService _smtpEmailService;
        private readonly IUnitOfWork _unitOfWOrk;
        private readonly ITotpService _totpService;
        public UserService(UserManager<User> usermgr, IMailService smtpEmailService, IUnitOfWork unitOfWork, ITotpService totpService
            )
        {
            _usermgr = usermgr;
            _smtpEmailService = smtpEmailService;
            _unitOfWOrk = unitOfWork;
            _totpService = totpService;
        }

        public async Task<List<UserViewModel>> GetUsers()
        {
            List<User> UsersAllProps =  _usermgr.Users.ToList();
            List<UserViewModel> Users= new List<UserViewModel>();
            await System.Threading.Tasks.Task.Run(() => {
                UsersAllProps.ForEach(u => {
                    Users.Add(new UserViewModel
                    {
                        Id = u.Id,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        MiddleName = u.MiddleName,
                        Birthday = u.Birthday,
                        Designation = u.Designation,
                        Email = u.Email,
                        EmployeeID = u.EmployeeID,
                        UserName = u.UserName
                    });
                });
            });
            return Users;
        }

        public async Task<UserViewModel> GetUser(int id)
        {
           User UserAllProps = await _usermgr.FindByIdAsync(id.ToString());
            UserViewModel UserView = default;
            if (UserAllProps != null)
            {
                UserView = new UserViewModel()
                {
                    Id = UserAllProps.Id,
                    FirstName = UserAllProps.FirstName,
                    LastName = UserAllProps.LastName,
                    MiddleName = UserAllProps.MiddleName,
                    Birthday = UserAllProps.Birthday,
                    Designation = UserAllProps.Designation,
                    Email = UserAllProps.Email,
                    EmployeeID = UserAllProps.EmployeeID,
                    UserName = UserAllProps.UserName
                };
            }
            
            return UserView;
        }

        /*
        public async Task<List<IdentityError>> Delete(int id)
        {
            List<IdentityError> errors = new List<IdentityError>() { };
            User user = await _usermgr.FindByIdAsync(id.ToString());
            var result = await _usermgr.DeleteAsync(user);
            if (!result.Succeeded)
            {
               errors = result.Errors.ToList();
            }
            return errors;
        }
        */
        
        public async Task<List<ValidationResult>> DeleteUser(int id)
        {
            User user = await _usermgr.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.IsDeleted = true;
                user.IsLocked = true;
                IdentityResult result = await _usermgr.UpdateAsync(user);
                List<IdentityError>  errors = result.Errors.ToList();
                if (errors.Any())
                {
                    foreach (IdentityError error in errors)
                    {
                        errorResults.Add(new ValidationResult(error.Description));
                    }
                    return errorResults;
                }
                else
                {
                    return errorResults;
                }
            }
            else
            {
                errorResults.Add(new ValidationResult("Invalid ID"));
                return errorResults;
            }
        }

        public async Task<List<ValidationResult>> BlockUser(int id)
        {
            User user = await _usermgr.FindByIdAsync(id.ToString());
            if (user != null)
            {
                user.IsLocked = true;
                IdentityResult result = await _usermgr.UpdateAsync(user);
                List<IdentityError>  errors = result.Errors.ToList();
                if (errors.Any())
                {
                    foreach (IdentityError error in errors)
                    {
                        errorResults.Add(new ValidationResult(error.Description));
                    }
                    return errorResults;
                }
                else
                {
                    return errorResults;
                }
            }
            else
            {
                errorResults.Add(new ValidationResult("Invalid ID"));
                return errorResults;
            }
        }
        
        public async Task<bool> SendOTP(string email)
        {
            User user = await _usermgr.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
            if (user == null)
            {
                return false;
            }
            int otp = _totpService.Generate(user.Email);

            _smtpEmailService.SendMail(new Mail("dev.test@sbsc.com", "TimeTrackR reset your password", user.Email)
            {
                Body = NewAccountMessages.OtpSentMessage + otp
            });
            return true;
        }

        public async Task<List<ValidationResult>> PasswordReset(ResetPasswordModel model)
        {
            User user = _usermgr.Users.FirstOrDefault(u => u.Email.ToLower() == model.email.ToLower());
            if (user == null)
            {
                errorResults.Add(new ValidationResult("No user account exists with this email address"));
                return errorResults;
            }
            var confirmationResult = _totpService.Verify(model.otp, user.Email);
            if (confirmationResult)
            {
                string resetToken = await _usermgr.GeneratePasswordResetTokenAsync(user);
                IdentityResult result = await _usermgr.ResetPasswordAsync(user, resetToken, model.password);
                List<IdentityError> errors = result.Errors.ToList();
                if (errors.Any())
                {
                    foreach (IdentityError error in errors)
                    {
                        errorResults.Add(new ValidationResult(error.Description));
                    }
                    return errorResults;
                }
                else
                {
                    return errorResults;
                }
            }
            else
            {
                errorResults.Add(new ValidationResult("The OTP couldn't be verified with this mail address"));
                return errorResults;
            }
        }
        
        public async Task<List<ValidationResult>> UploadUsers(UserUploadModel model) { 
            if (!IsValid(model)) 
                return errorResults;

            try
            {
                var users = new ExcelReader(model.File.OpenReadStream()).ReadAllSheets<UserUploadExcelModel>();
                var lineNo = 1;
                foreach (var u in users)
                {
                    ++lineNo;
                    if (!u.IsValid(out List<string> err))
                    {
                        errorResults.Add(new ValidationResult($"error at line# {lineNo} : => {string.Join("|", err)}"));
                        continue;
                    }
                    else
                    {
                        var user = new User()
                        {
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                            MiddleName = u.MiddleName,
                            Birthday = DateTime.Parse(u.Birthday),
                            Designation = u.Designation,
                            EmployeeID = u.EmployeeID,
                            Email = u.Email,
                            UserName = u.UserName
                        };
                        var result = await _usermgr.CreateAsync(user);
                        List<IdentityError> errors = result.Errors.ToList();
                        if (result.Errors.Any())
                        {
                            foreach (IdentityError error in errors)
                            {
                                errorResults.Add(new ValidationResult(error.Description));
                            }
                        }
                        else
                        {
                            _smtpEmailService.SendMail(new Mail("dev.test@sbsc.com", "Activate your account", u.Email)
                            {
                                Body = NewAccountMessages.NewSignUpMessage
                            });
                        }
                    }

                }
                return errorResults;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        public async Task<List<ValidationResult>> AddUser(UserViewModel model)
        {
            if (!IsValid(model))
                return errorResults;
            User user = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                MiddleName = model.MiddleName,
                Birthday = model.Birthday,
                Designation = model.Designation,
                EmployeeID = model.EmployeeID,
                Email = model.Email,
                UserName = model.UserName
            };
            var result = await _usermgr.CreateAsync(user);
            List<IdentityError> errors = result.Errors.ToList();
            if (result.Errors.Any())
            {
                foreach (IdentityError error in errors)
                {
                    errorResults.Add(new ValidationResult(error.Description));
                }
            }
            else
            {
                _smtpEmailService.SendMail(new Mail("dev.test@sbsc.com", "Activate your account", model.Email)
                {
                    Body = NewAccountMessages.NewSignUpMessage
                });
            }
            return errorResults;
        }
        protected bool IsValid<T>(T entity)
        {
            return Validator.TryValidateObject(entity, new ValidationContext(entity, null, null), errorResults, false);

        }
    }
}
