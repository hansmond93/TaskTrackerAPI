using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using Core.ViewModel;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Application.Interfaces
{
    public interface IUserService
    {
        //List<ValidationResult> Create(UserUploadExcelModel model);
        Task<List<ValidationResult>> UploadUsers(UserUploadModel model);
        Task<List<ValidationResult>> AddUser(UserViewModel model);
        Task<List<UserViewModel>> GetUsers();
        Task<UserViewModel> GetUser(int id);
        
        Task<bool> SendOTP(string email);
        Task<List<ValidationResult>> DeleteUser(int id);
        Task<List<ValidationResult>> BlockUser(int id); 
        Task<List<ValidationResult>> PasswordReset(ResetPasswordModel model);
        
    }
}
