using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Core.AspnetCore;
using Microsoft.AspNetCore.Mvc;
using Core.ViewModel;
using Core.Extensions;
using Core.AspNetCore;
using Application.Interfaces;
using ExcelManager;
using Core.ViewModel.Enums;
using System.IO;
using Entities;

namespace Web.Controllers
{
    [Route("api/v1/[controller]/[action]")]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        // GET: api/User
        [HttpGet]
        public async Task<ApiResponse<List<UserViewModel>>> Get()
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userService.GetUsers();
                return new ApiResponse<List<UserViewModel>>(result);
            });
        }

        // GET: api/User/5
        [HttpGet("{id}", Name = "Get")]
        public async Task<ApiResponse<UserViewModel>> Get(int id)
        {
            var user = await _userService.GetUser(id);
            if (user != null)
            {
                return new ApiResponse<UserViewModel>(user);
            }
            return new ApiResponse<UserViewModel>(errors: "User with this ID does not exist", codes: ApiResponseCodes.NOT_FOUND);
        }

        // POST: api/User
        [HttpPost]
        //[Route("UploadUser")]
        public async Task<ApiResponse<string>> UploadUser([FromForm]UserUploadModel model)
        {
            return await HandleApiOperationAsync(async () => {
                if (!ModelState.IsValid) 
                { 
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
                }

                var usersResult = await _userService.UploadUsers(model);
                if (usersResult.Any())
                {
                    return new ApiResponse<string>(errors: usersResult.Select(x => x.ErrorMessage).ToArray(), 
                        codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("Successfully uploaded users");
            });
        }

        [HttpPost]
        public async Task<ApiResponse<string>> CreateUser([FromBody]UserViewModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                if (!model.Birthday.HasValue)
                {
                    model.Birthday = null;
                }
                if (!ModelState.IsValid)
                {
                    return new ApiResponse<string>(errors: ListModelErrors, codes: ApiResponseCodes.INVALID_REQUEST);
                }
                var userResult = await _userService.AddUser(model);
                if (userResult.Any())
                {
                    return new ApiResponse<string>(errors: userResult.Select(x => x.ErrorMessage).ToArray(), codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("Successfully created user");
            });
        }
        
        [HttpGet]
        public async Task<ApiResponse<string>> GetOTP(string email)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userService.SendOTP(email);
                if (result)
                {
                    return new ApiResponse<string>("OTP sent to " + email);
                }
                return new ApiResponse<string>("No account exists with " + email, errors: "wrong email address", codes: ApiResponseCodes.ERROR);
            });
        }

        [HttpPost]
        public async Task<ApiResponse<string>> ResetPassword([FromForm]ResetPasswordModel model)
        {
            return await HandleApiOperationAsync(async () =>
            {
                var result = await _userService.PasswordReset(model);
                if (result.Any())
                {
                    return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray(),
                        codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("Successfully reset password for " + model.email);
            });
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<ApiResponse<string>> Delete(int id)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userService.DeleteUser(id);
                if (result.Any())
                {
                    return new ApiResponse<string>( errors: result.Select( x => x.ErrorMessage).ToArray(), codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("User set as deleted");
            });
        }

        [HttpPost("{id}")]
        public async Task<ApiResponse<string>> BlockUser(int id)
        {
            return await HandleApiOperationAsync(async () => {
                var result = await _userService.BlockUser(id);
                if (result.Any())
                {
                    return new ApiResponse<string>(errors: result.Select(x => x.ErrorMessage).ToArray(), codes: ApiResponseCodes.ERROR);
                }
                return new ApiResponse<string>("User set as locked");
            });
        }
    }
}
